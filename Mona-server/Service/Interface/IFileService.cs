using Microsoft.AspNetCore.WebUtilities;
using Mona.Utilities;

namespace Mona.Service.Interface;

public interface IFileService
{
    Task<FileUploadSummary> UploadFileAsync(MultipartReader multipartReader, string messageId);
}