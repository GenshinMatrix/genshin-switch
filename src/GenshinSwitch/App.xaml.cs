using GenshinSwitch.Activation;
using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Core;
using GenshinSwitch.Core.Contracts.Services;
using GenshinSwitch.Core.Services;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
using GenshinSwitch.Notifications;
using GenshinSwitch.Services;
using GenshinSwitch.ViewModels;
using GenshinSwitch.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using Xunkong.Hoyolab;

namespace GenshinSwitch;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host { get; }
    public static bool IsElevated { get; } = GetElevated();

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static MainWindow MainWindow { get; } = new MainWindow();

    public App()
    {
        Logger.Info("Startup...");

        Task.Run(async () =>
        {
            var cookie = "account_mid_v2=01d2hootvp_mhy;cookie_token_v2=v2_prE4Gysvxm5PITJ96ymhcPhGdNtXUIUfyTBlKUXgwWVSZOsPnc056kmth6YWVqNVCycci62FHW1OsOOw7lfiViyvx3gi6o3J_J-u8hfvqZvFSKbmYdE=";
            var client = new HoyolabClient();
            //string s = await client.GetAppVersion();
            return;
            // 米游社账号
            var user = await client.GetHoyolabUserInfoAsync(cookie);
            // 原神账号
            
            var roles = await client.GetGenshinRoleInfosAsync(cookie);
            var role = roles[0];
            // 签到信息
            var signInfo = await client.GetSignInInfoAsync(role);
            // 开始签到
            //var isSigned = await client.SignInAsync(role);
            // 战绩
            var summary = await client.GetGameRecordSummaryAsync(role);
            // 角色
            var details = await client.GetAvatarDetailsAsync(role);
            // 角色技能
            var skills = await client.GetAvatarSkillLevelAsync(role, details.FirstOrDefault()?.Id ?? 0);
            // 活动
            var act = await client.GetActivitiesAsync(role);
            // 便笺
            var dailynote = await client.GetDailyNoteAsync(role);
            // 札记
            var travelNotesSummary = await client.GetTravelNotesSummaryAsync(role);
            // 深渊
            var spirall = await client.GetSpiralAbyssInfoAsync(role, 1);
            // 新闻列表
            var newsList = await client.GetNewsListAsync(Xunkong.Hoyolab.News.NewsType.Announce);
            // 新闻内容
            var newsDetail = await client.GetNewsDetailAsync(newsList.FirstOrDefault()?.PostId ?? 0);
        });

        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<IWebViewService, WebViewService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ISpecialPathService, SpecialPathService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<WebViewViewModel>();
            services.AddTransient<WebViewPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<AddContactViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();
        EnsureElevated();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    public static void RestartAsElevated(int? exitCode = null)
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                Verb = "runas",
                UseShellExecute = true,
                FileName = Environment.ProcessPath,
                WorkingDirectory = Environment.CurrentDirectory,
            });
        }
        catch (Win32Exception)
        {
            return;
        }
        Environment.Exit(exitCode ?? 0);
    }

    public void EnsureElevated()
    {
#if !DEBUG
        if (!IsElevated)
        {
            RestartAsElevated('r' + 'u' + 'n' + 'a' + 's');
        }
#endif
    }

    private static bool GetElevated()
    {
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        Logger.Fatal(e.Exception);
        e.Handled = true;
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        //App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
