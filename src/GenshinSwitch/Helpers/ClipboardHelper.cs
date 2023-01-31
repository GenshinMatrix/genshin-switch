using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace GenshinSwitch.Helpers;

/// <summary>
/// https://www.nuget.org/packages/Scighost.WinUILib
/// https://github.com/Scighost/WinUILib/blob/main/WinUILib/Helpers/ClipboardHelper.cs
/// </summary>
public static class ClipboardHelper
{
    public static void SetText(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            DataPackage dataPackage = new()
            {
                RequestedOperation = DataPackageOperation.Copy,
            };
            dataPackage.SetText(value);
            Clipboard.SetContent(dataPackage);
            Clipboard.Flush();
        }
    }

    public static void SetBitmap(IRandomAccessStream stream)
    {
        RandomAccessStreamReference bitmap = RandomAccessStreamReference.CreateFromStream(stream);
        DataPackage dataPackage = new()
        {
            RequestedOperation = DataPackageOperation.Copy,
        };
        dataPackage.SetBitmap(bitmap);
        Clipboard.SetContent(dataPackage);
        Clipboard.Flush();
    }

    public static void SetBitmap(IStorageFile file)
    {
        RandomAccessStreamReference bitmap = RandomAccessStreamReference.CreateFromFile(file);
        DataPackage dataPackage = new()
        {
            RequestedOperation = DataPackageOperation.Copy,
        };
        dataPackage.SetBitmap(bitmap);
        Clipboard.SetContent(dataPackage);
        Clipboard.Flush();
    }

    public static void SetBitmap(Uri uri)
    {
        RandomAccessStreamReference bitmap = RandomAccessStreamReference.CreateFromUri(uri);
        DataPackage dataPackage = new()
        {
            RequestedOperation = DataPackageOperation.Copy,
        };
        dataPackage.SetBitmap(bitmap);
        Clipboard.SetContent(dataPackage);
        Clipboard.Flush();
    }

    public static void SetStorageItems(DataPackageOperation operation, params IStorageItem[] items)
    {
        DataPackage dataPackage = new()
        {
            RequestedOperation = operation,
        };
        dataPackage.SetStorageItems(items);
        Clipboard.SetContent(dataPackage);
        Clipboard.Flush();
    }

    public static async Task<string?> GetTextAsync()
    {
        DataPackageView content = Clipboard.GetContent();
        if (content.Contains(StandardDataFormats.Text))
        {
            return await content.GetTextAsync();
        }
        return null;
    }
}
