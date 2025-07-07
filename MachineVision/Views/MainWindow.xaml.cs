using MachineVision.Core.Events;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Input;

namespace MachineVision.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEventAggregator aggregator;
        public MainWindow(IEventAggregator aggregator)
        {
            InitializeComponent();
            this.aggregator = aggregator;
            //注册提示消息
            aggregator.ResgiterSnackBarMessage(arg =>
            {
                Snackbar.MessageQueue?.Enqueue(arg.Message);
            });
        }

        private void ColorZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnMinClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaxClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Close();
                Environment.Exit(0);
            });
        }

       
    }
}
