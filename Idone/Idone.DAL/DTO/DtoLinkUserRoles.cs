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
        public DtoLinkUserRoles(IIdentity userId, IEnumerable<IIdentity> roleIds)
        {
            UserId = userId;
            RoleIds = roleIds;
        }

        /// <summary>
        /// Идентификаторы ролей пользователя.
        /// </summary>
        public IEnumerable<IIdentity> RoleIds { get; private set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public IIdentity UserId { get; private set; }
    }
}