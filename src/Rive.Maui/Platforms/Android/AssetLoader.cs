using Android.Content;
using Rive.Android.Core;

namespace Rive.Maui;

public class AssetLoader : FileAssetLoader
{
    private readonly Context _context;
    private readonly List<DynamicAsset> _dynamicAssets;

    public AssetLoader(Context context, List<DynamicAsset> dynamicAssets)
    {
        _context = context;
        _dynamicAssets = dynamicAssets;
    }

    public override bool LoadContents(FileAsset asset, byte[] inBandBytes)
    {
        var dynamicAsset = _dynamicAssets.FirstOrDefault(x => string.Equals(x.Name, asset.Name, StringComparison.OrdinalIgnoreCase));
        if (dynamicAsset == null)
            return false;

        var resourceIdentifier = _context.Resources?.GetIdentifier(dynamicAsset.Filename, "drawable", _context.PackageName) ?? 0;
        if (resourceIdentifier == 0)
            return false;

        using var stream = _context.Resources?.OpenRawResource(resourceIdentifier);
        if (stream == null)
            return false;

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        switch (asset)
        {
            case ImageAsset imageAsset:
                imageAsset.Decode(memoryStream.ToArray());
                return true;
            case FontAsset fontAsset:
                fontAsset.Decode(memoryStream.ToArray());
                return true;
            case AudioAsset audioAsset:
                audioAsset.Decode(memoryStream.ToArray());
                return true;
            default:
                return false;
        }
    }
}