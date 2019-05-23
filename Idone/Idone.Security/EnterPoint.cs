namespace Idone.Security
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;

    using Idone.Core;
    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;
    using Idone.Security.Services;

    using LanguageExt;

    using Microsoft.Extensions.DependencyInjection;

    using static LanguageExt.Prelude;

    public class EnterPoint : ISecurityModule
    {
        private readonly UserService _userService;
        public EnterPoint(IServiceProvider serviceProvider)
        {
            _userService = serviceProvider.GetService<UserService>();
        }

        public Either<Error, DtoRegistratedUser> RegistrateUser(DtoRegistrateUser registrateUser)
        {
            var result = _userService.RegistrateUser(registrateUser);

            return result;
        }

        public Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression)
        {
            //TODO: выделить в метод сервиса AdService
            var ctx = new PrincipalContext(ContextType.Domain, "tomskasu");
            var query = new UserPrincipal(ctx)
            {
                DisplayName = searchExpression
            };
            var foundUsers = new PrincipalSearcher(query).FindAll()
                .Where(user => user.DisplayName != null)
                .Select(user => new DtoAdUser(user.Sid.Value, "", user.Name, "", "tipo@mail.ru"));

            return Right<Error, IEnumerable<DtoAdUser>>(foundUsers);
        }

        public Either<Error, DtoGridUser> GetGridUser(DtoGridQueryUser gridQueryUser)
        {
            var result = _userService.GetGridUser(gridQueryUser);

            return result;
        }
    }
}