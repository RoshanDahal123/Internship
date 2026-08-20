namespace Ecommerce.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = Roles.User.ToString();

       public ICollection<UserRole>? UserRoles { get; set; }
    }

    public enum Roles
    {
        Admin,
        User
    }
}
