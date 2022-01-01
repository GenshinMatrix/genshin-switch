using System;
using System.Diagnostics;
using System.IO;

namespace GenshinSwitch
{
    internal class ConfigCtrl
    {
        /// <summary>
        /// 默认配置文件名称
        /// </summary>
        public const string DefaultConfigName = "genshin-switch";

        /// <summary>
        /// 默认配置文件路径
        /// </summary>
        public static readonly string DefaultConfigPath = InitConfigPath($"{DefaultConfigName}.yaml", false);

        /// <summary>
        /// 配置操作接口
        /// </summary>
        public static YamlUtil ConfigUtil { get; set; } = new YamlUtil();

        static ConfigCtrl()
        {
        }

        /// <summary>
        /// 初始化配置文件路劲
        /// </summary>
        public static string InitConfigPath(string baseName, bool inLaunchPath = false)
        {
            if (inLaunchPath)
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(appPath, baseName);
            }
            else
            {
                string appUserPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string configPath = Path.Combine(Path.Combine(appUserPath, DefaultConfigName), baseName);

                if (!Directory.Exists(new FileInfo(configPath).DirectoryName))
                {
                    Directory.CreateDirectory(new FileInfo(configPath).DirectoryName);
                }
                return configPath;
            }
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        public static Config Init()
        {
            Config instance = null;

            if (File.Exists(DefaultConfigPath))
            {
                instance = Load();
            }
            else
            {
                Trace.WriteLine($"Config file `{DefaultConfigPath} not exists.");
            }

            if (instance == null)
            {
                // 从文件初始化失败
                // 转为使用默认值
                instance = new Config();

                // 第一次初始化就保存到文件
                // ※Model层实体还未赋值所以不要调用Save()
                SaveAs(DefaultConfigPath, instance);

                Trace.WriteLine($"Config reloaded from `{DefaultConfigPath} failed, so using default config.");
            }
            return instance;
        }

        /// <summary>
        /// 从默认路劲加载配置文件
        /// </summary>
        /// <returns>配置实例</returns>
        public static Config Load()
        {
            return LoadFrom(DefaultConfigPath);
        }

        /// <summary>
        /// 从指定路劲加载配置文件
        /// </summary>
        /// <returns>配置实例</returns>
        public static Config LoadFrom(string filename)
        {
            Trace.WriteLine($"Config loaded from `{filename}");

            ConfigUtil.FileName = filename;
            return ConfigUtil.Deserialize<Config>();
        }

        /// <summary>
        /// 向默认路劲保存配置文件
        /// </summary>
        /// <returns>操作结果</returns>
        public static bool Save()
        {
            return SaveAs(DefaultConfigPath);
        }

        /// <summary>
        /// 向指定路劲保存配置文件
        /// </summary>
        /// <param name="filename">路劲</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <returns>操作结果</returns>
        public static bool SaveAs(string filename, bool overwrite = true)
        {
            if (!overwrite && File.Exists(filename))
            {
                return true;
            }
            ConfigUtil.FileName = filename;
            return ConfigUtil.Serialize(Config.Instance);
        }

        /// <summary>
        /// 向指定路劲保存配置文件
        /// </summary>
        /// <param name="filename">路劲</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <returns>操作结果</returns>
        public static bool SaveAs(string filename, object obj, bool overwrite = true)
        {
            if (!overwrite && File.Exists(filename))
            {
                return true;
            }
            ConfigUtil.FileName = filename;
            return ConfigUtil.Serialize(obj);
        }
    }
}
