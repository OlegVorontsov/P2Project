using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Application.Messaging;
using P2Project.Application.Shared.Dtos.Files;
using P2Project.Application.Volunteers.Commands.AddPetPhotos;
using P2Project.Domain.PetManagment.ValueObjects.Files;
using P2Project.Domain.Shared.Errors;
using P2Project.UnitTestsFabrics;
using FileInfo = P2Project.Application.FileProvider.Models.FileInfo;
using Result = CSharpFunctionalExtensions.Result;

namespace P2Project.Application.UnitTests
{
    public class UploadFilesToPetTests
    {
        private readonly Mock<IValidator<AddPetPhotosCommand>> _validatorMock = new();
        private readonly Mock<IFileProvider> _fileProviderMock = new();
        private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ILogger<AddPetPhotosHandler>> _loggerMock = new();
        private readonly Mock<IMessageQueue<IEnumerable<FileInfo>>> _messageQueueMock = new();

        [Fact]
        public async Task Upload_Files_To_Pet()
        {
            // arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteer = VolunteerFabric.CreateVolunteer();
            var pet = PetFabric.CreatePet();
            volunteer.AddPet(pet);

            var stream = new MemoryStream();
            var fileName = "test.jpg";
            var uploadFileDto = new UploadFileDto(stream, fileName);

            var command = new AddPetPhotosCommand(
                    volunteer.Id.Value,
                    pet.Id.Value,
                    [uploadFileDto, uploadFileDto]);

            _validatorMock.Setup(v => v.ValidateAsync(
                command, cancellationToken))
                .ReturnsAsync(new ValidationResult());

            var extension = Path.GetExtension(uploadFileDto.FileName);

            List<FilePath> filePaths =
            [
                FilePath.Create(Guid.NewGuid(), extension).Value,
                FilePath.Create(Guid.NewGuid(), extension).Value
            ];

            _fileProviderMock.Setup(f => f.UploadFiles(
                It.IsAny<List<FileData>>(),
                cancellationToken))
                .ReturnsAsync(
                    Result.Success<IReadOnlyList<FilePath>,
                    Error>(filePaths));

            _volunteersRepositoryMock.Setup(r => r.GetById(
                volunteer.Id, cancellationToken)).ReturnsAsync(volunteer);

            _unitOfWorkMock.Setup(u => u.SaveChanges(cancellationToken))
                .Returns(Task.CompletedTask);

            var handler = new AddPetPhotosHandler(
                _validatorMock.Object,
                _fileProviderMock.Object,
                _volunteersRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _messageQueueMock.Object);

            // act
            var uploadResult = await handler.Handle(
                command, cancellationToken);
            var filesCount = volunteer.Pets
                .Where(p => p.Id == pet.Id).First().Photos.Count;

            // assert
            uploadResult.IsSuccess.Should().BeTrue();
            uploadResult.Value.Should().Be(pet.Id.Value);
            filesCount.Should().Be(2);
        }
    }
}