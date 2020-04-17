namespace Idone.DAL.Entities
{
    /// <summary>
    /// Полное имя.
    /// </summary>
    public class FullName
    {
        /// <summary>
        /// Конструктор для EF Core.
        /// </summary>
        private FullName()
        {
        }

        /// <summary>
        /// Конструктор объекта-значения.
        /// </summary>
        /// <param name="surname"> Фамилия. </param>
        /// <param name="name"> Имя. </param>
        /// <param name="patronymic"> Отчество. </param>
        public FullName(string surname, string name, string patronymic)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronymic { get; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Surname { get; }
    }
}