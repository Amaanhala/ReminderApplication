using ReminderApplication.ViewModel;

namespace ReminderApplication.Extensions
{
    public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegisterViewModel>();

        return services;
    }
}
}