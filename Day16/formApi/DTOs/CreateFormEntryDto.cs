namespace formApi.DTOs;

    // What the client sends on POST — matches your FormData interface (no id yet)
public class CreateFormEntryDto
{
public string Name { get; set; } = string.Empty;
public string Email { get; set; } = string.Empty;
public int Age { get; set; }
public string Address { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public List<EducationDto> Education { get; set; } = new();
}

