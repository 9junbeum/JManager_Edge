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
using System.Windows.Shapes;

namespace JManager_Edge
{
    /// <summary>
    /// Device_Set.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Device_Set : Window
    {
        Device_Data_Function deviceData = Device_Data_Function.instance;
        private int arr_num;
        public Device_Set(int i)
        {
            InitializeComponent();
            arr_num = i;
        }

        private void restart(object sender, RoutedEventArgs e)
        {

        }

        private void factory_reset(object sender, RoutedEventArgs e)
        {

        }

        private void sound_correction(object sender, RoutedEventArgs e)
        {

        }

        private void confirm(object sender, RoutedEventArgs e)
        {

        }

        private void close(object sender, RoutedEventArgs e)
        {
            //돌아가기
            this.Close();
        }
    }
}
