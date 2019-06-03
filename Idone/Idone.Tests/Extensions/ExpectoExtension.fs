namespace Idone.Tests.Extensions

[<AutoOpen>]
module Expecto =
    open LanguageExt
    open Expecto

    /// Ожидает, что значение является Either.Right _
    let isRight (actual: Either<_, _>) (message: string) =
        match actual.State with
        | EitherStatus.IsRight -> ()
        | _ -> failtestf "%s. Expected Either.Right _, was Either.Left." message

    /// Ожидает, что значение является Either.Left _
    let isLeft (actual: Either<_, _>) (message: string) =
        match actual.State with
        | EitherStatus.IsLeft -> ()
        | _ -> failtestf "%s. Expected Either.Left _, was Either.Right." message

    /// Ожидает, что два значения будут эквивалентны друг другу
    let areEqual (actual: Record<'a>) (expected: Record<'a>) (message: string) =
        if actual <> expected then
            failtestf "%s. Actual value was %A but had expected it to be %A." 
                <||| (message, actual, expected)

    /// Ожидает, что два значения будут не эквивалентны друг другу
    let areNotEqual (actual: Record<'a>) (expected: Record<'a>) (message: string) =
        if actual = expected then
            failtestf "%s. Actual value was equal to %A but had expected them to be non-equal."
                <|| (message, actual)