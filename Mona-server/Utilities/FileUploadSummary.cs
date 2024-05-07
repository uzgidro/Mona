using Mona.Model;

namespace Mona.Utilities;

public class FileUploadSummary
{
    public MessageModel Message { get; set; }
    public int TotalFilesUploaded { get; set; }
    public string TotalSizeUploaded { get; set; }
    public IList<string> FilePaths { get; set; } = new List<string>();
    public IList<string> NotUploadedFiles { get; set; } = new List<string>();
}