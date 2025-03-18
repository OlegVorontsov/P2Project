using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Requests.AmazonS3;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Dtos.Pets;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Queries;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Queries.Pets.GetPetById;

public class GetPetByIdHandler :
    IQueryValidationHandler<PetDto, GetPetByIdQuery>
{
    private readonly IValidator<GetPetByIdQuery> _validator;
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;
    private readonly IFilesHttpClient _httpClient;
    private readonly ILogger<GetPetByIdHandler> _logger;

    public GetPetByIdHandler(
        IValidator<GetPetByIdQuery> validator,
        IVolunteersReadDbContext volunteersReadDbContext,
        IFilesHttpClient httpClient,
        ILogger<GetPetByIdHandler> logger)
    {
        _validator = validator;
        _volunteersReadDbContext = volunteersReadDbContext;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<PetDto, ErrorList>> Handle(
        GetPetByIdQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var petDto = await _volunteersReadDbContext.Pets
            .FirstOrDefaultAsync(p => p.Id == query.PetId, cancellationToken);

        if (petDto is null)
        {
            _logger.LogWarning("Failed to query pet with id {id}", query.PetId);
            return Errors.General.NotFound(query.PetId).ToErrorList();
        }
        
        var getAvatarUrlResult = await _httpClient.GetPresignedUrl(
            petDto.Avatar.FileName,
            new GetPresignedUrlRequest(petDto.Avatar.BucketName),
            cancellationToken);
        if (getAvatarUrlResult.IsSuccess)
            petDto.AvatarUrl = getAvatarUrlResult.Value.Url;
        
        List<string> photosUrls = [];
        foreach (var photo in petDto.Photos)
        {
            var getPresignedPhotoUrlRequest = new GetPresignedUrlRequest(Constants.BUCKET_NAME_PHOTOS);
            var getPhotoUrlResult = await _httpClient.GetPresignedUrl(
                photo.FileName,
                getPresignedPhotoUrlRequest,
                cancellationToken);
            if(getPhotoUrlResult.IsSuccess)
                photosUrls.Add(getPhotoUrlResult.Value.Url);
        }
        petDto.PhotosUrls = photosUrls;

        _logger.LogInformation("Pet with id {id} queried", query.PetId);

        return petDto;
    }
}