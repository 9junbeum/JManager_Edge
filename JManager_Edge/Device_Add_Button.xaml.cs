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

namespace JManager_Edge
{
    /// <summary>
    /// Device_Add_Button.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Device_Add_Button : UserControl
    {
        Device_Data device = Device_Data.instance;
        public Device_Add_Button()
        {
            InitializeComponent();
        }

        private void Button__Click(object sender, RoutedEventArgs e)
        {
            //장치 추가
            Device_Add add_device = new Device_Add(device.Devices.Length);
            add_device.ShowDialog();
            //디바이스 설정 저장(Singleton Pattern)
        }
    }
}
