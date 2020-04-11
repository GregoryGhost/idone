namespace Idone.Tests.Types   

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
        let (^) param value = sprintf "%s=%s" param value
        let ae = if __.AcceptEula then "Y" else "F"
        
        [
            "ACCEPT_EULA" ^ ae
            "SA_PASSWORD" ^ __.SaPswd
            "Database" ^ __.Database 
        ] |> ResizeArray<string>