using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Labk2
{
    public partial class Form1 : Form
    {
        private UdpClient udpListener;
        private Thread listenThread;
        private System.Windows.Forms.Timer timer;
        private string currentTime = "00:00:00";

        public Form1()
        {
            InitializeComponent();
            InitializeUdpListener();
            InitializeTimer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void InitializeUdpListener()
        {
            udpListener = new UdpClient(12345);

            listenThread = new Thread(new ThreadStart(ListenForData));
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTimeOnForm(currentTime);
            DateTime dt = DateTime.ParseExact(currentTime, "HH:mm:ss", null);
            dt = dt.AddSeconds(1);
            currentTime = dt.ToString("HH:mm:ss");
        }

        private void ListenForData()
        {
            while (true)
            {
                try
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 12345);
                    byte[] data = udpListener.Receive(ref endPoint);
                    string message = Encoding.UTF8.GetString(data);

                    if (message.StartsWith("get time:"))
                    {
                        currentTime = message.Substring("get time:".Length);
                        UpdateTimeOnForm(currentTime);
                    }
                    else
                    {
                        Console.WriteLine("Неизвестна команда: " + message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка при обробці даних: " + ex.Message);
                }
            }
        }

        private void UpdateTimeOnForm(string time)
        {
            if (labelTime.InvokeRequired)
            {
                labelTime.Invoke(new Action(() => labelTime.Text = "Час: " + time));
            }
            else
            {
                labelTime.Text = "Час: " + time;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            udpListener.Close();
            listenThread.Abort();
        }
    }
}
