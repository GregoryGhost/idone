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
