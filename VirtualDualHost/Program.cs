using System;
using System.Windows.Forms;
using StandardFeature;
namespace VirtualDualHost
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            #region MyRegion

            //处理未捕获的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            #endregion
            Application.Run(new Form_Main());
            glExitApp = true;//标志应用程序可以退出
        }
        /// <summary>
        /// 是否退出应用程序
        /// </summary>
        static bool glExitApp = false;

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.LogError(sender.GetType().Name, e.ToString());
            LogHelper.LogError(sender.GetType().Name, e.ExceptionObject.ToString());

            while (true)
            {//循环处理，否则应用程序将会退出
                if (glExitApp)
                {//标志应用程序可以退出，否则程序退出后，进程仍然在运行
                    LogHelper.LogError(sender.GetType().Name, "ExitApp :" + e.ExceptionObject.ToString());
                    return;
                }
                System.Threading.Thread.Sleep(2 * 1000);
            };
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

            //string row = e.Exception.StackTrace.Substring(e.Exception.StackTrace.IndexOf("行号"), e.Exception.StackTrace.Length - e.Exception.StackTrace.IndexOf("行号"));

            LogHelper.LogError(sender.GetType().Name, "Application_ThreadException:" +
                e.Exception.Message);
            LogHelper.LogError(sender.GetType().Name, e.Exception);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 取得当前源码的哪一行
        /// </summary>
        /// <returns></returns>
        public static int GetLineNum()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileLineNumber();
        }

        /// <summary>
        /// 取当前源码的源文件名
        /// </summary>
        /// <returns></returns>
        public static string GetCurSourceFileName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);

            return st.GetFrame(0).GetFileName();

        }
    }
}
