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
        Device_Data deviceData = Device_Data.instance;
        private int arr_num;
        bool loaded = false;

        public Device_Set(int i)
        {
            InitializeComponent();
            this.arr_num = i;
            loaded = true;
        }

        private void restart(object sender, RoutedEventArgs e)
        {
        //장치 재시작
            string url = "http://" + deviceData.Devices[arr_num].IP + "/axis-cgi/restart.cgi"; 
            deviceData.Send_Url(url, arr_num);
            system_.Text += "장치가 재부팅 됩니다.\n";
        }

        private void factory_reset(object sender, RoutedEventArgs e)
        {
            //공장 초기화
            //string url = "http://" + deviceData.Devices[arr_num].IP + "/axis-cgi/hardfactorydefault.cgi";
            //deviceData.Send_Url(url, arr_num);
            //system_.Text += "장치 공장초기화를 시작합니다. \n";

        }

        private void sound_correction(object sender, RoutedEventArgs e)
        {
            //보정 테스트


        }

        private void close(object sender, RoutedEventArgs e)
        {
            //돌아가기
            this.Close();
        }


        private void gain_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(loaded)
            {
                string url = "http://" + deviceData.Devices[arr_num].IP + "/axis-cgi/param.cgi?action=update&AudioSource.A0.OutputGain=" + gain_slider.Value;
                string result = deviceData.Send_Url(url, arr_num);
                system_.Text += "Gain 값" + gain_slider.Value.ToString() + "로 설정되었습니다.\n";
                system_.Text += result;
            }
        }

    }
}
