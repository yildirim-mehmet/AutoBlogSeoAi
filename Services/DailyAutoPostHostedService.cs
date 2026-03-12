//
// bu öenerilmez admin/automation çalıştır önerilir
//

//namespace BlogSeoAi.Services
//{
//    public class DailyAutoPostHostedService
//    {
//    }
//}

using Microsoft.Extensions.DependencyInjection;

namespace BlogSeoAi.Services;

public class DailyAutoPostHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DailyAutoPostHostedService(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Basit yaklaşım: her 1 saatte bir kontrol et, o gün üretildiyse pas geç
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // İstersen burada saat kontrolü yap: örn 03:00 civarı çalışsın
                var nowLocal = DateTime.Now;
                if (nowLocal.Hour == 3) // 03:00
                {
                    using var scope = _scopeFactory.CreateScope();
                    var gen = scope.ServiceProvider.GetRequiredService<IDailyPostGenerator>();
                    await gen.GenerateAndPublishForTodayAsync(stoppingToken);
                }
            }
            catch
            {
                // logla (ILogger ekleyebilirsin)
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}