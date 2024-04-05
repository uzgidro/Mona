using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Mona.Service.Interface;
using Mona.Utilities;

namespace Mona.Service;

public class FileService : IFileService
{
    private const string UploadsSubDirectory = "FilesUploaded";

    public async Task<FileUploadSummary> UploadFileAsync(Stream fileStream, string contentType)
    {
        var fileCount = 0;
        long totalSizeInBytes = 0;
        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var multipartReader = new MultipartReader(boundary, fileStream);
        var section = await multipartReader.ReadNextSectionAsync();
        var filePaths = new List<string>();
        var notUploadedFiles = new List<string>();

        while (section != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                totalSizeInBytes += await SaveFileAsync(fileSection, filePaths, notUploadedFiles);
                fileCount++;
            }
            else if (section.Headers.ContainsValue("form-data; name=\"messageId\""))
            {
                using var reader = new StreamReader(section.Body, Encoding.UTF8);
                Console.WriteLine(await reader.ReadToEndAsync());
            }

            section = await multipartReader.ReadNextSectionAsync();
        }

        return new FileUploadSummary
        {
            TotalFilesUploaded = fileCount,
            TotalSizeUploaded = ConvertSizeToString(totalSizeInBytes),
            FilePaths = filePaths,
            NotUploadedFiles = notUploadedFiles
        };
    }

    private static async Task<long> SaveFileAsync(FileMultipartSection fileSection, List<string> filePaths,
        List<string> notUploadedFiles)
    {
        var extension = Path.GetExtension(fileSection.FileName);

        Directory.CreateDirectory(UploadsSubDirectory);
        var fileName = fileSection.FileName;
        var uniqueFileName = fileName;
        var index = 0;

        while (File.Exists(Path.Combine(UploadsSubDirectory, uniqueFileName)))
        {
            index++;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var indexedFileName = $"{fileNameWithoutExtension} ({index}){extension}";
            uniqueFileName = indexedFileName;
        }

        var filePath = Path.Combine(UploadsSubDirectory, uniqueFileName);

        await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
        await fileSection.FileStream?.CopyToAsync(stream);

        filePaths.Add(GetFullFilePath(fileSection));

        return fileSection.FileStream.Length;
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

    private static string EncodeFileName(string fileName)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(fileName);
        return Convert.ToBase64String(plainTextBytes);
    }

    private static string DecodeFileName(string encodedFileName)
    {
        var base64EncodedBytes = Convert.FromBase64String(encodedFileName);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
}