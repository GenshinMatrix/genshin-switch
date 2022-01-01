using System;
using System.Diagnostics;
using System.IO;
using YamlDotNet.Serialization;

namespace GenshinSwitch
{
    public class YamlUtil
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 序列化(保存文件)
        /// </summary>
        /// <param name="obj">序列化实体</param>
        /// <returns>操作结果</returns>
        public bool Serialize<T>(T obj)
        {
            bool ret = false;

            try
            {
                Serializer serializer = new Serializer();
                string str = serializer.Serialize(obj);

                using StreamWriter sw = File.CreateText(FileName);

                sw.Write(str);
                sw.Flush();
#if YAMLOUT && DEBUG
                Logger.Info($"Save to file `{FileName} :{Environment.NewLine}{str}");
#endif
                ret = true;
            }
            catch (Exception e)
            {
                if (FileName != null)
                {
                    Trace.WriteLine($"Serialize file `'{FileName}' failed! {e}");
                }
                else
                {
                    Trace.WriteLine($"Serialize file named null failed! {e}");
                }
            }
            return ret;
        }

        /// <summary>
        /// 反序列化(读取文件)
        /// </summary>
        /// <returns>反序列化实体</returns>
        public T Deserialize<T>()
        {
            T info = default(T);
            StreamReader reader = null;

            try
            {
                Deserializer deserializer = new Deserializer();

                reader = new StreamReader(FileName);
                info = deserializer.Deserialize<T>(reader);
            }
            catch (Exception e)
            {
                if (FileName != null)
                {
                    Trace.WriteLine($"Deserialize file '{FileName}' failed! {e}");
                }
                else
                {
                    Trace.WriteLine("Deserialize file named `'null' failed! {e}");
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return info;
        }
    }
}
