namespace Idone.DAL.DTO
{
    using System.Collections.Generic;

    using LanguageExt;

    /// <summary>
    /// DTO права доступа для роли.
    /// </summary>
    public class DtoLinkRolePermissions : Record<DtoLinkRolePermissions>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="roleId"> Идентификатор роли. </param>
        /// <param name="permissionIds"> Идентификаторы прав.</param>
        public DtoLinkRolePermissions(int roleId, IEnumerable<int> permissionIds)
        {
            RoleId = roleId;
            PermissionIds = permissionIds;
        }

        /// <summary>
        /// Идентификаторы прав.
        /// </summary>
        public IEnumerable<int> PermissionIds { get; }

        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        public int RoleId { get; }
    }
}