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
    /// Device_Button.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Device_Button : UserControl
    {
        Device_Data deviceData = Device_Data.instance;
        private int arr_num;
        MaterialDesignThemes.Wpf.PackIcon pi = null; //나중에 사라질 것

        public Device_Button(int i)
        {
            InitializeComponent(); 
            arr_num = i;
        }

        private void left_Btn(object sender, RoutedEventArgs e)
        {
            //Button_.BorderThickness = BorderThicknessPro;
            if(Button_.BorderBrush == Brushes.Red)
            {
                Button_.BorderBrush = Brushes.Transparent;
            }
            else
            {
                Button_.BorderBrush = Brushes.Red;
            }
                
        }

        private void Add_Device_Btn(object sender, RoutedEventArgs e)
        {
            //장치 추가
            Device_Add add_device = new Device_Add(arr_num);
            add_device.ShowDialog();
            //디바이스 설정 저장(Singleton Pattern)
        }

        private void Mod_Device_Btn(object sender, RoutedEventArgs e)
        {
            //장치 수정
            //새로운 창 추가로 구현 요망
            Device_Add add_device = new Device_Add(arr_num);
            add_device.ip_address_.Text = deviceData.Devices[arr_num].IP;
            add_device.name_.Text = deviceData.Devices[arr_num].Name;
            if (deviceData.Devices[arr_num].Kind != 3)
            {
                add_device.kind_.SelectedIndex = deviceData.Devices[arr_num].Kind;

                add_device.Show();
            }
            else
            {
                MessageBox.Show("장치를 먼저 등록해주세요.");
            } 

        }

        private void Set_Device_Btn(object sender, RoutedEventArgs e)
        {
            //장치 설정
            if (deviceData.Devices[arr_num].Kind != 3)
            {
                Device_Set set_device = new Device_Set(arr_num);
                set_device.ShowDialog();
            }
            else
            {
                MessageBox.Show("장치를 먼저 등록해주세요.");
            }
        }

        private void Del_Device_Btn(object sender, RoutedEventArgs e)
        {
            //장치 삭제
            MessageBoxResult messageboxresult = MessageBox.Show("이 장치를 삭제하시겠습니까? 이 작업은 되돌릴 수 없습니다.\n", "저장", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (messageboxresult == MessageBoxResult.OK)
            {
                deviceData.Devices[arr_num] = new Device("", "", 3,"","");

            }
        }
    }
}
