using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Controls;
using GenshinSwitch.Controls.Notice;
using GenshinSwitch.Core;
using GenshinSwitch.Core.Settings;
using GenshinSwitch.Fetch.Launch;
using GenshinSwitch.Fetch.Regedit;
using GenshinSwitch.Models;
using GenshinSwitch.Models.Messages;
using GenshinSwitch.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace GenshinSwitch.ViewModels;

public class MainViewModel : ObservableRecipient
{
    public DispatcherTimer DispatcherTimer { get; } = new();
    public ObservableCollection<Contact> Contacts { get; set; } = new();
    public ICommand AddContactCommand { get; }

    public MainViewModel()
    {
        DispatcherTimer.Interval = TimeSpan.FromSeconds(2);
        DispatcherTimer.Tick += (s, e) =>
        {
            if (LaunchCtrl.TryGetProcessRegion(out string region))
            {
                string runningProd = region switch
                {
                    LaunchCtrl.RegionOVERSEA => GenshinRegedit.ProdOVERSEA,
                    LaunchCtrl.RegionCN or _ => GenshinRegedit.ProdCN,
                };

                foreach (Contact contact in Contacts)
                {
#if !DEBUG
                    if (!LaunchCtrl.GetElevated())
                    {
                        break;
                    }
#endif
                    if (contact.Prod == runningProd)
                    {
                        contact.IsRunning = true;
                    }
                    else
                    {
                        contact.IsRunning = false;
                    }
                }
            }
            else
            {
                foreach (Contact contact in Contacts)
                {
                    contact.IsRunning = false;
                }
            }
        };
        DispatcherTimer.Start();

        AddContactCommand = new RelayCommand(async () =>
        {
            string prop = GenshinRegedit.ProdCN;
            var found = Settings.Contacts.Get().Where(kv => kv.Value.Prod == prop);

            if (found.Any())
            {
                Bubble.Warning($"当前账号已被添加为 {found.First().Value.AliasName}");
#if !DEBUG
                return;
#endif
            }

            AddContactContentDialog dialog = new()
            {
                XamlRoot = App.MainWindow.XamlRoot,
                RequestedTheme = App.MainWindow.ActualTheme,
            };

            if (await dialog.ShowAsync() == ContentDialogResult.Secondary)
            {
                AddOrUpdateContact(dialog.ContactMessage);
            }
        });
    }

    public void OnContactListViewItemClick(object sender, ItemClickEventArgs e)
    {
        if (ListViewHelper.TryRaiseItemDoubleClick(sender, e))
        {
            LaunchGameAsync((Contact)e.ClickedItem).ConfigureAwait(false);
        }
    }

    public void OnConcactDragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs e)
    {
        _ = sender;
        _ = e;

        Dictionary<string, Contact> dict = new();
        Dictionary<string, Contact> contacts = Settings.Contacts.Get();

        foreach (Contact contact in Contacts)
        {
            dict.Add(contact.Guid, contacts[contact.Guid]);
        }
        Settings.Contacts.Set(dict);
        SettingsManager.Save();
    }

    public async Task LaunchGameAsync(Contact contact)
    {
        try
        {
            await LaunchCtrl.LaunchAsync(relaunchMethod: Settings.RelaunchMethod.Get(), launchParameter: new LaunchParameter()
            {
                Region = contact.RegionName,
                Prod = contact.Prod,
            });
        }
        catch (Exception e)
        {
            NoticeService.AddNotice(string.Empty, e.Message);
        }
    }

    private void AddOrUpdateContact(ContactMessage msg)
    {
        Dictionary<string, Contact> dict = Settings.Contacts.Get();

        if (msg.Type == ContactMessage.ContactMessageType.Added)
        {
            dict.Add(msg.Contact.Guid, msg.Contact);
            Contacts.Add(msg.Contact);
            Bubble.Success($"添加 {msg.Contact.AliasName} 成功");
        }
        else if (msg.Type == ContactMessage.ContactMessageType.Edited)
        {
            if (dict.ContainsKey(msg.Contact.Guid))
            {
                dict[msg.Contact.Guid] = msg.Contact;
                Contacts.Add(msg.Contact);
            }
            else
            {
                Logger.Fatal($"[AddOrUpdateContact] Lag of {msg.Contact.Guid}");
                Debugger.Break();
                return;
            }
        }
        else if (msg.Type == ContactMessage.ContactMessageType.Removed)
        {
            dict.Remove(msg.Contact.Guid);
            _ = Contacts.TakeWhile(c => c.Guid == msg.Contact.Guid);
        }

        Settings.Contacts.Set(dict);
        SettingsManager.Save();
        WeakReferenceMessenger.Default.Send(msg);
    }
}
