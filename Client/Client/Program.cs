using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Labk2Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverIP = "127.0.0.1";
            int serverPort = 12345;

            using (UdpClient udpClient = new UdpClient())
            {
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

                while (true)
                {
                    Console.WriteLine("Введіть команду:");
                    Console.WriteLine("1. Отримати час");
                    Console.WriteLine("2. Вийти");
                    Console.Write("Введіть номер команди: ");

                    string userInput = Console.ReadLine();

                    if (userInput == "2")
                    {
                        break;
                    }
                    else if (userInput == "1")
                    {
                        string currentTime = DateTime.Now.ToString("HH:mm:ss");

                        string message = "get time:" + currentTime;
                        byte[] data = Encoding.UTF8.GetBytes(message);

                        try
                        {
                            udpClient.Send(data, data.Length, serverEndPoint);
                            Console.WriteLine("Час відправлено на сервер: " + currentTime);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Помилка при відправці часу на сервер: " + e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невідома команда. Виберіть 1 або 2.");
                    }
                }
            }
        }
    }
}
