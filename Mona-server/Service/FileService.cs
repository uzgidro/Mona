using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Mona.Context;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;
using Mona.Utilities;

namespace Mona.Service;

public class FileService(ApplicationContext context) : IFileService
{
    private const string UploadsSubDirectory = "FilesUploaded";

    public async Task<FileUploadSummary> UploadFileAsync(MultipartReader multipartReader, MessageModel message)
    {
        var fileCount = 0;
        long totalSizeInBytes = 0;
        var section = await multipartReader.ReadNextSectionAsync();
        var filePaths = new List<string>();
        var notUploadedFiles = new List<string>();

        while (section != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                var file = await SaveFileAsync(fileSection, filePaths, notUploadedFiles);
                await context.Files.AddAsync(new FileModel
                {
                    Name = file.Name, Path = file.Path, Size = file.Size, MessageId = message.Id,
                    CreatedAt = DateTime.Now
                });
                await context.SaveChangesAsync();
                fileCount++;
                totalSizeInBytes += file.Size;
            }

            section = await multipartReader.ReadNextSectionAsync();
        }

        // Missing files and empty text are not permitted.
        if (fileCount == 0 && string.IsNullOrEmpty(message.Text))
        {
            context.Messages.Remove(message);
        }

        return new FileUploadSummary
        {
            Message = message,
            TotalFilesUploaded = fileCount,
            TotalSizeUploaded = ConvertSizeToString(totalSizeInBytes),
            FilePaths = filePaths,
            NotUploadedFiles = notUploadedFiles
        };
    }

    private static async Task<FileDto> SaveFileAsync(FileMultipartSection fileSection, List<string> filePaths,
        List<string> notUploadedFiles)
    {
        var extension = Path.GetExtension(fileSection.FileName);

        var root = Path.Combine(UploadsSubDirectory, DateTime.Now.ToString("dd.MM.yyyy"));
        Directory.CreateDirectory(root);
        var fileName = fileSection.FileName;
        var uniqueFileName = fileName;
        var index = 0;

        while (File.Exists(Path.Combine(root, uniqueFileName)))
        {
            index++;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var indexedFileName = $"{fileNameWithoutExtension} ({index}){extension}";
            uniqueFileName = indexedFileName;
        }

        var filePath = Path.Combine(root, uniqueFileName);

        await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
        await fileSection.FileStream?.CopyToAsync(stream);

        filePaths.Add(GetFullFilePath(fileSection));

        return new FileDto { Name = fileName, Path = filePath, Size = fileSection.FileStream.Length };
    }

    private static string GetFullFilePath(FileMultipartSection fileSection)
    {
        return !string.IsNullOrEmpty(fileSection.FileName)
            ? Path.Combine(Directory.GetCurrentDirectory(), UploadsSubDirectory, fileSection.FileName)
            : string.Empty;
    }

    private static string ConvertSizeToString(long bytes)
    {
        var fileSize = new decimal(bytes);
        var kilobyte = new decimal(1024);
        var megabyte = new decimal(1024 * 1024);
        var gigabyte = new decimal(1024 * 1024 * 1024);

        return fileSize switch
        {
            _ when fileSize < kilobyte => "Less then 1KB",
            _ when fileSize < megabyte =>
                $"{Math.Round(fileSize / kilobyte, fileSize < 10 * kilobyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}KB",
            _ when fileSize < gigabyte =>
                $"{Math.Round(fileSize / megabyte, fileSize < 10 * megabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}MB",
            _ when fileSize >= gigabyte =>
                $"{Math.Round(fileSize / gigabyte, fileSize < 10 * gigabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}GB",
            _ => "n/a"
        };
    }

    private static string GetBoundary(MediaTypeHeaderValue contentType)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        if (string.IsNullOrWhiteSpace(boundary))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        return boundary;
    }
}