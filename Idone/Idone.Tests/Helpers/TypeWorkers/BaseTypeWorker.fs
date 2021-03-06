namespace Idone.Tests.Helpers.TypeWorkers

open Idone.DAL.Base
open Idone.DAL.DTO
open Idone.DAL.Dictionaries

open Idone.Tests.Extensions
open Idone.Tests.Helpers.IdoneApiHelper

open Idone.Tests.Constants
open LanguageExt

[<AbstractClass>]            
type BaseTypeWorker<
        'TType,
        'TRowType,
        'TRowDepType,
        'TTypeGrid,
        'TTypeDepGrid,
        'TTypeFilter,
        'TTypeGridQuery
            when 'TRowType       :> IIdentity
            and  'TTypeDepGrid   :> DtoGrid<'TRowDepType>
            and  'TTypeGrid      :> DtoGrid<'TRowType>
            and  'TTypeGridQuery :> AbstractGridQuery<'TTypeFilter>>() =
    
                
    abstract member GetGridDepType  : #IIdentity       -> Either<Error, 'TTypeDepGrid>
    abstract member GetGridEntities : 'TTypeGridQuery  -> Either<Error, 'TTypeGrid>
    abstract member ToGridQueryType : 'TType           -> Pagination -> 'TTypeGridQuery
    
    
    member __.ToDefaultGridQueryType (entity: 'TType): 'TTypeGridQuery =
        __.ToGridQueryType entity Constants.FIRST_PAGE
        
    member __.FindEntity (entity: 'TType): Either<Error, 'TRowType>   =
        entity |> __.ToDefaultGridQueryType |> __.GetGridEntities >>= takeFirstRow 
    
    member __.GetEntityIds (entities: 'TType seq): #IIdentity seq =
        entities
        |> Seq.map (fun entity -> either {
                return! __.FindEntity entity })
        |> reduceAllRights 
        |> Seq.map (fun entity -> new DtoFilterById(entity.Id) :> IIdentity)
        
    member __.GetDepTypes (entities: 'TType list): 'TRowDepType list =
        entities
        |> __.GetEntityIds
        |> Seq.map (fun entityId -> either {
                let grid = __.GetGridDepType entityId
                return! grid})
        |> reduceAllRights
        |> Seq.map (fun x -> takeFirstRow x)
        |> reduceAllRights
        |> Seq.toList