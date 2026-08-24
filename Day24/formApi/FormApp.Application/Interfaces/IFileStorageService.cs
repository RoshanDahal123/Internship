
using formApi.FormApp.Application.DTOs;

namespace formApi.FormApp.Application.Interfaces;

// Implemented by Infrastructure.Services (physical disk today, could be
// swapped for S3/Azure Blob later without touching Application or API).
public interface IFileStorageService
{
    /// <summary>Saves the file and returns a relative path, e.g. "/uploads/xxx.pdf".</summary>
    Task<string> SaveAsync(FileUploadDto file);
    Task DeleteAsync(string relativePath);
}
