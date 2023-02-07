using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenshinSwitch.Controls.Notice;
using GenshinSwitch.Fetch.Lazy;
using GenshinSwitch.Helpers;
using Microsoft.VisualStudio.Threading;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace GenshinSwitch.ViewModels;

public partial class SetLazyTokenViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string tokenInput = string.Empty;
    partial void OnTokenInputChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(TokenInput))
        {
            TokenOutput = string.Empty;
        }
        else
        {
            TokenOutput = LazyCrypto.Encrypt(TokenInput);
        }
    }

    [ObservableProperty]
    private string tokenOutput = string.Empty;

    public SetLazyTokenViewModel()
    {
        ReloadAsync().Forget();
    }

    private async Task ReloadAsync()
    {
        if (await LazyRepository.SetupToken())
        {
            string token = await LazyRepository.GetToken();

            if (RuntimeHelper.IsDebuggerAttached)
            {
                TokenInput = token;
            }
            TokenOutput = LazyCrypto.Encrypt(token);
        }
    }

    [RelayCommand]
    private async Task OpenTokenAsync()
    {
        FileOpenPicker dialog = new()
        {
            ViewMode = PickerViewMode.Thumbnail,
            SuggestedStartLocation = PickerLocationId.Desktop,
        };
        dialog.FileTypeFilter.Add(".txt");
        dialog.FileTypeFilter.Add(".token");

        InitializeWithWindow.Initialize(dialog, App.MainWindow.Hwnd);
        StorageFile file = await dialog.PickSingleFileAsync();

        if (file != null)
        {
            TokenInput = await File.ReadAllTextAsync(file.Path);
        }
    }

    [RelayCommand]
    private async Task TestTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(TokenOutput))
        {
            return;
        }
        Stopwatch stopwatch = new();
        stopwatch.Start();
        string token = LazyCrypto.Decrypt(TokenOutput);
        if (!string.IsNullOrEmpty(await LazyRepository.GetFile(token)))
        {
            stopwatch.Stop();
            NoticeService.AddNotice("测试服务器令牌", $"测试通过：耗时 {stopwatch.ElapsedMilliseconds} 毫秒");
        }
        else
        {
            NoticeService.AddNotice("测试服务器令牌", $"测试失败：令牌有误或网络环境欠佳");
        }
    }
}
