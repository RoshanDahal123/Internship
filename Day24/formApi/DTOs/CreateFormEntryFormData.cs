namespace formApi.DTOs
{
    public class CreateFormEntryFormData
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? EducationJson { get; set; }  // raw JSON string, parsed manually in the controller
        public IFormFile? CvFile { get; set; }        // the actual uploaded file
    }
}
