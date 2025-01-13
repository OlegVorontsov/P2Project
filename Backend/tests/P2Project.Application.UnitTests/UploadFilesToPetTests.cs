using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using P2Project.Core.Dtos.Files;
using P2Project.Core.Errors;
using P2Project.Core.Files;
using P2Project.Core.Files.Models;
using P2Project.Core.Interfaces;
using P2Project.Core.Messaging;
using P2Project.Core.ValueObjects;
using P2Project.UnitTestsFabrics;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Application.Commands.AddPetPhotos;
using P2Project.Volunteers.Domain;
using FileInfo = P2Project.Core.Files.Models.FileInfo;
using Result = CSharpFunctionalExtensions.Result;

namespace P2Project.Application.UnitTests
{
    public class UploadFilesToPetTests
    {
        private readonly IValidator<AddPetPhotosCommand> _validator =
            Substitute.For<IValidator<AddPetPhotosCommand>>();
        private readonly IFileProvider _fileProvider =
            Substitute.For<IFileProvider>();
        private readonly IVolunteersRepository _volunteersRepository =
            Substitute.For<IVolunteersRepository>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly ILogger<AddPetPhotosHandler> _logger =
            Substitute.For<ILogger<AddPetPhotosHandler>>();
        private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue =
            Substitute.For<IMessageQueue<IEnumerable<FileInfo>>>();
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

            var stream = new MemoryStream();
            var fileName = "test.jpg";
            var uploadFileDto = new UploadFileDto(stream, fileName);

            var command = new AddPetPhotosCommand(
                    volunteer.Id.Value,
                    pet.Id.Value,
                    [uploadFileDto, uploadFileDto]);
            
            var extension = Path.GetExtension(uploadFileDto.FileName);

            List<FilePath> filePaths =
            [
                FilePath.Create(Guid.NewGuid(), extension).Value,
                FilePath.Create(Guid.NewGuid(), extension).Value
            ];

            _fileProvider.UploadFiles(
                    Arg.Any<IEnumerable<FileData>>(), _cancellationToken)
                .Returns(Result.Success<IReadOnlyList<FilePath>, Error>(filePaths));

            _validator.ValidateAsync(Arg.Any<AddPetPhotosCommand>(), _cancellationToken)
                .Returns(new ValidationResult());

            var handler = new AddPetPhotosHandler(
                _validator,
                _fileProvider,
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