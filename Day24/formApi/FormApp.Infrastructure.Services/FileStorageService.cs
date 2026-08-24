using formApi.FormApp.Application.Interfaces;
using Microsoft.Extensions.Options;
using formApi.FormApp.Application.DTOs;

namespace formApi.FormApp.Infrastructure.Services;
public class FileStorageService : IFileStorageService
{
    private readonly FileStorageOptions _options;

    public FileStorageService(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

    public async Task<string> SaveAsync(FileUploadDto file)
    {
        if (!Directory.Exists(_options.UploadRootPath))
            Directory.CreateDirectory(_options.UploadRootPath);

        // Generate a unique filename — never trust/reuse the original filename directly,
        // two people uploading "resume.pdf" would otherwise overwrite each other's file.
        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_options.UploadRootPath, uniqueFileName);

        await using var destination = new FileStream(filePath, FileMode.Create);
        await file.OpenStream.CopyToAsync(destination);

        return $"{_options.RequestPathPrefix.TrimEnd('/')}/{uniqueFileName}";
    }

    public Task DeleteAsync(string relativePath)
    {
        var filePath = Path.Combine(
            _options.UploadRootPath,
            Path.GetFileName(relativePath)
        );

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}
