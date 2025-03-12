using FilesService.Core.Dtos;
using FilesService.Core.Interfaces;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using P2Project.Core.Interfaces;
using P2Project.SharedKernel.Errors;
using P2Project.UnitTestsFabrics;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Application.Commands.AddPetPhotos;
using P2Project.Volunteers.Domain;
using Result = CSharpFunctionalExtensions.Result;

namespace P2Project.Application.UnitTests
{
    public class UploadFilesToPetTests
    {
        private readonly IValidator<AddPetPhotosCommand> _validator =
            Substitute.For<IValidator<AddPetPhotosCommand>>();
        private readonly IFilesHttpClient _httpClient =
            Substitute.For<IFilesHttpClient>();
        private readonly IVolunteersRepository _volunteersRepository =
            Substitute.For<IVolunteersRepository>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly ILogger<AddPetPhotosHandler> _logger =
            Substitute.For<ILogger<AddPetPhotosHandler>>();
        private readonly IMessageQueue<IEnumerable<FileInfoDto>> _messageQueue =
            Substitute.For<IMessageQueue<IEnumerable<FileInfoDto>>>();
        private readonly CancellationToken _cancellationToken = 
            new CancellationTokenSource().Token;

        [Fact]
        public async Task Upload_Files_To_Pet()
        {
            // arrange
            var volunteer = VolunteerFabric.CreateVolunteer();
            var pet = PetFabric.CreatePet();
            volunteer.AddPet(pet);
            
            _volunteersRepository.GetById(volunteer.Id, _cancellationToken)
                .Returns(Result.Success<Volunteer, Error>(volunteer));
            _unitOfWork.SaveChanges(_cancellationToken).Returns(Task.CompletedTask);

            //var stream = new MemoryStream();
            var bucketName = "testBucket";
            var fileName = "test.jpg";
            var contentType = "image/jpeg";
            var size = 100000;
            var uploadFileRequest = new StartMultipartUploadRequest(
                bucketName, fileName, contentType, size);

            var command = new AddPetPhotosCommand(
                    volunteer.Id.Value,
                    pet.Id.Value,
                    [uploadFileRequest]);
            
            var extension = Path.GetExtension(uploadFileRequest.FileName);

            /*List<FilePath> filePaths =
            [
                FilePath.Create(Guid.NewGuid(), extension).Value,
                FilePath.Create(Guid.NewGuid(), extension).Value
            ];*/
            var response = new UploadPartFileResponse("key", "response.UploadId");

            _httpClient.StartMultipartUpload(
                    Arg.Any<StartMultipartUploadRequest>(), _cancellationToken)
                .Returns(response);

            _validator.ValidateAsync(Arg.Any<AddPetPhotosCommand>(), _cancellationToken)
                .Returns(new ValidationResult());

            var handler = new AddPetPhotosHandler(
                _validator,
                _httpClient,
                _volunteersRepository,
                _unitOfWork,
                _logger,
                _messageQueue);

            // act
            var uploadResult = await handler.Handle(
                command, _cancellationToken);
            var filesCount = volunteer.Pets
                .Where(p => p.Id == pet.Id).First().Photos.Count;

            // assert
            uploadResult.IsSuccess.Should().BeTrue();
            filesCount.Should().Be(2);
        }
    }
}