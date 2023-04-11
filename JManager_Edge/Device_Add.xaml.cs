using MaterialDesignColors.Recommended;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
    /// Device_Add.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Device_Add : Window
    {
        Device_Data deviceData = Device_Data.instance;
        int device_num;

        public Device_Add(int device_num)
        {
            InitializeComponent();
            this.device_num = device_num;
        }

        private void Ping_Test(object sender, RoutedEventArgs e)
        {
            //핑 테스트

            if (deviceData.ValidateIPv4(ip_address_.Text))
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
                string address = ip_address_.Text;

                for (int times = 0; times < 4; times++)
                {
                      PingReply reply = pingSender.Send(address, timeout, buffer, options); 
                    string str = string.Empty;
                    if (reply.Status == IPStatus.Success)
                    {
                        str += "Address: " + reply.Address.ToString() + "\n";   // 주소
                        str += "RoundTrip time: " + reply.RoundtripTime + "\n";   // 응답시간
                        str += "Time to live: " + reply.Options.Ttl + "\n";   // TTL : 데이터의 유효 기간
                        str += "Don't fragment: " + reply.Options.DontFragment + "\n";   // 손실데이터 유무
                        str += "Buffer size: " + reply.Buffer.Length + "\n\n";   // 버퍼사이즈
                        notification_window.Text += str;
                    }
                    else
                    {
                        str = reply.Status.ToString() + "\n\n";
                        notification_window.Text += str;
                    }
                    Delay(1000);
                }
            }
            else
            {
                MessageBox.Show("IP주소를 정확하게 입력해주세요.");
            }
        }
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }

        private void verify_login(object sender, RoutedEventArgs e)
        {
            //해당 아이디 비밀번호로 IP에 로그인 할 수 있는지 확인
            if (device_ID.Text != String.Empty)
            {
                if (device_PW.Password != String.Empty)
                {
                    if (deviceData.ValidateIPv4(ip_address_.Text))
                    {
                        if (deviceData.Devices[this.device_num].Kind == 0)//스피커 일때
                        {
                            SendUrl_with_IDPW(device_ID.Text, device_PW.Password, ip_address_.Text);
                        }
                        else if (deviceData.Devices[this.device_num].Kind == 1)//카메라 일때
                        {
                            SendUrl_with_IDPW_(device_ID.Text, device_PW.Password, ip_address_.Text);
                        }
                    }
                    else
                    {
                        MessageBox.Show("장치 IP주소를 정확하게 입력해주세요.");
                    }
                }
                else
                {
                    MessageBox.Show("장치 비밀번호(PW)를 정확하게 입력해주세요.");
                }
            }
            else
            {
                MessageBox.Show("장치 아이디(ID)를 정확하게 입력해주세요.");
            }

        }
        private void SendUrl_with_IDPW(string id, string pw, string ip)
        {
            string url = "http://" + ip + "/axis-cgi/param.cgi?action=list";
            string responseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            System.Net.NetworkCredential netCredential = new System.Net.NetworkCredential(id, pw, ip);
            request.Credentials = netCredential;

            //request.Timeout = 30 * 1000; // 30초
            //request.Headers.Add("Authorization", "BASIC SGVsbG8="); // 헤더 추가 방법

            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                {
                    Dispatcher.Invoke(() =>
                    {
                        HttpStatusCode status = resp.StatusCode;
                        Console.WriteLine(status);  // 정상이면 "OK"
                        notification_window.Text += status;
                        if (status == HttpStatusCode.OK)
                        {
                            //Stream respStream = resp.GetResponseStream();
                            //using (StreamReader sr = new StreamReader(respStream))
                            //{
                            //    responseText = sr.ReadToEnd();
                            //    notification_window.Text += responseText;
                            //}
                            verify_led.color.Fill = Brushes.Green;
                            notification_window.Text += "연결에 성공했습니다.\n";
                        }
                        else
                        {
                            notification_window.Text += "오류가 발생했습니다.\n";
                            notification_window.Text += status.ToString();
                        }
                    });                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                }
            }
            catch(Exception ex)
            {
                notification_window.Text += ex.Message +"\n";
                notification_window.Text += "입력하신 아이디, 비밀번호를 확인해주세요\n";
            }
        }
        private void SendUrl_with_IDPW_(string id, string pw, string ip)
        {
            string url = "http://" + ip + "";
            string responseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            System.Net.NetworkCredential netCredential = new System.Net.NetworkCredential(id, pw, ip);
            request.Credentials = netCredential;

            //request.Timeout = 30 * 1000; // 30초
            //request.Headers.Add("Authorization", "BASIC SGVsbG8="); // 헤더 추가 방법

            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                {
                    Dispatcher.Invoke(() =>
                    {
                        HttpStatusCode status = resp.StatusCode;
                        Console.WriteLine(status);  // 정상이면 "OK"
                        notification_window.Text += status;
                        if (status == HttpStatusCode.OK)
                        {
                            //Stream respStream = resp.GetResponseStream();
                            //using (StreamReader sr = new StreamReader(respStream))
                            //{
                            //    responseText = sr.ReadToEnd();
                            //    notification_window.Text += responseText;
                            //}
                            verify_led.color.Fill = Brushes.Green;
                            notification_window.Text += "연결에 성공했습니다.\n";
                        }
                        else
                        {
                            notification_window.Text += "오류가 발생했습니다.\n";
                            notification_window.Text += status.ToString();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                notification_window.Text += ex.Message + "\n";
                notification_window.Text += "입력하신 아이디, 비밀번호를 확인해주세요\n";
            }
        }

        private void Clear_Btn(object sender, RoutedEventArgs e)
        {
            notification_window.Clear();
        }

        private void Add_Btn(object sender, RoutedEventArgs e)
        {
            //IP주소 값이 정확하게 적혔는지 확인
            if (deviceData.ValidateIPv4(ip_address_.Text))
            {
                //이름이 정확하게 적혔는지 확인
                if (name_.Text != String.Empty)
                {
                    //종류가 선택되었는지 확인
                    if (kind_.SelectedIndex != -1)
                    {
                        if(device_ID.Text != String.Empty || kind_.SelectedIndex != 0)
                        {
                            if(device_PW.Password != String.Empty || kind_.SelectedIndex != 0)
                            {
                                if(verify_led.color.Fill != Brushes.Red || kind_.SelectedIndex != 0)
                                {
                                    if (deviceData.is_exist(ip_address_.Text))
                                    {
                                        MessageBox.Show("같은 장치가 등록되어있습니다. \n 장치는 한번만 등록할 수 있습니다.");
                                    }
                                    else
                                    {
                                        MessageBoxResult messageboxresult = MessageBox.Show("다음과 같은 내용으로 새로운 장치를 등록합니다.\n" + "IP주소 :" + ip_address_.Text + "\n장치 이름 :" + name_.Text + "\n장치 종류 :" + kind_.SelectionBoxItem, "저장", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                                        if (messageboxresult == MessageBoxResult.OK)
                                        {
                                            Device new_device = new Device(ip_address_.Text, name_.Text, kind_.SelectedIndex, device_ID.Text, device_PW.Password);
                                            deviceData.Devices[device_num] = new_device;
                                            this.Close();
                                        }

                                    }
                                }
                                else
                                {
                                    MessageBox.Show("아이디 비밀번호 검증을 통해 로그인을 확인해주세요.");
                                }

                            }
                            else
                            {

                                MessageBox.Show("PW를 입력해주세요.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("ID를 입력해주세요.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("장치 종류를 선택해주세요.");
                    }
                }
                else
                {
                    MessageBox.Show("장치 이름을 정확하게 입력해주세요.");
                }
            }
            else
            {
                MessageBox.Show("IP주소를 정확하게 입력해주세요.");
            }
        }

        private void Cancel_Btn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void kind__SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(kind_ != null)
            {
                if(kind_.SelectedIndex != 0)
                {
                    //스피커가 아니면
                    IDPW_Grid.IsEnabled = false;
                }
                else
                {
                    IDPW_Grid.IsEnabled = true;
                }
            }
        }

        private void ip_address__TextChanged(object sender, TextChangedEventArgs e)
        {
            //검증상태일때,
            if (verify_led != null)
            {
                if (verify_led.color.Fill == Brushes.Green)
                {
                    verify_led.color.Fill = Brushes.Red;
                }
            }
        }

        private void device_PW_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //검증상태일때,
            if (verify_led != null)
            {
                if (verify_led.color.Fill == Brushes.Green)
                {
                    verify_led.color.Fill = Brushes.Red;
                }
            }

        }
    }
}
