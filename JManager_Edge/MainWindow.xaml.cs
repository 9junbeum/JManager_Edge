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
using System.Security.Policy;

namespace JManager_Edge
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Schedule> schedules = new List<Schedule>();

        Button[] Btn_MainGR = new Button[16];
        Button[] Btn_SetGR = new Button[15];
        Button[] Btn_ScheduleGR = new Button[16];
        Button[] Btn_MainMP3 = new Button[20];
        Button[] Btn_ScheduleMP3 = new Button[20];
        Button[] Btn_ScheduleDay = new Button[7];
        Device_Button[] Btn_DeviceList = new Device_Button[48];

        Device_Data deviceData = Device_Data.instance;
        DispatcherTimer timer;

        bool b_Timer = false;
        bool b_Day = false;
        int i_TimerIndex = -1;

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
            schedules = new List<Schedule>();

            //Cb_Stop_H.IsEnabled = false;
            Cb_Start_H.SelectedIndex = 0;
            Cb_Start_M.SelectedIndex = 0;
            Cb_Start_S.SelectedIndex = 0;
            Txt_ScheduleName.MaxLength = 6;


            for (int i = 0; i < 60; i++)
            {
                if (i < 24)
                {
                    if (i < 10)
                    {
                        Cb_Start_H.Items.Add("0" + i);
                        Cb_Stop_H.Items.Add("0" + i);
                    }
                    else
                    {
                        Cb_Start_H.Items.Add("" + i);
                        Cb_Stop_H.Items.Add("" + i);
                    }
                }

                if (i < 10)
                {
                    Cb_Start_M.Items.Add("0" + i);
                    Cb_Start_S.Items.Add("0" + i);
                    Cb_Stop_M.Items.Add("0" + i);
                    Cb_Stop_S.Items.Add("0" + i);
                }
                else
                {
                    Cb_Start_M.Items.Add("" + i);
                    Cb_Start_S.Items.Add("" + i);
                    Cb_Stop_M.Items.Add("" + i);
                    Cb_Stop_S.Items.Add("" + i);
                }
            }
        }

        private void Add_Btn()
        {
            for (int i = 0; i < Btn_MainMP3.Length; i++)
            {
                Btn_MainMP3[i] = new Button();
                Btn_MainMP3[i].Width = 300;
                Btn_MainMP3[i].Height = 94;

                Btn_MainMP3[i].HorizontalAlignment = HorizontalAlignment.Left;
                Btn_MainMP3[i].VerticalAlignment = VerticalAlignment.Top;
                Btn_MainMP3[i].Name = "Btn_MainMP3" + (i + 1);
                Btn_MainMP3[i].Content = "" + (i + 1);
                Btn_MainMP3[i].Background = Brushes.LightGray;
                Btn_MainMP3[i].BorderBrush = Brushes.Black;
                Btn_MainMP3[i].Click += new RoutedEventHandler(Btn_MainMP3_Click);
                WPanel_MP3.Children.Add(Btn_MainMP3[i]);

                Btn_ScheduleMP3[i] = new Button();
                Btn_ScheduleMP3[i].Width = 200;
                Btn_ScheduleMP3[i].Height = 50;

                Btn_ScheduleMP3[i].HorizontalAlignment = HorizontalAlignment.Left;
                Btn_ScheduleMP3[i].VerticalAlignment = VerticalAlignment.Top;
                Btn_ScheduleMP3[i].Name = "Btn_ScheduleMP3" + (i + 1);
                Btn_ScheduleMP3[i].Content = "" + (i + 1);
                Btn_ScheduleMP3[i].Background = Brushes.LightGray;
                Btn_ScheduleMP3[i].BorderBrush = Brushes.Black;
                Btn_ScheduleMP3[i].Click += new RoutedEventHandler(Btn_ScheduleMP3_Click);
                WPanel_ScheduleMP3.Children.Add(Btn_ScheduleMP3[i]);
            }

            for (int i = 0; i < Btn_MainGR.Length; i++)
            {
                Btn_MainGR[i] = new Button();
                Btn_MainGR[i].Width = 140;
                Btn_MainGR[i].Height = 140;

                Btn_MainGR[i].HorizontalAlignment = HorizontalAlignment.Left;
                Btn_MainGR[i].VerticalAlignment = VerticalAlignment.Top;
                Btn_MainGR[i].Name = "Btn_MainGR" + (i + 1);
                Btn_MainGR[i].Content = "" + i;
                Btn_MainGR[0].Content = "그룹전체";
                Btn_MainGR[i].Background = Brushes.LightGray;
                Btn_MainGR[i].BorderBrush = Brushes.Black;
                Btn_MainGR[i].Click += new RoutedEventHandler(Btn_MainGR_Click);
                WPanel_GR.Children.Add(Btn_MainGR[i]);

                Btn_ScheduleGR[i] = new Button();
                Btn_ScheduleGR[i].Width = 200;
                Btn_ScheduleGR[i].Height = 50;

                Btn_ScheduleGR[i].HorizontalAlignment = HorizontalAlignment.Left;
                Btn_ScheduleGR[i].VerticalAlignment = VerticalAlignment.Top;
                Btn_ScheduleGR[i].Name = "Btn_ScheduleGR" + (i + 1);
                Btn_ScheduleGR[i].Content = "" + i;
                Btn_ScheduleGR[0].Content = "그룹전체";
                Btn_ScheduleGR[i].Background = Brushes.LightGray;
                Btn_ScheduleGR[i].BorderBrush = Brushes.Black;
                Btn_ScheduleGR[i].Click += new RoutedEventHandler(Btn_ScheduleGR_Click);
                WPanel_ScheduleGr.Children.Add(Btn_ScheduleGR[i]);
            }

            for (int i = 0; i < Btn_SetGR.Length; i++)
            {
                Btn_SetGR[i] = new Button();
                Btn_SetGR[i].Width = 140;
                Btn_SetGR[i].Height = 140;

                Btn_SetGR[i].HorizontalAlignment = HorizontalAlignment.Left;
                Btn_SetGR[i].VerticalAlignment = VerticalAlignment.Top;
                //Btn_MainGR.Margin = new Thickness(iPosX, iPosY, 0, 0);
                Btn_SetGR[i].Name = "Btn_SetGR" + (i + 1);
                Btn_SetGR[i].Content = "" + (i + 1);
                Btn_SetGR[i].Background = Brushes.LightGray;
                Btn_SetGR[i].BorderBrush = Brushes.Black;
                Btn_SetGR[i].Click += new RoutedEventHandler(Btn_SetGR_Click);
                WPanel_SetGR.Children.Add(Btn_SetGR[i]);
            }

            for (int i = 0; i < Btn_DeviceList.Length; i++)
            {
                /*
                Btn_DeviceList[i] = new Button();
                Btn_DeviceList[i].Width = 140;
                Btn_DeviceList[i].Height = 95;

                Btn_DeviceList[i].HorizontalAlignment = HorizontalAlignment.Left;
                Btn_DeviceList[i].VerticalAlignment = VerticalAlignment.Top;
                //Btn_MainGR.Margin = new Thickness(iPosX, iPosY, 0, 0);
                Btn_DeviceList[i].Name = "Btn_DeviceList" + (i + 1);
                Btn_DeviceList[i].Content = "" + (i + 1);
                Btn_DeviceList[i].BorderBrush = Brushes.Black;
                Btn_DeviceList[i].Click += new RoutedEventHandler(Btn_DeviceList_Click);
                */
                Btn_DeviceList[i] = new Device_Button(i);

                WPanel_DList.Children.Add(Btn_DeviceList[i]);
            }

            for (int i = 0; i < Btn_ScheduleDay.Length; i++)
            {
                Btn_ScheduleDay[i] = new Button();
                Btn_ScheduleDay[i].Width = 50;
                Btn_ScheduleDay[i].Height = 50;

                Btn_ScheduleDay[i].HorizontalAlignment = HorizontalAlignment.Left;
                Btn_ScheduleDay[i].VerticalAlignment = VerticalAlignment.Top;
                //Btn_MainGR.Margin = new Thickness(iPosX, iPosY, 0, 0);
                Btn_ScheduleDay[i].Name = "Btn_ScheduleDay" + (i + 1);
                Btn_ScheduleDay[i].Background = Brushes.LightGray;
                Btn_ScheduleDay[i].BorderBrush = Brushes.Black;
                Btn_ScheduleDay[i].Click += new RoutedEventHandler(Btn_ScheduleDay_Click);
                SPanel_Day.Children.Add(Btn_ScheduleDay[i]);
            }
            Btn_ScheduleDay[0].Content = "월";
            Btn_ScheduleDay[1].Content = "화";
            Btn_ScheduleDay[2].Content = "수";
            Btn_ScheduleDay[3].Content = "목";
            Btn_ScheduleDay[4].Content = "금";
            Btn_ScheduleDay[5].Content = "토";
            Btn_ScheduleDay[6].Content = "일";
        }

        private void Init_Devices()
        {
            //Devices null값 오류 방지를 위해 kind 3으로 초기화(3은 등록되지 않은 상태임.)
            for (int i = 0; i < Btn_DeviceList.Length; i++)
            {
                deviceData.Devices[i] = new Device("", "", 3, "", "");
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Lb_Timer.Content = DateTime.Now.ToLongTimeString();
            Lb_Day.Content = DateTime.Now.DayOfWeek.ToString();
            Lb_Date.Content = DateTime.Now.ToShortDateString();

            string s_Url = "";
            string[] s_Parse;

            for (int i = 0; i < schedules.Count; i++)
            {
                if (schedules[i].StartTime == DateTime.Now.ToString("HH:mm:ss"))
                {
                    s_Parse = schedules[i].Day.Split('/');
                    for (int j = 0; j < s_Parse.Length; j++)
                    {
                        if (s_Parse[j] == "월") { s_Parse[j] = "Monday"; }
                        else if (s_Parse[j] == "화") { s_Parse[j] = "Tuesday"; }
                        else if (s_Parse[j] == "수") { s_Parse[j] = "Wednesday"; }
                        else if (s_Parse[j] == "목") { s_Parse[j] = "Thursday"; }
                        else if (s_Parse[j] == "금") { s_Parse[j] = "Friday"; }
                        else if (s_Parse[j] == "토") { s_Parse[j] = "Saturday"; }
                        else if (s_Parse[j] == "일") { s_Parse[j] = "Sunday"; }

                        if (DateTime.Now.DayOfWeek.ToString() == s_Parse[j]) { b_Day = true; }
                    }

                    if (b_Timer == false)
                    {
                        if (b_Day == true)
                        {
                            b_Timer = true;
                            i_TimerIndex = i;
                            for (int j = 0; j < Btn_ScheduleMP3.Length; j++)
                            {
                                if (schedules[i].MP3 == (string)Btn_ScheduleMP3[j].Content)
                                {
                                    s_Url = "http://root:pass@192.168.21.195/axis-cgi/mediaclip.cgi?action=play&clip=" + j;
                                    //SendUrl(s_Url); 
                                }
                            }
                        }
                    }
                    b_Day = false;
                }

                if (schedules[i].StopTime == DateTime.Now.ToString("HH:mm:ss"))
                {
                    s_Parse = schedules[i].Day.Split('/');
                    for (int j = 0; j < s_Parse.Length; j++)
                    {
                        if (s_Parse[j] == "월") { s_Parse[j] = "Monday"; }
                        else if (s_Parse[j] == "화") { s_Parse[j] = "Tuesday"; }
                        else if (s_Parse[j] == "수") { s_Parse[j] = "Wednesday"; }
                        else if (s_Parse[j] == "목") { s_Parse[j] = "Thursday"; }
                        else if (s_Parse[j] == "금") { s_Parse[j] = "Friday"; }
                        else if (s_Parse[j] == "토") { s_Parse[j] = "Saturday"; }
                        else if (s_Parse[j] == "일") { s_Parse[j] = "Sunday"; }

                        if (DateTime.Now.DayOfWeek.ToString() == s_Parse[j]) { b_Day = true; }
                    }

                    if (b_Timer == true)
                    {
                        if (b_Day == true)
                        {
                            if (i_TimerIndex == i)
                            {
                                b_Timer = false;
                                s_Url = "http://root:pass@192.168.21.195/axis-cgi/mediaclip.cgi?action=stop";
                                //SendUrl(s_Url);
                            }
                        }
                    }
                    b_Day = false;
                }
            }
        }

        private void SendUrl_with_IDPW(string rrr, string id, string pw, string ip)
        {
            string url = rrr;
            string responseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            System.Net.NetworkCredential netCredential = new System.Net.NetworkCredential(id, pw, ip);
            request.Credentials = netCredential;

            //request.Timeout = 30 * 1000; // 30초
            //request.Headers.Add("Authorization", "BASIC SGVsbG8="); // 헤더 추가 방법

            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                Dispatcher.Invoke(() =>
                {
                    HttpStatusCode status = resp.StatusCode;
                    Console.WriteLine(status);  // 정상이면 "OK"
                    if (status == HttpStatusCode.OK)
                    {
                        Stream respStream = resp.GetResponseStream();
                        using (StreamReader sr = new StreamReader(respStream))
                        {
                            responseText = sr.ReadToEnd();
                        //    notification_window.Text += responseText;
                        }

                    }
                    else
                    {
                    }
                });
            }
        }

        private void SendUrl(string url, string id, string pw, string ip)
        {
            string s_url = "http://" + id + ":" + pw + "@" + ip + url;
            string responseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            System.Net.NetworkCredential netCredential = new System.Net.NetworkCredential(id, pw, ip);
            request.Credentials = netCredential;

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
            if (sData_Pasrs.Length == 3)
            {
                if (sData_Pasrs[1] == "stopping\r")
                {
                    //MessageBox.Show("ssss");
                }
            }
            else
            {
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
        }
        private void Btn_MainMP3_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
        }

        private void Btn_ScheduleMP3_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            for (int i = 0; i < Btn_ScheduleMP3.Length; i++)
            {
                if (Btn_ScheduleMP3[i] == btn)
                {
                    if (Btn_ScheduleMP3[i].Background == Brushes.LightGray)
                    {
                        Btn_ScheduleMP3[i].Background = Brushes.Red;
                    }
                    else if (Btn_ScheduleMP3[i].Background == Brushes.Red)
                    {
                        Btn_ScheduleMP3[i].Background = Brushes.LightGray;
                    }
                }
                else
                {
                    Btn_ScheduleMP3[i].Background = Brushes.LightGray;
                }
            }
        }

        private void Btn_MainGR_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            for (int i = 0; i < Btn_MainGR.Length; i++)
            {
                if (Btn_MainGR[i] == btn)
                {
                    if (i == 0)
                    {
                        for (int j = 1; j < Btn_MainGR.Length; j++)
                        {
                            Btn_MainGR[j].Background = Brushes.LightGray;
                        }

                        if (Btn_MainGR[i].Background == Brushes.LightGray)
                        {
                            Btn_MainGR[i].Background = Brushes.Red;
                        }
                        else if (Btn_MainGR[i].Background == Brushes.Red)
                        {
                            Btn_MainGR[i].Background = Brushes.LightGray;
                        }
                    }
                    else
                    {
                        Btn_MainGR[0].Background = Brushes.LightGray;

                        if (Btn_MainGR[i].Background == Brushes.LightGray)
                        {
                            Btn_MainGR[i].Background = Brushes.Red;
                            string[] pars;
                            if (deviceData.D_GrData[i - 1] != null)
                            {
                                pars = deviceData.D_GrData[i - 1].Split(',');
                                for (int j = 0; j < (pars.Length - 1); j++)
                                {
                                    Btn_DeviceList[Int32.Parse(pars[j])].BorderThickness = new Thickness(2);
                                    Btn_DeviceList[Int32.Parse(pars[j])].BorderBrush = Brushes.Red;
                                }
                            }
                        }
                        else if (Btn_MainGR[i].Background == Brushes.Red)
                        {
                            Btn_MainGR[i].Background = Brushes.LightGray;
                            string[] pars;
                            if (deviceData.D_GrData[i - 1] != null)
                            {
                                pars = deviceData.D_GrData[i - 1].Split(',');
                                for (int j = 0; j < (pars.Length - 1); j++)
                                {
                                    Btn_DeviceList[Int32.Parse(pars[j])].BorderThickness = new Thickness(1);
                                    Btn_DeviceList[Int32.Parse(pars[j])].BorderBrush = Brushes.Black;
                                }
                            }
                        }
                    }
                }
                else
                {

                }
            }
        }
        private void Btn_ScheduleDay_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            for (int i = 0; i < Btn_ScheduleDay.Length; i++)
            {
                if (Btn_ScheduleDay[i] == btn)
                {
                    if (Btn_ScheduleDay[i].Background == Brushes.Red)
                    {
                        Btn_ScheduleDay[i].Background = Brushes.LightGray;
                    }
                    else if (Btn_ScheduleDay[i].Background == Brushes.LightGray)
                    {
                        Btn_ScheduleDay[i].Background = Brushes.Red;
                    }
                }
            }
        }

        private void Btn_ScheduleGR_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            for (int i = 0; i < Btn_ScheduleGR.Length; i++)
            {
                if (Btn_ScheduleGR[i] == btn)
                {
                    if (i == 0)
                    {
                        for (int j = 1; j < Btn_ScheduleGR.Length; j++)
                        {
                            Btn_ScheduleGR[j].Background = Brushes.LightGray;
                        }

                        if (Btn_ScheduleGR[i].Background == Brushes.LightGray)
                        {
                            Btn_ScheduleGR[i].Background = Brushes.Red;
                        }
                        else if (Btn_ScheduleGR[i].Background == Brushes.Red)
                        {
                            Btn_ScheduleGR[i].Background = Brushes.LightGray;
                        }
                    }
                    else
                    {
                        Btn_ScheduleGR[0].Background = Brushes.LightGray;

                        if (Btn_ScheduleGR[i].Background == Brushes.LightGray)
                        {
                            Btn_ScheduleGR[i].Background = Brushes.Red;
                        }
                        else if (Btn_ScheduleGR[i].Background == Brushes.Red)
                        {
                            Btn_ScheduleGR[i].Background = Brushes.LightGray;
                        }
                    }
                }
                else
                {

                }
            }
        }
        private void Btn_SetGR_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            for (int i = 0; i < Btn_SetGR.Length; i++)
            {
                if (Btn_SetGR[i] == btn)
                {

                    if (Btn_SetGR[i].Background == Brushes.LightGray)
                    {
                        Btn_SetGR[i].Background = Brushes.Red;
                        Txt_GrName.Text = (string)Btn_SetGR[i].Content;

                        for (int j = 0; j < Btn_DeviceList.Length; j++)
                        {
                            Btn_DeviceList[j].BorderThickness = new Thickness(1);
                            Btn_DeviceList[j].BorderBrush = Brushes.Black;
                        }

                        if (deviceData.D_GrData[i] != null)
                        {
                            string[] pars;
                            pars = deviceData.D_GrData[i].Split(',');
                            for (int j = 0; j < (pars.Length - 1); j++)
                            {
                                Btn_DeviceList[Int32.Parse(pars[j])].BorderThickness = new Thickness(2);
                                Btn_DeviceList[Int32.Parse(pars[j])].BorderBrush = Brushes.Red;
                            }

                        }
                        else
                        {

                        }
                    }
                    else if (Btn_SetGR[i].Background == Brushes.Red)
                    {
                        Btn_SetGR[i].Background = Brushes.LightGray;
                    }
                }
                else
                {
                    Btn_SetGR[i].Background = Brushes.LightGray;
                }
            }
        }

        private void Btn_DeviceList_Click(object sender, EventArgs e)
        {
            //Button btn = sender as Button;

            //for (int i = 0; i < Btn_DeviceList.Length; i++)
            //{
            //    if (Btn_DeviceList[i] == btn)
            //    {
            //        if (Btn_DeviceList[i].BorderThickness == new Thickness(1))
            //        {
            //            Btn_DeviceList[i].BorderThickness = new Thickness(2);
            //            Btn_DeviceList[i].BorderBrush = Brushes.Red;
            //        }
            //        else if (Btn_DeviceList[i].BorderThickness == new Thickness(2))
            //        {
            //            Btn_DeviceList[i].BorderThickness = new Thickness(1);
            //            Btn_DeviceList[i].BorderBrush = Brushes.Black;
            //        }
            //    }
            //}
        }

        private void Btn_DRun_Click(object sender, RoutedEventArgs e)
        {
            Grid_DRun.Visibility = Visibility.Visible;
            Grid_DSchedule.Visibility = Visibility.Hidden;
            Grid_DSetting.Visibility = Visibility.Hidden;
            for (int i = 0; i < Btn_DeviceList.Length; i++)
            {
                Btn_DeviceList[i].BorderThickness = new Thickness(1);
                Btn_DeviceList[i].BorderBrush = Brushes.Black;
            }
            for (int i = 0; i < Btn_MainGR.Length; i++)
            {
                Btn_MainGR[i].Background = Brushes.LightGray;
            }
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
            for (int i = 0; i < Btn_DeviceList.Length; i++)
            {
                Btn_DeviceList[i].BorderThickness = new Thickness(1);
                Btn_DeviceList[i].BorderBrush = Brushes.Black;
            }
            for (int i = 0; i < Btn_SetGR.Length; i++)
            {
                //Btn_SetGR[i].BorderThickness = new Thickness(1);
                //Btn_SetGR[i].BorderBrush = Brushes.Black;
                Btn_SetGR[i].Background = Brushes.LightGray;
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            //string url = "http://root:pass@192.168.21.195/axis-cgi/mediaclip.cgi?action=play&clip=0";
            //string url = "http://root:pass@192.168.21.195/axis-cgi/mediaclip.cgi?action=stop";

            this.Close();

        }


        private void Btn_GrSet_Click(object sender, RoutedEventArgs e)
        {
            bool SelGr = false;

            for (int i = 0; i < Btn_SetGR.Length; i++)
            {
                if (Btn_SetGR[i].Background == Brushes.Red)
                {
                    Btn_SetGR[i].Content = Txt_GrName.Text;
                    SelGr = true;

                    for (int j = 0; j < Btn_DeviceList.Length; j++)
                    {
                        if (Btn_DeviceList[j].BorderThickness == new Thickness(2))
                        {
                            deviceData.D_GrData[i] += j + ",";
                        }


                    }
                }


            }

            if (SelGr == false)
            {
                MessageBox.Show("설정할 그룹채널을 선택해주세요!");
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            string s_Name = "";
            string s_Day = "";
            string s_StartT = "";
            string s_StopT = "";
            string s_Mp3 = "";
            string s_Group = "";
            bool b_Name = false;
            bool b_Time = false;

            s_Name = Txt_ScheduleName.Text;
            s_StartT = Cb_Start_H.Text + ":" + Cb_Start_M.Text + ":" + Cb_Start_S.Text;
            s_StopT = Cb_Stop_H.Text + ":" + Cb_Stop_M.Text + ":" + Cb_Stop_S.Text;

            for (int i = 0; i < Btn_ScheduleDay.Length; i++)
            {
                if (Btn_ScheduleDay[i].Background == Brushes.Red)
                {
                    s_Day += (string)Btn_ScheduleDay[i].Content + "/";
                }

                if (i == (Btn_ScheduleDay.Length - 1))
                {
                    if (s_Day.Length != 0)
                    {
                        s_Day = s_Day.Substring(0, s_Day.Length - 1);
                    }
                }
            }
            for (int i = 0; i < Btn_ScheduleMP3.Length; i++)
            {
                if (Btn_ScheduleMP3[i].Background == Brushes.Red)
                {
                    s_Mp3 = (string)Btn_ScheduleMP3[i].Content;
                }
            }
            for (int i = 0; i < Btn_ScheduleGR.Length; i++)
            {
                if (Btn_ScheduleGR[i].Background == Brushes.Red)
                {
                    s_Group += (string)Btn_ScheduleGR[i].Content + "/";
                }

                if (i == (Btn_ScheduleGR.Length - 1))
                {
                    if (s_Group.Length != 0)
                    {
                        s_Group = s_Group.Substring(0, s_Group.Length - 1);
                    }
                }
            }
            for (int i = 0; i < schedules.Count; i++)
            {
                if (schedules[i].Name == s_Name)
                    b_Name = true;

                if (schedules[i].StartTime == s_StartT)
                    b_Time = true;
                else if (schedules[i].StopTime == s_StartT)
                    b_Time = true;
            }

            if (String.IsNullOrWhiteSpace(Txt_ScheduleName.Text))
            {
                MessageBox.Show("예약명을 입력해주세요!");
            }
            else if (b_Name == true)
            {
                MessageBox.Show("예약명이 중복입니다!");
            }
            else
            {
                if (s_Day == "")
                {
                    MessageBox.Show("요일을 선택해주세요!");
                }
                else
                {
                    if (s_StartT == s_StopT)
                    {
                        MessageBox.Show("시작시간과 종료시간이 같습니다!");
                    }
                    else if (b_Time == true)
                    {
                        MessageBox.Show("예약시간이 중복입니다!");
                    }
                    else
                    {
                        if (s_Mp3 == "")
                        {
                            MessageBox.Show("음원을 선택해주세요!");
                        }
                        else
                        {

                            if (s_Group == "")
                            {
                                MessageBox.Show("그룹을 선택해주세요!");
                            }
                            else
                            {
                                schedules.Add(new Schedule()
                                {
                                    Number = "" + (LV_Schedule.Items.Count + 1),
                                    Name = s_Name,
                                    Day = s_Day,
                                    StartTime = s_StartT,
                                    StopTime = s_StopT,
                                    MP3 = s_Mp3,
                                    Group = s_Group
                                });

                                LV_Schedule.ItemsSource = schedules;
                                LV_Schedule.Items.Refresh();
                            }
                        }
                    }
                }
            }
        }

        private void Btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            if (LV_Schedule.SelectedIndex != -1)
            {
                string s_Name = "";
                string s_Day = "";
                string s_StartT = "";
                string s_StopT = "";
                string s_Mp3 = "";
                string s_Group = "";
                bool b_Name = false;
                bool b_Time = false;

                s_Name = Txt_ScheduleName.Text;
                s_StartT = Cb_Start_H.Text + ":" + Cb_Start_M.Text + ":" + Cb_Start_S.Text;
                s_StopT = Cb_Stop_H.Text + ":" + Cb_Stop_M.Text + ":" + Cb_Stop_S.Text;

                for (int i = 0; i < Btn_ScheduleDay.Length; i++)
                {
                    if (Btn_ScheduleDay[i].Background == Brushes.Red)
                    {
                        s_Day += (string)Btn_ScheduleDay[i].Content + "/";
                    }

                    if (i == (Btn_ScheduleDay.Length - 1))
                    {
                        if (s_Day.Length != 0)
                        {
                            s_Day = s_Day.Substring(0, s_Day.Length - 1);
                        }
                    }
                }
                for (int i = 0; i < Btn_ScheduleMP3.Length; i++)
                {
                    if (Btn_ScheduleMP3[i].Background == Brushes.Red)
                    {
                        s_Mp3 = (string)Btn_ScheduleMP3[i].Content;
                    }
                }
                for (int i = 0; i < Btn_ScheduleGR.Length; i++)
                {
                    if (Btn_ScheduleGR[i].Background == Brushes.Red)
                    {
                        s_Group += (string)Btn_ScheduleGR[i].Content + "/";
                    }

                    if (i == (Btn_ScheduleGR.Length - 1))
                    {
                        if (s_Group.Length != 0)
                        {
                            s_Group = s_Group.Substring(0, s_Group.Length - 1);
                        }
                    }
                }
                for (int i = 0; i < schedules.Count; i++)
                {
                    if (schedules[i].Name == s_Name)
                    {
                        if (i != LV_Schedule.SelectedIndex)
                            b_Name = true;
                    }
                    if (schedules[i].StartTime == s_StartT)
                    {
                        if (i != LV_Schedule.SelectedIndex)
                            b_Time = true;
                    }
                    else if (schedules[i].StopTime == s_StartT)
                    {
                        if (i != LV_Schedule.SelectedIndex)
                            b_Time = true;
                    }

                }

                if (String.IsNullOrWhiteSpace(Txt_ScheduleName.Text))
                {
                    MessageBox.Show("예약명을 입력해주세요!");
                }
                else if (b_Name == true)
                {
                    MessageBox.Show("예약명이 중복입니다!");
                }
                else
                {
                    if (s_Day == "")
                    {
                        MessageBox.Show("요일을 선택해주세요!");
                    }
                    else
                    {
                        if (s_StartT == s_StopT)
                        {
                            MessageBox.Show("시작시간과 종료시간이 같습니다!");
                        }
                        else if (b_Time == true)
                        {
                            MessageBox.Show("예약시간이 중복입니다!");
                        }
                        else
                        {
                            if (s_Mp3 == "")
                            {
                                MessageBox.Show("음원을 선택해주세요!");
                            }
                            else
                            {

                                if (s_Group == "")
                                {
                                    MessageBox.Show("그룹을 선택해주세요!");
                                }
                                else
                                {
                                    schedules[LV_Schedule.SelectedIndex].Name = s_Name;
                                    schedules[LV_Schedule.SelectedIndex].StartTime = s_StartT;
                                    schedules[LV_Schedule.SelectedIndex].StopTime = s_StopT;
                                    schedules[LV_Schedule.SelectedIndex].Day = s_Day;
                                    schedules[LV_Schedule.SelectedIndex].MP3 = s_Mp3;
                                    schedules[LV_Schedule.SelectedIndex].Group = s_Group;
                                    LV_Schedule.Items.Refresh();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("수정할 예약목록을 선택해주세요!");
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            foreach (Schedule item in LV_Schedule.SelectedItems)
            {
                schedules.Remove(item);

                for (int i = 0; i < schedules.Count; i++)
                {
                    schedules[i].Number = "" + (i + 1);
                }
            }
            for (int i = 0; i < Btn_ScheduleGR.Length; i++)
            {
                Btn_ScheduleGR[i].Background = Brushes.LightGray;
            }
            for (int i = 0; i < Btn_ScheduleMP3.Length; i++)
            {
                Btn_ScheduleMP3[i].Background = Brushes.LightGray;
            }
            for (int i = 0; i < Btn_ScheduleDay.Length; i++)
            {
                Btn_ScheduleDay[i].Background = Brushes.LightGray;
            }
            Txt_ScheduleName.Text = "";
            Cb_Start_H.SelectedIndex = 0;
            Cb_Start_M.SelectedIndex = 0;
            Cb_Start_S.SelectedIndex = 0;


            LV_Schedule.Items.Refresh();
        }

        private void Cb_Start_H_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cb_Stop_H.SelectedIndex = Cb_Start_H.SelectedIndex;
            //Cb_Start_M.SelectedIndex = 0;
            //Cb_Start_S.SelectedIndex = 0;
        }

        private void Cb_Start_M_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cb_Stop_M.SelectedIndex = Cb_Start_M.SelectedIndex;
            /*if (Cb_Start_M.SelectedIndex == 59)
            {
                if (Cb_Stop_H.SelectedIndex == 23)
                    Cb_Stop_H.SelectedIndex = 0;
                else
                    Cb_Stop_H.SelectedIndex = (Cb_Stop_H.SelectedIndex + 1);
                Cb_Stop_M.SelectedIndex = 0;
            }
            else
            {
                Cb_Stop_H.SelectedIndex = Cb_Start_H.SelectedIndex;
                Cb_Stop_M.SelectedIndex = (Cb_Start_M.SelectedIndex + 1);
            }*/
        }

        private void Cb_Start_S_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cb_Stop_S.SelectedIndex = Cb_Start_S.SelectedIndex;
        }

        private void Cb_Stop_H_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cb_Start_H.SelectedIndex == 23)
            {

            }
            else
            {
                if (Cb_Stop_H.SelectedIndex < Cb_Start_H.SelectedIndex)
                {
                    //MessageBox.Show("종료시간을 올바르게 선택해주세요!");
                    //Cb_Stop_H.SelectedIndex = Cb_Start_H.SelectedIndex;
                }
            }
        }

        private void Cb_Stop_M_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cb_Start_M.SelectedIndex == 59)
            {

            }
            else
            {
                if (Cb_Stop_H.SelectedIndex == Cb_Start_H.SelectedIndex)
                {
                    if (Cb_Stop_M.SelectedIndex < Cb_Start_M.SelectedIndex)
                    {
                        //MessageBox.Show("종료시간을 올바르게 선택해주세요!");
                        //Cb_Stop_M.SelectedIndex = (Cb_Start_M.SelectedIndex+1);
                    }
                }
            }
        }

        private void Cb_Stop_S_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cb_Stop_M.SelectedIndex == Cb_Start_M.SelectedIndex)
            {
                if (Cb_Stop_S.SelectedIndex < Cb_Start_S.SelectedIndex)
                {
                    //MessageBox.Show("종료시간을 올바르게 선택해주세요!");
                    //Cb_Stop_S.SelectedIndex = Cb_Start_S.SelectedIndex;
                }
            }
        }

        private void LV_Schedule_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Schedule.SelectedItems.Count == 1)
            {
                string[] s_Parse;
                Txt_ScheduleName.Text = schedules[LV_Schedule.SelectedIndex].Name;

                s_Parse = schedules[LV_Schedule.SelectedIndex].StartTime.ToString().Split(':');
                Cb_Start_H.Text = s_Parse[0];
                Cb_Start_M.Text = s_Parse[1];
                Cb_Start_S.Text = s_Parse[2];
                s_Parse = schedules[LV_Schedule.SelectedIndex].StopTime.ToString().Split(':');
                Cb_Stop_H.Text = s_Parse[0];
                Cb_Stop_M.Text = s_Parse[1];
                Cb_Stop_S.Text = s_Parse[2];

                for (int i = 0; i < Btn_ScheduleMP3.Length; i++)
                {
                    if (schedules[LV_Schedule.SelectedIndex].MP3 == (string)Btn_ScheduleMP3[i].Content)
                    {
                        Btn_ScheduleMP3[i].Background = Brushes.Red;
                    }
                    else
                    {
                        Btn_ScheduleMP3[i].Background = Brushes.LightGray;
                    }

                }
                s_Parse = schedules[LV_Schedule.SelectedIndex].Group.ToString().Split('/');
                for (int i = 0; i < Btn_ScheduleGR.Length; i++)
                {
                    Btn_ScheduleGR[i].Background = Brushes.LightGray;
                }
                for (int i = 0; i < Btn_ScheduleGR.Length; i++)
                {
                    for (int j = 0; j < s_Parse.Length; j++)
                    {
                        if ((string)Btn_ScheduleGR[i].Content == s_Parse[j])
                        {
                            Btn_ScheduleGR[i].Background = Brushes.Red;
                        }
                    }
                }
                s_Parse = schedules[LV_Schedule.SelectedIndex].Day.ToString().Split('/');
                for (int i = 0; i < Btn_ScheduleDay.Length; i++)
                {
                    Btn_ScheduleDay[i].Background = Brushes.LightGray;
                }
                for (int i = 0; i < Btn_ScheduleDay.Length; i++)
                {
                    for (int j = 0; j < s_Parse.Length; j++)
                    {
                        if ((string)Btn_ScheduleDay[i].Content == s_Parse[j])
                        {
                            Btn_ScheduleDay[i].Background = Brushes.Red;
                        }
                    }
                }
            }
        }


        //스레드로 연속 재생
        public Thread StartThread(string url, string id, string pw, string ip)
        {
            var t = new Thread(() => SendUrl(url, id, pw, ip));
            t.Start();
            return t;
        }
        private void Update_Device_Button()
        {
            //Devices 배열 기반으로 Button들 업데이트 하는 Thread

            while (true)
            {
                 Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < Btn_DeviceList.Length; i++)
                    {
                        if (deviceData.Devices[i] != null)
                        {
                            MaterialDesignThemes.Wpf.PackIcon packIcon = new MaterialDesignThemes.Wpf.PackIcon();
                            switch (deviceData.Devices[i].Kind)
                            {
                                case (0): Btn_DeviceList[i].device_icon.Kind = PackIconKind.Speaker; break;
                                case (1): Btn_DeviceList[i].device_icon.Kind = PackIconKind.Cctv; break;
                                case (2): Btn_DeviceList[i].device_icon.Kind = PackIconKind.MobileDevices; break;
                                case (3): Btn_DeviceList[i].device_icon.Kind = PackIconKind.HelpCircleOutline; break;
                            }

                            //디바이스가 저장되어있음(등록 되어있음)
                            if (deviceData.Devices[i].IP != "")
                            {
                                if (!Ping_(deviceData.Devices[i].IP))
                                {
                                    //디바이스가 연결 안되어있음
                                    Btn_DeviceList[i].led_.color.Fill = Brushes.Orange;
                                }
                                else
                                {
                                    //디바이스가 연결 되어있음
                                    Btn_DeviceList[i].led_.color.Fill = Brushes.LightGreen;
                                }
                            }
                            else
                            {
                                Btn_DeviceList[i].led_.color.Fill = Brushes.Gray;
                            }

                            Btn_DeviceList[i].device_name_box.Text = deviceData.Devices[i].Name;

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
    public class Schedule
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public string MP3 { get; set; }
        public string Group { get; set; }
    }
}
