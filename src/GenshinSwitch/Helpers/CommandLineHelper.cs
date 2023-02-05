using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace GenshinSwitch;

public class CommandLineHelper
{
    public static CommandLineHelper Current { get; private set; } = new(Environment.GetCommandLineArgs());
    public StringDictionary Parameters { get; private set; } = new();

    public CommandLineHelper(string[] args)
    {
        Regex spliter = new(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Regex remover = new(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        string param = null;
        string[] parts;

        // 有效参数格式如下:
        // {-,/,--}param{ ,=,:}((",')value(",'))
        // 例: 
        // -param1 value1 --param2 /param3:"Test-:-work" 
        //   /param4=happy -param5 '--=nice=--'
        foreach (string txt in args.Skip(1))
        {
            // 寻找新参数 (-,/ or --) 以及可能的封闭值 (=,:)
            parts = spliter.Split(txt, 3);

            switch (parts.Length)
            {
                // 找到一个值（用于找到的最后一个参数（空格分隔符））
                case 1:
                    if (param != null)
                    {
                        if (!Parameters.ContainsKey(param))
                        {
                            parts[0] = remover.Replace(parts[0], "$1");

                            Parameters.Add(param, parts[0]);
                        }
                        param = null;
                    }
                    break;

                // 只找到一个参数
                case 2:
                    // 最后一个参数仍在等待
                    // 如果没有值将其设置为true
                    if (param != null)
                    {
                        if (!Parameters.ContainsKey(param))
                        {
                            Parameters.Add(param, "true");
                        }
                    }
                    param = parts[1];
                    break;

                // 带封闭值的参数
                case 3:
                    // 最后一个参数仍在等待。
                    // 如果没有值将其设置为true
                    if (param != null)
                    {
                        if (!Parameters.ContainsKey(param))
                        {
                            Parameters.Add(param, "true");
                        }
                    }

                    param = parts[1];

                    // 删除可能的封闭字符 (",')
                    if (!Parameters.ContainsKey(param))
                    {
                        parts[2] = remover.Replace(parts[2], "$1");
                        Parameters.Add(param, parts[2]);
                    }

                    param = null;
                    break;
            }
        }

        // 以防参数仍在等待
        if (param != null)
        {
            if (!Parameters.ContainsKey(param))
            {
                Parameters.Add(param, bool.TrueString);
            }
        }
    }

    public static bool Has(string key) => Current.Parameters.ContainsKey(key);

    public static bool? GetValueBoolean(string key)
    {
        bool? ret = null;

        try
        {
            string value = Current.Parameters[key];

            if (!string.IsNullOrEmpty(value))
            {
                ret = Convert.ToBoolean(value);
            }
        }
        catch
        {
        }
        return ret;
    }

    public static int? GetValueInt32(string key)
    {
        int? ret = null;

        try
        {
            string value = Current.Parameters[key];

            if (!string.IsNullOrEmpty(value))
            {
                ret = Convert.ToInt32(value);
            }
        }
        catch
        {
        }
        return ret;
    }

    public static double? GetValueDouble(string key)
    {
        double? ret = null;

        try
        {
            string value = Current.Parameters[key];

            if (!string.IsNullOrEmpty(value))
            {
                ret = Convert.ToDouble(value);
            }
        }
        catch
        {
        }
        return ret;
    }

    public static bool IsValueBoolean(string key)
    {
        return GetValueBoolean(key) ?? false;
    }

    public string this[string key] => Parameters[key]!;
}
