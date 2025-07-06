using Common.Dto;
using Service.interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
//קובץ עבור ביטול אוטוטמי אם מתנדב לא הגיב לבקשה תוך זמן מסוים
public class ScheduledCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ScheduledCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var messageService = scope.ServiceProvider.GetRequiredService<IService<MessageDto>>();

            var allMessages = await messageService.GetAll();

            var now = DateTime.UtcNow;
            var expiredMessages = allMessages
                .Where(m => m.volunteer_id != null &&
                            m.confirmArrival == false &&
                            m.created_at.AddHours(5) < now)
                .ToList();

            foreach (var msg in expiredMessages)
            {
                msg.volunteer_id = null; // משחרר מתנדב
                await messageService.UpDateItem(msg.message_id, msg);
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken); // מריץ כל 30 דק'
        }
    }
}
