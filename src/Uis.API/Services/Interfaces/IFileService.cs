using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Services.Interfaces;
public interface IFileService
{
    Task<ResultService<string>> UploadFileAsync(IFormFile file, string folder);
    ResultService DeleteFileAsync(string fileUrl);
    ResultService ValidateFile(IFormFile file);
}
