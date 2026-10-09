namespace MediCare.Domain.Users
{
    public enum PermissionEffect
    {
        /// <summary>Theo role: không có dòng nào trong User_Permission.</summary>
        Inherit = 0,

        /// <summary>User có quyền này, bất kể role (IsGranted = true).</summary>
        Allow = 1,

        /// <summary>User không có quyền này, bất kể role (IsGranted = false).</summary>
        Deny = 2
    }
}
