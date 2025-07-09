using IMVSL2LMeasureModuCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VM.Core;
using VM.PlatformSDKCS;

namespace MachineVision.View.Views
{
    /// <summary>
    /// MainTabView.xaml 的交互逻辑
    /// </summary>
    public partial class MainTabView : UserControl
    {
        public MainTabView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);
                if (null != vmEx)
                {
                    string strMsg = "InitControl failed. Error Code: " + Convert.ToString(vmEx.errorCode, 16);
                    MessageBox.Show(strMsg);
                }
                else
                {
                    return;
                }
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VmSolution.Instance.SyncRun();
            IMVSL2LMeasureModuTool l2ltool = (IMVSL2LMeasureModuTool)VmSolution.Instance["流程1.线线测量1"];
            VmProcedure vmProcess1 = (VmProcedure)VmSolution.Instance["流程1"];
            // ImageSource = vmProcess1;
            VmRenderControl.ModuleSource = vmProcess1;
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            try
            {
                VmSolution.Load("E:\\Visionmaster\\快速匹配\\角度2.sol");
               // IsEnable = false;
            }
            catch (VmException ex)
            {
                throw ex;
            }
        }
    }
}
