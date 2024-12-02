namespace Rive.Maui;

public class DynamicAsset
{
    public required string Name { get; set; }
    public string? Filename { get; set; }
    public string? FileExtension { get; set; }
    public byte[]? FileBytes { get; set; }
}