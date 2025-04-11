using CSharpFunctionalExtensions;
using NotificationService.Core.Options;
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
    private long? _chatId;

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
        var dbChatId = await _repository.GetTelegramChatId(userId, CancellationToken.None);
        if (dbChatId is null)
            return Errors.General.Failure($"Telegram chatId of user: {userId} is null");

        try
        {
            await _botClient.SendMessage(dbChatId, message);
            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            return Errors.General.Failure(ex.Message);
        }
    }
    
    public async Task<UnitResult<Error>> StartRegisterChatId(Guid userId)
    {
        var notificationSettingsExist = await _repository.Get(userId, CancellationToken.None);
        if (notificationSettingsExist is null)
            return Errors.General.Failure("UserNotificationSettings is null");
        
        if (notificationSettingsExist.TelegramChatId is not null)
            return Result.Success<Error>();

        try
        {
            _botClient.OnMessage += AddUserChatIdToDb;

            //Ожидание получения номера чата с пользователем
            while (_chatId is null)
                await Task.Delay(100);
            
            notificationSettingsExist.SetTelegramChatId((long)_chatId);
        
            var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
            await _unitOfWork.SaveChanges(CancellationToken.None);
            transaction.Commit();
            return Result.Success<Error>();
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
        StopRegisterChatId();
        _chatId = message.Chat.Id;
    }
}