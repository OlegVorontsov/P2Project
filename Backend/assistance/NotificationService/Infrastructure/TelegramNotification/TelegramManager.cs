using CSharpFunctionalExtensions;
using NotificationService.Core.Options;
using NotificationService.Domain;
using NotificationService.Domain.ValueObjects;
using NotificationService.Infrastructure.Repositories;
using P2Project.SharedKernel.Errors;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotificationService.Infrastructure.TelegramNotification;

public class TelegramManager
{
    private readonly TelegramBotClient _botClient;
    private readonly NotificationRepository _repository;
    private readonly UnitOfWork _unitOfWork;
    private string? _telegramUserId;
    private long? _telegramChatId;
    private int? MAX_SECONDS_TO_INITIALIZE = 500;
    private int? MAX_TRIES_TO_INITIALIZE = 3;

    public TelegramManager(
        IConfiguration configuration,
        UnitOfWork unitOfWork,
        NotificationRepository repository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;

        var telegramOptions = configuration.GetSection(TelegramOptions.SECTION_NAME).Get<TelegramOptions>();
        _botClient = new TelegramBotClient(telegramOptions!.API);
    }
    
    public async Task<UnitResult<Error>> SendMessage(Guid userId, string message)
    {
        var telegramSettings = await _repository.GetTelegramSettings(userId, CancellationToken.None);
        if (telegramSettings is null)
            return Errors.General.Failure($"TelegramSettings user's: {userId} is null");

        try
        {
            await _botClient.SendMessage(telegramSettings.ChatId, message);
            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            return Errors.General.Failure(ex.Message);
        }
    }
    
    public async Task<Result<UserNotificationSettings, Error>> StartRegisterChatId(Guid userId, string telegramUserId)
    {
        var notificationSettingsExist = await _repository.Get(userId, CancellationToken.None);
        if (notificationSettingsExist is null)
            return Errors.General.Failure("UserNotificationSettings is null");
        
        if (notificationSettingsExist.TelegramSettings is not null &&
            notificationSettingsExist.TelegramSettings!.UserId == telegramUserId)
            return notificationSettingsExist;

        try
        {
            _telegramUserId = telegramUserId;
            _botClient.OnMessage += AddUserChatIdToDb;

            //Ожидание получения номера чата с пользователем
            var secondsCount = 0;
            var tries = 0;
            while (_telegramChatId is null)
            {
                await Task.Delay(1000);
                secondsCount++;
                tries++;

                if(secondsCount > MAX_SECONDS_TO_INITIALIZE || tries > MAX_TRIES_TO_INITIALIZE)
                    return Errors.General.Failure("Fail to register telegramChatId");
            }

            
            notificationSettingsExist.SetTelegramSettings(new TelegramSettings(_telegramUserId, _telegramChatId.Value));
        
            var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
            await _unitOfWork.SaveChanges(CancellationToken.None);
            transaction.Commit();
            return notificationSettingsExist;
        }
        catch (Exception ex)
        {
            return Errors.General.Failure(ex.Message);
        }
    }

    private void StopRegisterChatId() => _botClient.OnMessage -= AddUserChatIdToDb;

    private async Task AddUserChatIdToDb(
        Message message, UpdateType type)
    {
        if (message.Chat.Username != _telegramUserId)
            return;
        
        StopRegisterChatId();
        _telegramChatId = message.Chat.Id;
    }
}