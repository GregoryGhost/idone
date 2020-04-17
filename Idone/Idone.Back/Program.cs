namespace Idone.Back
{
    using Idone.DAL.Base;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }

        public static void Main(string[] args)
        {
            //TODO: нужна единая точка входа по инициализации настроек модулей.
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<AppContext>().InitTest();
            }

            host.Run();
        }
    }
}