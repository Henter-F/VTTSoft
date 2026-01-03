using System;
using System.Collections.Generic;
using System.ICore.Interface;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using UIShell.OSGi;

namespace System.ICore.Log
{
    public  class LogUitl
    {
        private static bool IsClearing = false;
        private static ILogViewModel LogViewBox = null;


        /// <summary>
        /// 常规日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fileName"></param>
        public static void LogInfo(string text, string fileName = "调试")
        {
            AppLog.AddINFO(text, fileName);
            if (LogViewBox == null) LogViewBox = BundleRuntime.Instance?.GetFirstOrDefaultService<ILogViewModel>();
            if (LogViewBox != null && LogViewBox.PageModels != null)
            {
                foreach (var item in LogViewBox.PageModels)
                {
                    if (item.TabCaption == fileName.Trim())
                    {
                        RichTextBox control = item.TabContent;
                        WriteLog writeLog = new WriteLog(control);
                        writeLog.AddLogger(text, EnumLoggerType.Info);
                    }
                }
                ClearLog(LogViewBox.SaveLogIndate);
            }
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fileName"></param>
        public static void LogWarn(string text, string fileName = "调试")
        {
            AppLog.AddWarn(text, fileName);
            if (LogViewBox == null) LogViewBox = BundleRuntime.Instance?.GetFirstOrDefaultService<ILogViewModel>();
            if (LogViewBox != null && LogViewBox.PageModels != null)
            {
                foreach (var item in LogViewBox.PageModels)
                {
                    if (item.TabCaption == fileName.Trim())
                    {
                        RichTextBox control = item.TabContent;
                        WriteLog writeLog = new WriteLog(control);
                        writeLog.AddLogger(text, EnumLoggerType.Warning);
                    }
                }
            }
        }

        /// <summary>
        /// 报错日志
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="fileName"></param>
        public static void LogError(Exception exp, string fileName = "调试")
        {
            AppLog.AddException(exp, fileName);
            if (LogViewBox == null) LogViewBox = BundleRuntime.Instance?.GetFirstOrDefaultService<ILogViewModel>();
            if (LogViewBox != null && LogViewBox.PageModels != null)
            {
                foreach (var item in LogViewBox.PageModels)
                {
                    if (item.TabCaption == fileName.Trim())
                    {
                        RichTextBox control = item.TabContent;
                        WriteLog writeLog = new WriteLog(control);
                        string msg = exp.Message + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine;
                        if (exp.InnerException != null)
                        {
                            msg += exp.InnerException.Message + System.Environment.NewLine
                                + exp.InnerException.TargetSite + System.Environment.NewLine;
                        }
                        writeLog.AddLogger(msg, EnumLoggerType.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fileName"></param>
        public static void LogError(string text, string fileName = "调试")
        {
            AppLog.AddException(text, fileName);
            if (LogViewBox == null) LogViewBox = BundleRuntime.Instance?.GetFirstOrDefaultService<ILogViewModel>();
            if (LogViewBox != null && LogViewBox.PageModels != null)
            {
                foreach (var item in LogViewBox.PageModels)
                {
                    if (item.TabCaption == fileName.Trim())
                    {
                        RichTextBox control = item.TabContent;
                        WriteLog writeLog = new WriteLog(control);
                        writeLog.AddLogger(text, EnumLoggerType.Error);
                    }
                }
            }
            //List<ISqlSugar> SQL = ContainerManage.Instance?.Get<ISqlSugar>();
        }


        /// <summary>
        /// 清除日志
        /// </summary>
        /// <param name="beforeDay"></param>
        public static void ClearLog(int beforeDay = 7)
        {
            if (IsClearing == false)
            {
                Task t = new Task(() =>
                {
                    IsClearing = true;
                    AppLog.Clear(beforeDay);
                    IsClearing = false;
                });
                t.Start();
            }
        }

    }


    public class WriteLog
    {

        private RichTextBox richTextBoxRemote;
        /// <summary>
        /// 构造函数传入RichTextBox控件的实例。
        /// </summary>
        /// <param name="richTextBoxRemote"></param>
        public WriteLog(RichTextBox richTextBoxRemote)
        {
            this.richTextBoxRemote = richTextBoxRemote;
        }

        public void AddLogger(string msg, EnumLoggerType type)
        {
            try
            {

                if (richTextBoxRemote == null) return;
                //if (richTextBoxRemote == null || type == EnumLoggerType.Info) return;
                string t = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                richTextBoxRemote.Dispatcher.BeginInvoke((ThreadStart)delegate ()
                {
                    Run run = new Run();
                    switch (type)
                    {
                        case EnumLoggerType.Info:
                            run.Text = t + " 信息：" + msg;
                            run.Foreground = Brushes.Black;
                            break;
                        case EnumLoggerType.Warning:
                            run.Text = t + " 警告：" + msg;
                            run.Foreground = Brushes.BlueViolet;
                            break;
                        case EnumLoggerType.Error:
                            run.Text = t + " 错误：" + msg;
                            run.Foreground = Brushes.OrangeRed;
                            break;
                        default:
                            break;
                    }

                    Paragraph paragraph = new Paragraph(run)
                    {
                        LineHeight = 2,
                        FontSize = 12,
                    };

                    richTextBoxRemote.Document.Blocks.Add(paragraph);
                    // 滚动至最后行
                    richTextBoxRemote.ScrollToEnd();
                    // 删除
                    if (richTextBoxRemote.Document.Blocks.Count > 100)
                    {
                        for (int i = 100; i < richTextBoxRemote.Document.Blocks.Count; i++)
                        {
                            richTextBoxRemote.Document.Blocks.Remove(richTextBoxRemote.Document.Blocks.FirstBlock);
                        }
                    }
                });
            }
            catch (Exception)
            {
            }
        }
    }

    public enum EnumLoggerType
    {
        Info = 0,
        Warning,
        Error,
    }
}
