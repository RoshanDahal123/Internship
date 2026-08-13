namespace formApi.DTOs
{// Mirrors the frontend's Education interface exactly — no "Id" or
 // "UserEntryId" here, since the frontend never needs to know about those
    public class EducationDto
    {
        public string Degree { get; set; } = string.Empty;
        public string InstitutionName { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
