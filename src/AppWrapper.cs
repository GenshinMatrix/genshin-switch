using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace GenshinSwitch
{
    public class AppWrapper
    {
        #region [公有 静态属性]
        /// <summary>
        /// 是否支持程序多开
        /// ※默认不支持多开
        /// </summary>
        protected static bool mutilInstanceEnabled = false;
        [DefaultValue(false)]
        public static bool IsMutilInstanceEnabled
        {
            get => mutilInstanceEnabled;
            set
            {
                if (mutex == null)
                {
                    mutilInstanceEnabled = value;
                }
            }
        }

        /// <summary>
        /// 获取是否已有启动中程序单例
        /// </summary>
        public static bool IsAnotherInstanceStarted
        {
            get
            {
                if (mutex == null)
                {
                    mutex = new Mutex(true, InstanceName, out createdMutex);
                }
                return !createdMutex;
            }
        }

        /// <summary>
        /// 程序单例名
        /// </summary>
        public static string InstanceName { get; protected set; } = string.Empty;
        #endregion

        #region [私有 静态字段]
        /// <summary>
        /// 是否有程序单例
        /// </summary>
        protected static bool createdMutex = false;

        /// <summary>
        /// 程序单例锁
        /// </summary>
        protected static Mutex mutex = null;
        #endregion

        #region [私有 静态方法]
        /// <summary>
        /// 呈现程序实例名称
        /// </summary>
        /// <param name="programName">程序名称</param>
        /// <returns>程序实例名称</returns>
        private static string RenderInstanceName(string programName)
        {
            bool InRange(int index, int? count)
            {
                if (count == null)
                {
                    return false;
                }
                if (index >= 0 && index < count)
                {
                    return true;
                }
                return false;
            }
            StringBuilder sb = new StringBuilder();

            for (int i = default; i < programName.Length; i++)
            {
                char chr = programName[i];

                if (i > 0)
                {
                    if (char.IsUpper(chr))
                    {
                        if (InRange(i - 1, programName.Length))
                        {
                            char chrPrev = programName[i - 1];

                            if (!char.IsUpper(chrPrev))
                            {
                                sb.Append("_");
                            }
                        }
                        sb.Append(chr.ToString().ToLower());
                    }
                    else
                    {
                        sb.Append(chr);
                    }
                }
                else
                {
                    sb.Append(chr.ToString().ToLower());
                }
            }

            string name = $"{sb}_single_instance";

            return name;
        }
        #endregion

        #region [公有 静态方法]
        /// <summary>
        /// 初始化程序单例名
        /// </summary>
        /// <param name="mainFormType">主窗体类型</param>
        public static void InitInstanceName(Type mainFormType)
        {
            InstanceName = RenderInstanceName(GetAssemblyTitle(mainFormType.Assembly));
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <returns>程序集名称</returns>
        public static string GetAssemblyTitle(Assembly assembly = null)
        {
            AssemblyTitleAttribute attr = GetAssembly<AssemblyTitleAttribute>(assembly);

            return attr?.Title ?? Path.GetFileNameWithoutExtension((assembly ?? Assembly.GetExecutingAssembly()).CodeBase);
        }

        /// <summary>
        /// 获取指定类型首个程序集
        /// </summary>
        /// <typeparam name="TAssy">程序集类型</typeparam>
        /// <returns>程序集</returns>
        public static TAssy GetAssembly<TAssy>(Assembly assembly = null)
        {
            TAssy[] assemblies = GetAssemblies<TAssy>(assembly);

            if (assemblies.Length > 0)
            {
                return assemblies[0];
            }
            return default;
        }

        /// <summary>
        /// 获取指定类型程序集集合
        /// </summary>
        /// <typeparam name="TAssy">程序集类型</typeparam>
        /// <returns>程序集集合</returns>
        public static TAssy[] GetAssemblies<TAssy>(Assembly assembly = null)
        {
            object[] attributes = (assembly ?? Assembly.GetExecutingAssembly()).GetCustomAttributes(typeof(TAssy), false);
            List<TAssy> attributeList = new List<TAssy>();

            foreach (object attribute in attributes)
            {
                if (attribute is TAssy)
                {
                    attributeList.Add((TAssy)attribute);
                }
            }
            return attributeList.ToArray();
        }

        /// <summary>
        /// 以字符串形式程序本类
        /// </summary>
        /// <returns>字符串</returns>
        public static new string ToString() => InstanceName;
        #endregion
    }
}
