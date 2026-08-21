namespace formApi.Models;

public class Education
{
    public int Id { get; set; }
    public string Degree { get; set; } = string.Empty;

    public string InstitutionName { get; set; } = string.Empty;
    public int Year { get; set; }

    // Foreign key — links this education row back to its parent entry
    public int UserEntryId { get; set; }
    public UserEntry UserEntry { get; set; } = null!;

}


