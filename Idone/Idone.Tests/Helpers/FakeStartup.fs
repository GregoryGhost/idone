namespace Idone.Tests.Helpers

open Microsoft.Extensions.Configuration
open Idone.Back

type FakeStartup(config: IConfiguration) =
    inherit Startup(config)
    