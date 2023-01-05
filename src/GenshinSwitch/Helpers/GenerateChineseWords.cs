using System.Text;

namespace GenshinSwitch.Helpers;

/// <summary>
/// https://blog.csdn.net/szsbell/article/details/105864814
/// </summary>
public static class GenerateChineseWords
{
    static GenerateChineseWords()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public static string Generate(int? countRequest = null!)
    {
        int count = countRequest ?? new Random().Next(2, 6);
        string chineseWords = string.Empty;
        Random rm = new();

        for (int i = 0; i < count; i++)
        {
            int regionCode = rm.Next(16, 56);
            int positionCode;

            if (regionCode == 55)
            {
                positionCode = rm.Next(1, 90);
            }
            else
            {
                positionCode = rm.Next(1, 95);
            }

            int regionCode_Machine = regionCode + 160;
            int positionCode_Machine = positionCode + 160;
            byte[] bytes = new byte[]
            {
                (byte)regionCode_Machine,
                (byte)positionCode_Machine,
            };

            chineseWords += Encoding.GetEncoding("gb2312").GetString(bytes);
        }
        return chineseWords;
    }
}
