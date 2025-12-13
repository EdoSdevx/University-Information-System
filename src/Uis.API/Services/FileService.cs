using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;
public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly long _maxFileSize = 50 * 1024 * 1024;
    private readonly string[] _allowedExtensions = new[]
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
        ".txt", ".zip", ".rar", ".7z", ".tar", ".gz",
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg"
    };


    public FileService(ILogger<FileService> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public ResultService ValidateFile(IFormFile file)
    {

        if (file == null || file.Length == 0)
        {
            return ResultService.Fail("File is required");
        }

        if (file.Length > _maxFileSize)
        {
            return ResultService.Fail("File size cannot exceed 50MB");
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(fileExtension))
        {
            return ResultService.Fail($"File type {fileExtension} is not supported");
        }

        return ResultService.Ok("File is valid");
    }
    public async Task<ResultService<string>> UploadFileAsync(IFormFile file, string folder)
    {
        try
        {   
            if (ValidateFile(file) is { Success: false } validation)
            {
                _logger.LogWarning("File validation failed: {ErrorMessage}", validation.Message);
                return ResultService<string>.Fail(validation.Message);
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads", folder);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
                _logger.LogInformation("Created upload folder: {FolderPath}", uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileUrl = $"/uploads/{folder}/{uniqueFileName}";

            _logger.LogInformation("File uploaded successfully: {FileName} to {FileUrl}",
                file.FileName, fileUrl);

            return ResultService<string>.Ok(fileUrl, "File uploaded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file: {FileName}", file?.FileName);
            return ResultService<string>.Fail("Failed to upload file. Please try again.");
        }
    }
    public ResultService DeleteFileAsync(string fileUrl)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                return ResultService.Fail("File URL is required");
            }

            var relativePath = fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                return ResultService.Ok("File deleted successfully");
            }

            _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
            return ResultService.NotFound("File not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FileUrl}", fileUrl);
            return ResultService.Fail("Failed to delete file");
        }
    }

}
