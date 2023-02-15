using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace JManager_Edge
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Button Btn_MainGR;
        Button Btn_MainMP3;
        Device_Button[] Btn_DeviceList_ = new Device_Button[60];
        DispatcherTimer timer;

        Device_Data_Function deviceData = Device_Data_Function.instance;

        public MainWindow()
        {
            InitializeComponent();
            Init();
            Add_Btn();
            Init_Devices();

            Thread watch_device_data_change_thread = new Thread(new ThreadStart(Update_Device_Button));
            watch_device_data_change_thread.Start();
        }

        private void Init()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(0.01);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Add_Btn()
        {
            for (int i = 0; i < 20; i++)
            {
                Btn_MainMP3 = new Button();
                Btn_MainMP3.Width = 300;
                Btn_MainMP3.Height = 94;

                Btn_MainMP3.HorizontalAlignment = HorizontalAlignment.Left;
                Btn_MainMP3.VerticalAlignment = VerticalAlignment.Top;
                //Btn_MainGR.Margin = new Thickness(iPosX, iPosY, 0, 0);
                Btn_MainMP3.Name = "Btn_MainMP3" + (i + 1);
                Btn_MainMP3.Content = "" + (i + 1);
                Btn_MainMP3.Click += new RoutedEventHandler(Btn_MainMP3_Click);
                WPanel_MP3.Children.Add(Btn_MainMP3);
            }

            for (int i = 0; i < 16; i++)
            {
                Btn_MainGR = new Button();
                Btn_MainGR.Width = 140;
                Btn_MainGR.Height = 140;

                Btn_MainGR.HorizontalAlignment = HorizontalAlignment.Left;
                Btn_MainGR.VerticalAlignment = VerticalAlignment.Top;
                //Btn_MainGR.Margin = new Thickness(iPosX, iPosY, 0, 0);
                Btn_MainGR.Name = "Btn_MainGR" + (i + 1);
                Btn_MainGR.Content = "" + (i + 1);
                Btn_MainGR.Click += new RoutedEventHandler(Btn_MainGR_Click);
                WPanel_GR.Children.Add(Btn_MainGR);
            }

            for (int i = 0; i < Btn_DeviceList_.Length; i++)
            {

                Btn_DeviceList_[i] = new Device_Button(i);
                WPanel_DList.Children.Add(Btn_DeviceList_[i]);
            }
        }
        private void Init_Devices()
        {
            //Devices null값 오류 방지를 위해 kind 3으로 초기화(3은 등록되지 않은 상태임.)
            for (int i = 0; i < Btn_DeviceList_.Length; i++)
            {
                deviceData.Devices[i] = new Device("", "", 3, "", "");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Lb_Timer.Content = DateTime.Now.ToLongTimeString();
            Lb_Week.Content = DateTime.Now.DayOfWeek.ToString();
            Lb_Date.Content = DateTime.Now.ToShortDateString();
        }

        private void SendUrl(string url)
        {
            string responseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            System.Net.NetworkCredential netCredential = new System.Net.NetworkCredential("root", "1", "192.168.21.193");
            request.Credentials = netCredential;

            //request.Timeout = 30 * 1000; // 30초
            //request.Headers.Add("Authorization", "BASIC SGVsbG8="); // 헤더 추가 방법


            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                HttpStatusCode status = resp.StatusCode;
                Console.WriteLine(status);  // 정상이면 "OK"

                Stream respStream = resp.GetResponseStream();
                using (StreamReader sr = new StreamReader(respStream))
                {
                    responseText = sr.ReadToEnd();
                }
            }

            Console.WriteLine(responseText);
            DataPars(responseText);
        }

        private void DataPars(string data)
        {
            string[] sData_Pasrs;
            string[] sData_Pasrs2;
            sData_Pasrs = data.ToString().Split('\n');
            for (int i = 0; i < (sData_Pasrs.Length - 1); i++)
            {
                sData_Pasrs[i] = sData_Pasrs[i].Substring(5);
                sData_Pasrs2 = sData_Pasrs[i].Split('.');

                string sdata;

                switch (sData_Pasrs2[0])
                {
                    case "AudioSource":
                        sdata = sData_Pasrs2[sData_Pasrs2.Length - 1];
                        sData_Pasrs2 = sdata.Split('=');
                        switch (sData_Pasrs2[0])
                        {
                            case "InputGain":
                                MessageBox.Show(sData_Pasrs2[0]);
                                break;
                            case "OutputGain":
                                MessageBox.Show(sData_Pasrs2[0]);
                                break;
                            case "InputType":
                                MessageBox.Show(sData_Pasrs2[0]);
                                break;
                        }
                        break;

                    case "MediaClip":
                        sdata = sData_Pasrs2[sData_Pasrs2.Length - 1];
                        sData_Pasrs2 = sdata.Split('=');

                        switch (sData_Pasrs2[0])
                        {
                            case "InputGain":
                                MessageBox.Show(sData_Pasrs2[0]);
                                break;
                            case "OutputGain":
                                MessageBox.Show(sData_Pasrs2[0]);
                                break;
                            case "InputType":
                                MessageBox.Show(sData_Pasrs2[0]);
                                break;
                        }
                        break;
                }
            }
        }
        private void Btn_MainMP3_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
        }

        private void Btn_MainGR_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;


        }

        private void Btn_DeviceList_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            MessageBox.Show(btn.Tag.ToString());


        }

        private void Btn_DRun_Click(object sender, RoutedEventArgs e)
        {
            Grid_DRun.Visibility = Visibility.Visible;
            Grid_DSchedule.Visibility = Visibility.Hidden;
            Grid_DSetting.Visibility = Visibility.Hidden;
        }

        private void Btn_DSchedule_Click(object sender, RoutedEventArgs e)
        {
            Grid_DRun.Visibility = Visibility.Hidden;
            Grid_DSchedule.Visibility = Visibility.Visible;
            Grid_DSetting.Visibility = Visibility.Hidden;
        }

        private void Btn_DSetting_Click(object sender, RoutedEventArgs e)
        {
            Grid_DRun.Visibility = Visibility.Hidden;
            Grid_DSchedule.Visibility = Visibility.Hidden;
            Grid_DSetting.Visibility = Visibility.Visible;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://192.168.21.193/axis-cgi/param.cgi?action=list";
            //SendUrl(url);
        }


        private void Update_Device_Button()
        {
            //Devices 배열 기반으로 Button들 업데이트 하는 Thread

            while (true)
            {
                Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < Btn_DeviceList_.Length; i++)
                    {

                        if (deviceData.Devices[i] != null)
                        {
                            MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
                            switch (deviceData.Devices[i].Kind)
                            {
                                case (0): packIcon.Kind = PackIconKind.Speaker; break;
                                case (1): packIcon.Kind = PackIconKind.Cctv; break;
                                case (2): packIcon.Kind = PackIconKind.MobileDevices; break;
                                case (3): packIcon.Kind = PackIconKind.HelpCircleOutline; Btn_DeviceList_[i].Button_.Background = Brushes.Gray; break;
                            }

                            //디바이스가 저장되어있음(등록 되어있음)
                            if (deviceData.Devices[i].IP != "")
                            {
                                if (!Ping_(deviceData.Devices[i].IP))
                                {
                                    //디바이스가 연결 안되어있음
                                    Btn_DeviceList_[i].Button_.Background = Brushes.Orange;
                                }
                                else
                                {
                                    //디바이스가 연결 되어있음
                                    Btn_DeviceList_[i].Button_.Background = Brushes.Green;
                                }
                            }

                            Btn_DeviceList_[i].device_icon = packIcon;
                            Btn_DeviceList_[i].device_name_box.Text = deviceData.Devices[i].Name;

                        }
                        else
                        {
                            MessageBox.Show("Device 정보를 불러오는데 실패했습니다. 오류");
                        }


                    }
                });
                Thread.Sleep(1000);


            }
        }

        private bool Ping_(string ip)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "test";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            string address = ip;

            PingReply reply = pingSender.Send(address, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                //실패
                return false;
            }

        }

    }
}
