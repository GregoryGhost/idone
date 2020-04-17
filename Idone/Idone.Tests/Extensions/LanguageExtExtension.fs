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


    /// Построитель для монады Either из LanguageExt
    type EitherBuilder () =
        member __.Bind(x, f) = x >>= f
        member __.ReturnFrom x = x

    /// Вычислительное выражение для монады Either из LanguageExt
    let either = new EitherBuilder()

    let reduceAllRights (list : Either<'l, 'r> seq) : 'r seq =
        list.Rights()
        

