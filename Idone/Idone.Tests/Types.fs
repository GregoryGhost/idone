namespace Idone.Tests.Types

type Role =
    {
        Name : string
    }

type Perm =
    {
        Name : string
    }

type PermRoleLink =
    {
        Role : Role
        Perm : Perm
    }
