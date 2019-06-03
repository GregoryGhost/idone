namespace Idone.Tests.Extensions

[<AutoOpen>]
module LanguageExtExtension = 
    open LanguageExt

    /// Преобразовать Option в Either
    let toEither (x : 'R option) (error : 'L) : Either<'L, 'R> =
        match x with
        | Some y -> LanguageExt.Prelude.Right<'L, 'R>(y)
        | None -> LanguageExt.Prelude.Left<'L, 'R>(error)

    /// Монадический Bind для Either из LanguageExt
    let (>>=) (x : Either<'L,'R>) (func: ('R -> Either<'L, 'B>)) =
        x.Bind(func)

