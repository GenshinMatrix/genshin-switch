using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Vanara.PInvoke;

namespace GenshinSwitch.Controls;

public class ListViewHelper
{
    private static readonly uint DoubleClickInterval = User32.GetDoubleClickTime();

    public static DateTime GetItemDoubleClickDateTime(DependencyObject obj) => (DateTime)obj.GetValue(ItemDoubleClickDateTimeProperty);
    public static void SetItemDoubleClickDateTime(DependencyObject obj, DateTime value) => obj.SetValue(ItemDoubleClickDateTimeProperty, value);
    public static readonly DependencyProperty ItemDoubleClickDateTimeProperty = DependencyProperty.RegisterAttached("ItemDoubleClickDateTime", typeof(DateTime), typeof(ListViewHelper), new(DateTime.MinValue));

    public static int GetItemDoubleClickHash(DependencyObject obj) => (int)obj.GetValue(ItemDoubleClickHashProperty);
    public static void SetItemDoubleClickHash(DependencyObject obj, int value) => obj.SetValue(ItemDoubleClickHashProperty, value);
    public static readonly DependencyProperty ItemDoubleClickHashProperty = DependencyProperty.RegisterAttached("ItemDoubleClickHash", typeof(int), typeof(ListViewHelper), new(default));

    public static bool TryRaiseItemDoubleClick(object sender, ItemClickEventArgs e)
    {
        if (sender is ListView listView)
        {
            int lastHash = GetItemDoubleClickHash(listView);
            int nowHash = e.ClickedItem.GetHashCode();
            DateTime lastTime = GetItemDoubleClickDateTime(listView);
            DateTime nowTime = DateTime.Now;

            SetItemDoubleClickHash(listView, nowHash);
            SetItemDoubleClickDateTime(listView, nowTime);
            if (lastHash == nowHash && (nowTime - lastTime).TotalMilliseconds <= DoubleClickInterval)
            {
                return true;
            }
        }
        return false;
    }
}
