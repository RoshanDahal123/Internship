namespace formApi.FormApp.Application.DTOs
{
    public class FileUploadDto
    {
        public string FileName { get; set; } = string.Empty;
        public long Length { get; set; }
        public required Stream OpenStream { get; init; } // caller passes an already-open readable stream
    }

}
