using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSocketServerInstanceModeClient
{
    class Program
    {
        static Socket socketClient { get; set; }
        static void Main(string[] args)
        {
            #region socket创建实例 这里把发送和接收给分离了

            socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(ip, 5180);
            //进行连接
            socketClient.Connect(point);

            #region 不停的接收服务器端发送的消息
            Thread thread = new Thread(Recive);
            thread.IsBackground = true;
            thread.Start();
            #endregion


            #region 不停的给服务器发送数据
            Thread thread2 = new Thread(Send);
            thread2.IsBackground = true;
            thread2.Start();
            #endregion

            #endregion

            #region Tcp创建实例 测试SuperSocketServerInstanceMode 因为实例服务例子里在连接时写入了数据 
            //TCPConnect("127.0.0.1", 5180);
            #endregion 
            Console.ReadKey();
        }

        /// <summary>
        /// 接收消息
        /// </summary> 
        static void Recive()
        { 
            while (true)
            {
                //获取发送过来的消息
                byte[] buffer = new byte[1024 * 1024 * 2];
                var effective = socketClient.Receive(buffer);
                if (effective == 0)
                {
                    break;
                }
                var str = Encoding.Default.GetString(buffer, 0, effective);
                Console.WriteLine("服务器 --- " + str);
                Thread.Sleep(2000);
            }
        }


        static void Send()
        {
            int i = 0;
            int param1 = 0;
            int param2 = 0;
            while (true)
            {
                i++;
                param1 = i + 1;
                param2 = i + 2;
                Console.WriteLine($"Send  i:{i}  param1:{param1} param2:{param2}");
                string msg = $"SocketCommand {param1} {param2}" + "\r\n";
                Console.WriteLine($"msg:{msg}");
                var buffter = Encoding.Default.GetBytes(msg);
                var temp = socketClient.Send(buffter);
                Console.WriteLine($"Send  发送的字节数:{temp} "); 
                Thread.Sleep(1000);
            }

        }
        static void TCPConnect(String server, Int32 port)
        {
            string message = $"ADD kiba518 518" + "\r\n";
            try
            { 
                TcpClient client = new TcpClient();
                client.Connect(server, port); 
                Byte[] data = System.Text.Encoding.Default.GetBytes(message); 
                String responseData = String.Empty; 
                NetworkStream stream = client.GetStream(); 
                byte[] buffer = new byte[1024 * 1024 * 2];
                Int32 bytes = stream.Read(buffer, 0, buffer.Length);
                responseData = System.Text.Encoding.Default.GetString(buffer, 0, bytes);
                Console.WriteLine("接收服务器在连接事件中写入的数据: {0}", responseData); 
                stream.Write(data, 0, data.Length); 
                Console.WriteLine("发送数据: {0}", message); 
                data = new Byte[256]; 
                bytes = stream.Read(buffer, 0, buffer.Length);
                responseData = System.Text.Encoding.Default.GetString(buffer, 0, bytes);
                Console.WriteLine("接收返回值: {0}", responseData); 
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e.Message);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e.Message);
            } 
            Console.Read();
        }
    }
}
