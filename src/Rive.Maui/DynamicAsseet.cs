namespace Rive.Maui;

public class DynamicAsset
{
    public DynamicAsset()
    {
    }

    public DynamicAsset(string name, string filename, string extension)
    {
        Name = name;
        Filename = filename;
        Extension = extension;
    }

    public required string Name { get; set; }
    public required string Filename { get; set; }
    public required string Extension { get; set; }
}