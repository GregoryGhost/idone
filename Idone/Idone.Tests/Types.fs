namespace Idone.Tests.Types   

open Docker.DotNet
open Docker.DotNet.Models
open Microsoft.Extensions.DependencyInjection


module Helpers =
    let (^) param value = sprintf "%s=%s" param value
    
open Helpers

type Role =
    {
        Name : string
    }

type Perm =
    {
        Name : string
        Description : string
    }

type PermRoleLink =
    {
        Role : Role
        Perm : Perm
    }
    
type Address =
    {
        IpAddress : string
        Port : string
    }
    override __.ToString() =
        sprintf "%s,%s" __.IpAddress __.Port 
    
type DatabaseSettings =
    {
        Server : Address
        Database : string
        UserId : string
        Pswd: string
        TrustedConnection : bool
    }
    member __.toConnectionString : string =
        let (^) paramName value = sprintf "%s=%s;" paramName value
        [|
            "Server" ^ __.Server.ToString()
            "Database" ^ __.Database
            "User ID" ^ __.UserId
            "Password" ^ __.Pswd
            "Trusted_Connection" ^ __.TrustedConnection.ToString()
        |] |> System.String.Concat
       
type DockerDatabaseEnv =
    {
        AcceptEula : bool
        SaPswd : string
        Database : string
    }
    member __.toEnv : ResizeArray<string> =
        let ae = if __.AcceptEula then "Y" else "F"
        
        [
            "ACCEPT_EULA" ^ ae
            "SA_PASSWORD" ^ __.SaPswd
            "Database" ^ __.Database 
        ] |> ResizeArray<string>
        
type DockerActiveDirectoryEnv =
    {
        Organisation : string
        Domain : string
        AdminPassword : string
    }
    member __.toEnv : ResizeArray<string> =
        [
           "LDAP_ORGANISATION" ^ __.Organisation
           "LDAP_DOMAIN" ^ __.Domain
           "LDAP_ADMIN_PASSWORD" ^ __.AdminPassword
        ] |> ResizeArray<string>
        
type Docker =
    {
        Client : DockerClient
        Containers: CreateContainerResponse list
    }
    
type TestEnvironment =
    {
        ServiceProvider : ServiceProvider
        Docker : Docker
    }
    static member create (provider : ServiceProvider)
                  (dockerClient : DockerClient)
                  (containers : CreateContainerResponse list) : TestEnvironment =
        {
            ServiceProvider = provider
            Docker =
                {
                    Client = dockerClient
                    Containers = containers
                }
        }
        
type ContainerResponse =
    {
        Response : CreateContainerResponse
        HostPort : string
        Ip: string
    }
    
type ContainerParams =
    {
        ContainerParameters : CreateContainerParameters
        LocalIp : string
    }
    
type ContainerSettings =
    {
        Client : DockerClient
        Image : string
        Tag : string
        Port : string
        Env : ResizeArray<string>
    }