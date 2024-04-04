using Mona.Utilities;

namespace Mona.Service.Interface;

public interface IFileService
{
    Task<FileUploadSummary> UploadFileAsync(Stream fileStream, string contentType);
}