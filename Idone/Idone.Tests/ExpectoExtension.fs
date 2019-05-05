namespace Idone.Tests.Extensions

module Expect =
    open LanguageExt
    open Expecto

    let isRight (actual: Either<_, _>) (message: string) =
        match actual.State with
        | EitherStatus.IsRight -> ()
        | _ -> failtestf "%s. Expected Either.Right _, was Either.Left." message

    let isLeft (actual: Either<_, _>) (message: string) =
        match actual.State with
        | EitherStatus.IsLeft -> ()
        | _ -> failtestf "%s. Expected Either.Left _, was Either.Right." message