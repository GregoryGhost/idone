namespace Idone.Security.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

    using LanguageExt;

    using LdapForNet;
    using LdapForNet.Native;

    using static LanguageExt.Prelude;

    using Success = Idone.DAL.Dictionaries.Success;

    /// <summary>
    /// Сервис по работе с AD-пользователями.
    /// </summary>
    public class AdService
    {
        /// <summary>
        /// Домен сервиса Active Directory. 
        /// </summary>
        private readonly string _domain;

        private readonly string _domainComponent;

        private readonly string _login;

        private readonly string _pswd;

        private readonly string _uidAdmin;

        /// <summary>
        /// Инициализировать зависимости.
        /// </summary>
        /// <param name="domain">Домен сервиса Active Directory.</param>
        /// <param name="login">Логин в LDAP аля "cn=admin,dc=example,dc=org".</param>
        /// <param name="pswd">Пароль пользователя.</param>
        /// <param name="domainComponent">Компонент Домена.</param>
        public AdService(string domain, string login, string pswd, string adminNickname = "admin",
            string domainComponent = "dc=example,dc=org")
        {
            if (string.IsNullOrEmpty(domain))
                throw new NullReferenceException($"Пустой аргумент {nameof(domain)}");

            if (!Dns.GetHostAddresses(domain).Any())
            {
                var msg = $"Не найден домен сервиса Active Directory для переданного аргумента {nameof(domain)}";
                throw new ArgumentException(msg);
            }

            _domain = domain;
            _login = login;
            _pswd = pswd;
            _uidAdmin = FormatUserUid(adminNickname, domainComponent);
            _domainComponent = domainComponent;
        }

        private static string FormatUserUid(string nickname, string domainCompany)
        {
            return $"cn={nickname},{domainCompany}";
        }

        /// <summary>
        /// Найти пользователей по отображаемому имени.
        /// </summary>
        /// <param name="searchExpression"> Шаблон поиска. </param>
        /// <returns> Возращает монаду с найденными совпадениями пользователей по отображаемому имени. </returns>
        public Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression)
        {
            using (var cn = new LdapConnection())
            {
                cn.Connect(_domain);
                cn.Bind(Native.LdapAuthMechanism.SIMPLE, _login, _pswd);
                var adUsers = cn.Search(
                    _domainComponent,
                    $"(displayName={searchExpression})");

                var foundUsers =
                    adUsers.Select(user => ToAdUser(user, _domainComponent)).
                            Rights(); //TODO: писать в лог Left вариант Either'а через Where

                return Right<Error, IEnumerable<DtoAdUser>>(foundUsers);
            }
        }

        private static Either<DictValueCases, DtoAdUser> ToAdUser(LdapEntry userData,
            string domainCompany)
        {
            var nicknameValue = GetValue(userData.Attributes, "uid");
            var surnameValue = GetValue(userData.Attributes, "sn");
            var givenNameValue = GetValue(userData.Attributes, "givenName");
            var emailValue = GetValue(userData.Attributes, "mail");
            var middleNameValue = GetValue(userData.Attributes, "middleName");

            var adUser = from nickname in nicknameValue
                from surname in surnameValue
                from givenName in givenNameValue
                from email in emailValue
                let uid = FormatUserUid(nickname, domainCompany)
                select new DtoAdUser(uid, surname, givenName, middleNameValue.IfLeft(string.Empty), email);

            return adUser;
        }

        private static Either<DictValueCases, string> GetValue(IReadOnlyDictionary<string, List<string>> attributes, string key)
        {
            var leftCase = lpar<DictValueCase, string, DictValueCases>(DictValueCases.Create, key);
            if (attributes.TryGetValue(key, out var values))
                return values.Any()
                    ? Right(values.First())
                    : Left<DictValueCases, string>(leftCase(DictValueCase.EmptyValue));

            return Left(leftCase(DictValueCase.WrongKey));
        }

        /// <summary>
        /// Создать AD-пользователя.
        /// </summary>
        /// <param name="newUser">Данные нового пользователя.</param>
        /// <returns>Результат операции.</returns>
        public Either<Error, Success> CreateUser(DtoNewAdUser newUser)
        {
            using (var cn = new LdapConnection())
            {
                cn.Connect(_domain);
                cn.Bind(Native.LdapAuthType.Simple, new LdapCredential { UserName = _login, Password = _pswd });
                cn.Add(
                    new LdapEntry
                    {
                        Dn = FormatUserUid(newUser.Nickname, _domainComponent), //TODO: вынести в метод формирования=
                        Attributes = new Dictionary<string, List<string>>
                        {
                            { "sn", new List<string> { newUser.Surname } },
                            { "objectclass", new List<string> { "inetOrgPerson" } },
                            { "givenName", new List<string> { newUser.Name } },
                            { "displayName", new List<string> { $"{newUser.Surname} {newUser.Name} {newUser.Patronomic}" } },
                            { "title", new List<string> { newUser.Patronomic } },
                            { "mail", new List<string> { newUser.Email } },
                            { "uid", new List<string> { newUser.Nickname } },
                            { "userPassword", new List<string> { newUser.Password } }
                        }
                    });
            }

            return Right<Error, Success>(Success.ItsSuccess);
        }
    }
}