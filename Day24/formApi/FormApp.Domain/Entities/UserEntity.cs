

namespace formApi.FormApp.Domain.Entities
{
    public class UserEntry
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? CvFilePath { get; set; } // relative path, e.g. "/uploads/xxx.pdf"

        public List<Education> Education { get; set; } = new();
    }

}
