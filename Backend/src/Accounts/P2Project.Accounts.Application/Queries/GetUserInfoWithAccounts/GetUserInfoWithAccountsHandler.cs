using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Core.Dtos.Accounts;
using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Caching;
using P2Project.Core.Interfaces.Queries;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Queries.GetUserInfoWithAccounts;

public class GetUserInfoWithAccountsHandler :
    IQueryValidationHandler<UserDto, GetUserInfoWithAccountsQuery>
{
    private readonly IAccountsReadDbContext _readDbContext;
    private readonly IValidator<GetUserInfoWithAccountsQuery> _validator;
    private readonly ICacheService _cache;
    private readonly IFilesHttpClient _httpClient;

    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Constants.CacheConstants.DEFAULT_EXPIRATION_MINUTES),
    };

    public GetUserInfoWithAccountsHandler(
        IAccountsReadDbContext readDbContext,
        IValidator<GetUserInfoWithAccountsQuery> validator,
        ICacheService cache,
        IFilesHttpClient httpClient)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _cache = cache;
        _httpClient = httpClient;
    }

    public async Task<Result<UserDto, ErrorList>> Handle(
        GetUserInfoWithAccountsQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var key = Constants.CacheConstants.USERS_PREFIX + query.Id;
        var userInfo = await _cache.GetOrSetAsync(
            key,
            _cacheOptions,
            async () => await GetUserInfoDto(query, cancellationToken),
            cancellationToken);
        if (userInfo is null)
            return Errors.General.NotFound(query.Id).ToErrorList();

        if (userInfo.Avatar != null)
        {
            var getAvatarUrlResult = await _httpClient.GetPresignedUrl(
                userInfo.Avatar.FileName,
                new GetPresignedUrlRequest(userInfo.Avatar.BucketName),
                cancellationToken);
            if (getAvatarUrlResult.IsSuccess)
                userInfo.AvatarUrl = getAvatarUrlResult.Value.Url;
        }

        if (!userInfo.Photos.Any()) return userInfo;

        List<string> photosUrls = [];
        foreach (var photo in userInfo.Photos)
        {
            var getPresignedPhotoUrlRequest = new GetPresignedUrlRequest(Constants.BUCKET_NAME_PHOTOS);
            var getPhotoUrlResult = await _httpClient.GetPresignedUrl(
                photo.FileName,
                getPresignedPhotoUrlRequest,
                cancellationToken);
            if(getPhotoUrlResult.IsSuccess)
                photosUrls.Add(getPhotoUrlResult.Value.Url);
        }

        userInfo.PhotosUrls = photosUrls;

        return userInfo;
    }

    private async Task<UserDto?> GetUserInfoDto(
        GetUserInfoWithAccountsQuery query,
        CancellationToken cancellationToken)
    {
        var userDto = await _readDbContext.Users
            .Include(u => u.AdminAccount)
            .Include(u => u.VolunteerAccount)
            .Include(u => u.ParticipantAccount)
            .FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);
        if (userDto == null) return null;

        ParticipantAccountDto? participantAccountDto = null;
        VolunteerAccountDto? volunteerAccountDto = null;
        AdminAccountDto? adminAccountDto = null;

        if (userDto.ParticipantAccount != null)
        {
            participantAccountDto = new ParticipantAccountDto();
            participantAccountDto.ParticipantAccountId = userDto.ParticipantAccount.ParticipantAccountId;
            participantAccountDto.UserId = userDto.ParticipantAccount.UserId;
            participantAccountDto.BannedForRequestsUntil = userDto.ParticipantAccount!.BannedForRequestsUntil;
        }

        if (userDto.VolunteerAccount != null)
        {
            volunteerAccountDto = new VolunteerAccountDto();
            volunteerAccountDto.VolunteerAccountId = userDto.VolunteerAccount.VolunteerAccountId;
            volunteerAccountDto.UserId = userDto.VolunteerAccount.UserId;
            volunteerAccountDto.Experience = userDto.VolunteerAccount.Experience;
            volunteerAccountDto.AssistanceDetails = userDto.VolunteerAccount.AssistanceDetails;
            volunteerAccountDto.Certificates = userDto.VolunteerAccount.Certificates;
        }

        if (userDto.AdminAccount != null)
        {
            adminAccountDto = new AdminAccountDto();
            adminAccountDto.AdminAccountId = userDto.AdminAccount.AdminAccountId;
            adminAccountDto.UserId = userDto.AdminAccount.UserId;
        }

        var photos = new List<MediaFileDto>();
        if (userDto.Photos.Any())
        {
            photos = userDto.Photos.Select(p => new MediaFileDto(
                p.BucketName,
                p.FileKey,
                p.FileName,
                p.FileType,
                p.IsMain)).ToList();
        }

        var userInfoDto = new UserDto()
        {
            Id = userDto.Id,
            FirstName = userDto.FirstName,
            SecondName = userDto.SecondName,
            LastName = userDto.LastName,
            UserName = userDto.UserName,
            Email = userDto.Email,
            SocialNetworks = userDto.SocialNetworks
                .Select(s => new SocialNetworkDto(s.Name, s.Link)).ToList(),
            Avatar = userDto.Avatar,
            Photos = photos,
            AdminAccount = adminAccountDto,
            VolunteerAccount = volunteerAccountDto,
            ParticipantAccount = participantAccountDto,
        };

        return userInfoDto;
    }
}