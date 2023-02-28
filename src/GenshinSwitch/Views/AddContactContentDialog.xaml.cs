using GenshinSwitch.Models;
using GenshinSwitch.Models.Messages;
using GenshinSwitch.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GenshinSwitch.Views;

public sealed partial class AddContactContentDialog : ContentDialog
{
    public AddContactViewModel ViewModel { get; }
    internal ContactMessage ContactMessage { get; private set; } = null!;

    public AddContactContentDialog(Contact contact = null!)
    {
        ViewModel = App.GetService<AddContactViewModel>();
        ViewModel.Reload(contact);
        InitializeComponent();

        ViewModel.PropertyChanged += (_, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.AliasName):
                    if (!string.IsNullOrEmpty(ViewModel.AliasName))
                    {
                        AliasNameTextBlockTeachingTip.IsOpen = false; /// Protect from stuck of <see cref="TeachingTip"/>
                    }
                    break;
            }
        };

        SecondaryButtonClick += async (s, e) =>
        {
            if (string.IsNullOrEmpty(ViewModel.AliasName))
            {
                e.Cancel = true;
                AliasNameTextBlockTeachingTip.IsOpen = false;
                await Task.Delay(1); /// Protect from stuck of <see cref="TeachingTip"/>
                AliasNameTextBlockTeachingTip.IsOpen = true;
            }

            if (!e.Cancel)
            {
                ContactMessage = new()
                {
                    Type = contact is null ? ContactMessage.ContactMessageType.Added : ContactMessage.ContactMessageType.Edited,
                    Contact = contact ?? new(),
                };

                ContactMessage.Contact.AliasName = ViewModel.AliasName;
                ContactMessage.Contact.LocalIconUri = ViewModel.LocalIconUri;
                ContactMessage.Contact.Server = ViewModel.Server;
                ContactMessage.Contact.Prod = ViewModel.Prod;
                ContactMessage.Contact.Cookie = ViewModel.Cookie;
            }
        };
    }
}
