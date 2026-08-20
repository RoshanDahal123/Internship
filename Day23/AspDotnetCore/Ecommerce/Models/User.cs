namespace Ecommerce.Models;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public ICollection<UserRole>UserRoles { get; set; } = new List<UserRole>();
}
    
