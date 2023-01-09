using GenshinSwitch.Core;
using GenshinSwitch.Fetch.Regedit;

namespace GenshinSwitch.Fetch.Lazy;

public static class LazyOutputHelper
{
    public static readonly string Path = LazySpecialPathProvider.GetPath("genshin-lazy.yaml");

    public static void Save(string uid, string prod = null!, DateTime? dateTime = null!)
    {
        try
        {
            string fileIn = File.Exists(Path) ? File.ReadAllText(Path) : string.Empty;
            Dictionary<string, object> paramDict = LazyOutputSerializer.DeserializeObject<Dictionary<string, object>>(fileIn);
            List<LazyOutput> outputs = null!;

            paramDict ??= new();
            if (!paramDict.ContainsKey("Completed"))
            {
                outputs = new List<LazyOutput>();
                paramDict.Add("Completed", outputs);
            }
            if (outputs is not List<LazyOutput>)
            {
                outputs = new List<LazyOutput>();
                object l = paramDict["Completed"];

                if (l is List<object> ee)
                {
                    foreach (object eee in ee)
                    {
                        if (eee is Dictionary<object, object> eeee)
                        {
                            outputs.Add(new()
                            {
                                Uid = eeee[nameof(LazyOutput.Uid)]?.ToString()!,
                                Prod = eeee[nameof(LazyOutput.Prod)]?.ToString()!,
                                DateTimeUtc = eeee[nameof(LazyOutput.DateTimeUtc)]?.ToString()!,
                            });
                        }
                    }
                }
            }

            LazyOutput found = null!;
            outputs.ForEach(output =>
            {
                if (output.Uid == uid || output.Prod == prod)
                {
                    found = output;
                }
            });

            string dateTimeString = (dateTime ?? DateTime.Now).ToUniversalTime().ToDateTimeString();
            prod ??= GenshinRegedit.ProdCN ?? GenshinRegedit.ProdOVERSEA;

            if (found is null)
            {
                outputs.Add(new()
                {
                    Uid = uid,
                    Prod = prod,
                    DateTimeUtc = dateTimeString,
                });
            }
            else
            {
                found.Uid = uid;
                found.Prod = prod;
                found.DateTimeUtc = dateTimeString;
            }

            paramDict["Completed"] = outputs;

            string fileOut = LazyOutputSerializer.SerializeObject(paramDict);
            File.WriteAllText(Path, fileOut);
        }
        catch (Exception e)
        {
            Logger.Error(e.ToString());
        }
    }

    public static bool Check(string uid, string prod = null!)
    {
        try
        {
            if (!File.Exists(Path))
            {
                return false;
            }

            string fileIn = File.ReadAllText(Path);
            Dictionary<string, object> paramDict = LazyOutputSerializer.DeserializeObject<Dictionary<string, object>>(fileIn);

            uid ??= "0000000000";
            prod ??= GenshinRegedit.ProdCN ?? GenshinRegedit.ProdOVERSEA;

            if (paramDict != null)
            {
                if (paramDict.ContainsKey("Completed"))
                {
                    object l = paramDict["Completed"];

                    if (l is List<object> ee)
                    {
                        foreach (object eee in ee)
                        {
                            if (eee is Dictionary<object, object> eeee)
                            {
                                string uidIn = eeee[nameof(LazyOutput.Uid)]?.ToString()!;
                                string prodIn = eeee[nameof(LazyOutput.Prod)]?.ToString()!;

                                if (uidIn == uid || prodIn == prod)
                                {
                                    string dateTimeUtcStringIn = eeee[nameof(LazyOutput.DateTimeUtc)]?.ToString()!;
                                    DateTime? dateTimeUtcIn = dateTimeUtcStringIn.ToDateTime();

                                    if (dateTimeUtcIn != null)
                                    {
                                        if (UpdateTime.IsToday(dateTimeUtcIn.Value.AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours).ToUniversalTime()))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e.ToString());
        }
        return false;
    }
}
