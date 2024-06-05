namespace Mona.Model.Dto;

public struct FileDto
{
    public string? Id { get; init; }
    public string Name { get; init; }
    public long Size { get; init; }
    public string Path { get; init; }
}