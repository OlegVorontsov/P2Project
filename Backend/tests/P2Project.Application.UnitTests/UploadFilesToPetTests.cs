using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Shared;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Volunteers;
using P2Project.Application.Volunteers.UploadFilesToPet;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.UnitTestsFabrics;
using Result = CSharpFunctionalExtensions.Result;

namespace P2Project.Application.UnitTests
{
    public class UploadFilesToPetTests
    {
        [Fact]
        public async Task Handle_Should_Upload_Files_To_Pet()
        {
            // arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteer = VolunteerFabric.CreateVolunteer();
            var pet = PetFabric.CreatePet();
            volunteer.AddPet(pet);

            var stream = new MemoryStream();
            var fileName = "test.jpg";
            var uploadFileDto = new UploadFileDto(stream, fileName);

            var command = new UploadFilesToPetCommand(
                    volunteer.Id.Value,
                    pet.Id.Value,
                    [uploadFileDto, uploadFileDto]);

            var validatorMock = new Mock<IValidator<UploadFilesToPetCommand>>();
            validatorMock.Setup(v => v.ValidateAsync(
                command, cancellationToken))
                .ReturnsAsync(new ValidationResult());

            var fileProviderMock = new Mock<IFileProvider>();

            var extension = Path.GetExtension(uploadFileDto.FileName);

            List<FilePath> filePaths =
            [
                FilePath.Create(Guid.NewGuid(), extension).Value,
                FilePath.Create(Guid.NewGuid(), extension).Value
            ];

            fileProviderMock.Setup(f => f.UploadFiles(
                It.IsAny<List<FileData>>(),
                cancellationToken))
                .ReturnsAsync(
                    Result.Success<IReadOnlyList<FilePath>,
                    Error>(filePaths));

            var volunteerRepositoryMock = new Mock<IVolunteersRepository>();

            volunteerRepositoryMock.Setup(r => r.GetById(
                volunteer.Id, cancellationToken)).ReturnsAsync(volunteer);

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(u => u.SaveChanges(cancellationToken))
                .Returns(Task.CompletedTask);

            var logger = LoggerFactory.Create(b => b.AddConsole())
                .CreateLogger<UploadFilesToPetHandler>();

            var handler = new UploadFilesToPetHandler(
                validatorMock.Object,
                fileProviderMock.Object,
                volunteerRepositoryMock.Object,
                unitOfWorkMock.Object,
                logger);

            // act
            var uploadResult = await handler.Handle(
                command, cancellationToken);
            var filesCount = volunteer.Pets
                .Where(p => p.Id == pet.Id).First().Photos.PetPhotos.Count;

            // assert
            uploadResult.IsSuccess.Should().BeTrue();
            uploadResult.Value.Should().Be(pet.Id.Value);
            filesCount.Should().Be(2);
        }
    }
}