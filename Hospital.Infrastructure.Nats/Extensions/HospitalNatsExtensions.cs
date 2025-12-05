using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Infrastructure.Nats;

public static class HospitalNatsExtensions
{
    public static IServiceCollection AddHospitalNatsConsumer(
        this IServiceCollection services)
    {
        services.AddHostedService<HospitalNatsConsumer>();

        return services;
    }
}
