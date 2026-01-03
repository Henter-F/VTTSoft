using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ICore.Log
{
    public  class AppLog
    {
        public static bool IsHourSplit = false;              //是否按小时拆分日志
        public static int MaxCacheCount = 1000;             //日志最大缓存数量
        public static string LogDirectory = "D:\\Log\\";
        private static AppLog gAppLog = null;
        public static AppLog Instance { get { if (gAppLog == null) gAppLog = new AppLog(); return gAppLog; } }
        private bool isClosed = false;
        private static bool isEnd = false;                  //一定用static  如果不是 在Release下卡在WaitWriteEnd函数
        private LogDataList outCacheList = new LogDataList();
        private Dictionary<string, LogDataList> outCacheDic = new Dictionary<string, LogDataList>();        //文件名称和日志列表键值对
        private static List<string> SubmitList = new List<string>();                                        //记录触发提交列表




        private static string GetFilePath(DateTime time, string fileName)
        {
            //fileName不包括扩展名
            string path = $"{LogDirectory}\\{time.Year}-{time.Month}-{time.Day}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (IsHourSplit)
                return path + $"\\{fileName}_" + time.Hour + ".txt";
            return path + $"\\{fileName}.txt";
        }
        /// <summary>
        /// Log日志等级
        /// </summary>
        public enum LogLevel
        {
            INFO,
            WARN,
            ERROR,
        }

        private class LogData
        {
            public string m_site = string.Empty;
            public AppLog.LogLevel m_level = AppLog.LogLevel.INFO;
            public string m_message = string.Empty;
            public string m_fileName = "Log";
            public LogData(AppLog.LogLevel level, string message, string fileName, string site = "")
            {
                m_level = level;
                m_message = message;
                m_fileName = fileName;
                m_site = site;
            }
        }
        private class LogDataList : List<LogData>
        {
            public void Write(string path, DateTime time)
            {
                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        foreach (LogData data in this)
                        {
                            string text = time.ToString("yyyy-MM-dd HH:mm:ss fff") + "  " + $"[{data.m_level.ToString()}]  " + data.m_message + System.Environment.NewLine;
                            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(text);
                            stream.Write(buffer, 0, buffer.Length);
                            stream.Flush();

                            if (data.m_level >= LogLevel.ERROR && !SubmitList.Contains(data.m_site))
                            {
                                SubmitList.Add(data.m_site);
                            }
                        }
                        stream.Close();
                    }
                }
                catch
                {
                }
            }
        }



        private AppLog()
        {
            Task.Factory.StartNew(Upate);
        }

        /// <summary>
        /// 启动日志扫描
        /// </summary>
        private void Upate()
        {
            while (true)
            {
                try
                {
                    System.Threading.Thread.Sleep(200);
                    //载入数据
                    lock (outCacheList)
                    {
                        if (isClosed && outCacheList.Count == 0) break;
                        foreach (LogData data in outCacheList)
                        {
                            if (outCacheDic.ContainsKey(data.m_fileName))
                                outCacheDic[data.m_fileName].Add(data);
                            else
                            {
                                LogDataList list = new LogDataList();
                                list.Add(data);
                                outCacheDic.Add(data.m_fileName, list);
                            }
                        }
                        outCacheList.Clear();
                    }
                    if (outCacheDic.Count == 0)
                        continue;
                    DateTime time = DateTime.Now;
                    foreach (var item in outCacheDic)
                    {
                        string fileName = item.Key;
                        LogDataList list = item.Value;
                        string path = GetFilePath(time, fileName);
                        list.Write(path, time);
                    }
                    outCacheDic.Clear();
                }
                catch //(Exception)
                {
                    break;
                }
            }
            isEnd = true;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="data"></param>
        private void Add(LogData data)
        {
            if (isClosed) return;
            lock (outCacheList)
            {
                if (outCacheList.Count >= MaxCacheCount)
                    return;
                outCacheList.Add(data);
            }
        }

        /// <summary>
        /// 程序未捕获的异常
        /// </summary>
        /// <param name="exp"></param>
        public static void AddException(Exception exp, string fileName)
        {
            string msg = exp.Message + System.Environment.NewLine
                + exp.ToString() + System.Environment.NewLine;
            if (exp.InnerException != null)
            {
                msg += exp.InnerException.Message + System.Environment.NewLine
                    + exp.InnerException.TargetSite + System.Environment.NewLine;
            }
            Instance.Add(new LogData(LogLevel.ERROR, msg, fileName));
        }

        /// <summary>
        /// 添加警告
        /// </summary>
        /// <param name="exp"></param>
        public static void AddWarn(string text, string fileName)
        {
            Instance.Add(new LogData(LogLevel.WARN, text, fileName));
        }

        /// <summary>
        /// 添加正常日志输出
        /// </summary>
        /// <param name="exp"></param>
        public static void AddINFO(string text, string fileName)
        {
            Instance.Add(new LogData(LogLevel.INFO, text, fileName));
        }


        /// <summary>
        /// 添加错误输出
        /// </summary>
        /// <param name="exp"></param>
        public static void AddException(string text, string fileName)
        {

            Instance.Add(new LogData(LogLevel.ERROR, text, fileName));
        }


        public void WaitToEnd()
        {
            isClosed = true;
            while (!isEnd)
            {

            }
        }

        /// <summary>
        /// 清除多少天前的日志
        /// </summary>
        /// <param name="beforeDay"></param>
        public static void Clear(int beforeDay = 7)
        {
            string directory = LogDirectory;
            try
            {
                if (!System.IO.Directory.Exists(directory)) return;
                DateTime now = System.DateTime.Now;
                List<string> deletes = new List<string>();
                DirectoryInfo dinfo = new DirectoryInfo(directory);
                foreach (DirectoryInfo temp in dinfo.GetDirectories())
                {
                    try
                    {
                        string name = temp.Name;
                        string[] arr = name.Split('-');
                        DateTime dt = new DateTime(int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]));
                        TimeSpan ts = now - dt;
                        if (ts.Days > beforeDay)
                            deletes.Add(temp.FullName);
                    }
                    catch
                    {
                        deletes.Add(temp.FullName);
                    }
                }
                foreach (string temp in deletes)
                    System.IO.Directory.Delete(temp, true);
            }
            catch
            {

            }
        }
    }
}
