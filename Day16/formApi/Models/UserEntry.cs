namespace formApi.Models;
public class UserEntry
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<Education> Education { get; set; } = new();
}
