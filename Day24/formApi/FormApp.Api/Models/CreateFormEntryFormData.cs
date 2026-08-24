namespace formApi.FormApp.API.Models;

// [FromForm] binding target. This is the ONE place IFormFile is allowed to
// exist — the controller maps it into Application's framework-agnostic
// FileUploadDto before calling into the service layer.
public class CreateFormEntryFormData
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? EducationJson { get; set; } // raw JSON string, parsed by the Application service
    public IFormFile? CvFile { get; set; }
}
