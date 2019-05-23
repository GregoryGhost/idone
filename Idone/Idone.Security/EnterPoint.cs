namespace Idone.Security
{
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;

    using LanguageExt;
    using static LanguageExt.Prelude;

    public class EnterPoint : ISecurityModule
    {
        public Either<Error, DtoRegistratedUser> RegistrateUser(DtoRegistrateUser registrateUser)
        {
            var test = new DtoRegistratedUser("kek@mail.ru");
            return Right<Error, DtoRegistratedUser>(test);
        }

        public Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression)
        {
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
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Базовый класс для запроса на формирование таблицы записей.
    /// </summary>
    /// <typeparam name="TFilter"> Тип фильтра записей. </typeparam>
    public abstract class AbstractGridQuery<TFilter> : Record<AbstractGridQuery<TFilter>>
    {
        public AbstractGridQuery(TFilter filter, Pagination pagination)
        {
            Filter = filter;
            Pagination = pagination;
        }

        /// <summary>
        /// Фильтр.
        /// </summary>
        public virtual TFilter Filter { get; private set; }

        /// <summary>
        /// Пагинация.
        /// </summary>
        public virtual Pagination Pagination { get; private set; }
    }

    /// <summary>
    /// DTO, описывающее пагинацию таблицы.
    /// </summary>
    public class Pagination : Record<Pagination>
    {
        /// <summary>
        /// Количество элементов на странице.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Номер страницы.
        /// </summary>
        public int Page { get; private set; }

        public Pagination(int count, int page)
        {
            Count = count;
            Page = page;
        }
    }

    public class DtoUserFilter : Record<DtoUserFilter>
    {
        public DtoUserFilter(string email)
        {
            Email = email;
        }

        public string Email { get; private set; }
    }

    public class DtoGridQueryUser : AbstractGridQuery<DtoUserFilter>
    {
        public DtoGridQueryUser(DtoUserFilter filter, Pagination pagination) 
            : base(filter, pagination)
        {
        }
    }

    public class DtoGridUser : DtoGrid<DtoRowUser>
    {
        public DtoGridUser(IEnumerable<DtoRowUser> rows, int total)
            : base(rows, total)
        {
        }
    }

    public class DtoRowUser
    {
        public string Email { get; private set; }
        public string DisplayName { get; private set; }

        public DtoRowUser(string email, string displayName)
        {
            Email = email;
            DisplayName = displayName;
        }
    }

    /// <summary>
    /// DTO для формирования таблицы записей.
    /// </summary>
    /// <typeparam name="T"> Тип записей таблицы. </typeparam>
    public class DtoGrid<T> : Record<DtoGrid<T>>
    {
        /// <summary>
        /// Строки.
        /// </summary>
        public IEnumerable<T> Rows { get; private set; }

        /// <summary>
        /// Общее количество строк.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="rows"> Строки. </param>
        /// <param name="total"> Общее количество строк. </param>
        public DtoGrid(IEnumerable<T> rows, int total)
        {
            Rows = rows;
            Total = total;
        }
    }

    public class DtoAdUser : Record<DtoAdUser>
    {
        public DtoAdUser(
            string sid,
            string surname,
            string name,
            string patronomic,
            string email)
        {
            Sid = sid;
            Surname = surname;
            Name = name;
            Patronomic = patronomic;
            Email = email;
        }

        public string Surname { get; private set; }
        public string Name { get; private set; }
        public string Patronomic { get; private set; }
        public string Email { get; private set; }
        public string Sid { get; private set; }
    }

    public class DtoRegistratedUser : Record<DtoRegistrateUser>
    {
        public string Email { get; private set; }

        public DtoRegistratedUser(string email)
        {
            Email = email;
        }
    }

    public class DtoRegistrateUser : Record<DtoRegistrateUser>
    {
        public DtoRegistrateUser(
            string sid, 
            string surname,
            string name,
            string patronomic,
            string email)
        {
            Sid = sid;
            Surname = surname;
            Name = name;
            Patronomic = patronomic;
            Email = email;
        }

        public string Surname { get; private set; }
        public string Name { get; private set; }
        public string Patronomic { get; private set; }
        public string Email { get; private set; }
        public string Sid { get; private set; }
        
    }

    public interface ISecurityModule
    {
        Either<Error, DtoRegistratedUser> RegistrateUser(DtoRegistrateUser registrateUser);

        Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression);

        Either<Error, DtoGridUser> GetGridUser(DtoGridQueryUser gridQueryUser);

        //TODO:дополнить остальными методами для Ролей, Прав.
    }

    public enum Error
    {
        Exception = 0,
    }
}