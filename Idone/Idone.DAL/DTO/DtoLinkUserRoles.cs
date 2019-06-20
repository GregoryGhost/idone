namespace Idone.DAL.DTO
{
    using System.Collections.Generic;

    using LanguageExt;

    /// <summary>
    /// DTO роли пользователя.
    /// </summary>
    public class DtoLinkUserRoles : Record<DtoLinkUserRoles>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="userId"> Идентификатор пользователя. </param>
        /// <param name="roleIds"> Идентификаторы ролей пользователя. </param>
        public DtoLinkUserRoles(int userId, IEnumerable<int> roleIds)
        {
            UserId = userId;
            RoleIds = roleIds;
        }

        /// <summary>
        /// Идентификаторы ролей пользователя.
        /// </summary>
        public IEnumerable<int> RoleIds { get; private set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; private set; }
    }
}