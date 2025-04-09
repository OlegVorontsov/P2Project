using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Requests.AmazonS3;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Core.Dtos.Accounts;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Queries;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Queries.GetUserInfoWithAccounts;

public class GetUserInfoWithAccountsHandler :
    IQueryValidationHandler<UserDto, GetUserInfoWithAccountsQuery>
{
    private readonly IAccountsReadDbContext _readDbContext;
    private readonly IValidator<GetUserInfoWithAccountsQuery> _validator;
    private readonly IFilesHttpClient _httpClient;

    public GetUserInfoWithAccountsHandler(
        IAccountsReadDbContext readDbContext,
        IValidator<GetUserInfoWithAccountsQuery> validator,
        IFilesHttpClient httpClient)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _httpClient = httpClient;
    }

    public async Task<Result<UserDto, ErrorList>> Handle(
        GetUserInfoWithAccountsQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var userDto = await _readDbContext.Users
            .Include(u => u.AdminAccount)
            .Include(u => u.VolunteerAccount)
            .Include(u => u.ParticipantAccount)
            .FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);
        
        if (userDto is null)
            return Errors.General.NotFound(query.Id).ToErrorList();

        if (userDto.Avatar != null)
        {
            var getAvatarUrlResult = await _httpClient.GetPresignedUrl(
                userDto.Avatar.FileName,
                new GetPresignedUrlRequest(userDto.Avatar.BucketName),
                cancellationToken);
            if (getAvatarUrlResult.IsSuccess)
                userDto.AvatarUrl = getAvatarUrlResult.Value.Url;
        }

        if (!userDto.Photos.Any()) return userDto;
        
        List<string> photosUrls = [];
        foreach (var photo in userDto.Photos)
        {
            var getPresignedPhotoUrlRequest = new GetPresignedUrlRequest(Constants.BUCKET_NAME_PHOTOS);
            var getPhotoUrlResult = await _httpClient.GetPresignedUrl(
                photo.FileName,
                getPresignedPhotoUrlRequest,
                cancellationToken);
            if(getPhotoUrlResult.IsSuccess)
                photosUrls.Add(getPhotoUrlResult.Value.Url);
        }
        userDto.PhotosUrls = photosUrls;
        
        return userDto;
    }
}