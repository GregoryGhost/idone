namespace Idone.Tests

open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc.Testing
open Idone.DAL.Base
open Microsoft.AspNetCore


type TestApplicationFactory<'TStartup when 'TStartup : not struct>() =
    inherit WebApplicationFactory<'TStartup>()

    override __.ConfigureWebHost(builder : IWebHostBuilder) =
        let configurate (services : IServiceCollection) =
            let host = WebHost.CreateDefaultBuilder().UseStartup<'TStartup>().Build()
            
            use scope = host.Services.CreateScope()
            scope.ServiceProvider.GetRequiredService<AppContext>().InitTest()

        builder.ConfigureServices(configurate) |> ignore