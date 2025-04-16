using Quartz;

namespace P2Project.Core.Outbox.ProcessMessages;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ProcessOutboxMessagesService _outboxMessagesService;

    public ProcessOutboxMessagesJob(ProcessOutboxMessagesService outboxMessagesService)
    {
        _outboxMessagesService = outboxMessagesService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _outboxMessagesService.Execute(context.CancellationToken);
    }
}