using MachineVision.Extensions;
using MachineVision.View;
using MachineVision.ViewModels;
using MachineVision.Views;
using NLog;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MachineVision
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        

        protected override void RegisterTypes(IContainerRegistry service)
        {
            service.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            service.RegisterForNavigation<SettingsView, SettingsViewModel>();
            service.AddService();      
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MachineViewModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override void OnInitialized()
        {
            KillProcess("VisionMasterServerApp");
            KillProcess("VisionMaster");
            KillProcess("VmModuleProxy.exe");
            InitNlog();
            RegisterEvents();
            base.OnInitialized();
        }

        private void InitNlog()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // 获取应用程序目录
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string logFolder = Path.Combine(basePath, "Log", DateTime.Now.ToString("yyyyMMdd"));

            // 确保日志目录存在
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            // 创建Info日志的目标文件
            var infoLogFile = new NLog.Targets.FileTarget("infoLogFile")
            {
                FileName = Path.Combine(logFolder, "info_log.txt"),
                // 只记录Info级别的日志
                Layout = "${longdate} ${uppercase:${level}} ${message} ${exception:format=tostring}"
            };

            // 创建Error日志的目标文件
            var errorLogFile = new NLog.Targets.FileTarget("errorLogFile")
            {
                FileName = Path.Combine(logFolder, "error_log.txt"),
                // 记录Error及以上级别的日志
                Layout = "${longdate} ${uppercase:${level}} ${message} ${exception:format=tostring}"
            };

            // 创建控制台目标
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // 添加规则：Info级别日志到info_log.txt
            config.AddRule(LogLevel.Info, LogLevel.Info, infoLogFile);

            // 添加规则：Error及以上级别日志到error_log.txt
            config.AddRule(LogLevel.Error, LogLevel.Fatal, errorLogFile);

            // 添加规则：Info及以上级别日志到控制台
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);

            // 应用配置
            NLog.LogManager.Configuration = config;

        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //UI线程未捕获异常处理事件（UI主线程）
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                var exception = e.Exception as Exception;
                if (exception != null)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                e.SetObserved();
            }
        }

        //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //ignore
            }
        }

        //UI线程未捕获异常处理事件（UI主线程）
        private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                HandleException(e.Exception);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //处理完后，我们需要将Handler=true表示已此异常已处理过
                e.Handled = true;
            }
        }

        private static void HandleException(Exception e)
        {
            Logger.Error(e);
            MessageBox.Show("程序异常：" + e.Source + "\r\n@@" + Environment.NewLine + e.StackTrace + "\r\n##" + Environment.NewLine + e.Message);

            //记录日志
        }

        #region 关闭Vm
        void KillProcess(string strKillName)
        {
            foreach (var P in System.Diagnostics.Process.GetProcesses())
            {
                if (P.ProcessName.Contains(strKillName))
                {
                    try
                    {
                        P.Kill();
                        P.WaitForExit();
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
            }
        }
        #endregion
    }
}
