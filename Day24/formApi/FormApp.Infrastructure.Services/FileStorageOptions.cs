namespace formApi.FormApp.Infrastructure.Services
{
    public class FileStorageOptions
    {
        public string UploadRootPath { get; set; } = string.Empty; // absolute physical path, e.g. wwwroot/uploads
        public string RequestPathPrefix { get; set; } = "/uploads"; // public URL prefix returned to clients
    }
}
