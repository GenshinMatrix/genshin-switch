using System;
using System.Windows;
using System.Windows.Input;

namespace GenshinSwitch
{
    public class NotifyIconViewModel
    {
        public ICommand ShowWindowCommand => new DelegateCommand
        {
            CommandAction = () =>
            {
                Application.Current.MainWindow.Restore();
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.Activate();
            }
        };

        public ICommand HideWindowCommand => new DelegateCommand
        {
            CommandAction = () => Application.Current.MainWindow.Hide()
        };

        public ICommand DoubleClickTrayCommand => new DelegateCommand
        {
            CommandAction = () =>
            {
                if (Application.Current.MainWindow.Visibility == Visibility.Visible)
                    HideWindowCommand.Execute(null);
                else
                    ShowWindowCommand.Execute(null);
            }
        };

        public ICommand ExitApplicationCommand => new DelegateCommand
        { 
            CommandAction = () =>
            {
                App.TaskbarIcon?.Dispose();
                Application.Current.Shutdown();
            }
        };
    }

    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
