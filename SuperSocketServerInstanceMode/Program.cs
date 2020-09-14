using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketServerInstanceMode
{
    class Program
    {
        static AppServer appServer { get; set; }
        static void Main(string[] args)
        {
            var serverConfig = new SuperSocket.SocketBase.Config.ServerConfig();
            serverConfig.Port = 5180;
            serverConfig.TextEncoding = "gb2312";
            serverConfig.MaxConnectionNumber = 1000;
            serverConfig.MaxRequestLength = 102400;
            serverConfig.Mode = SocketMode.Tcp;
            appServer = new AppServer(); 
            //配置
            if (!appServer.Setup(serverConfig))  
            {
                Console.WriteLine("配置失败!"); 
                return;
            } 
            //启动
            if (!appServer.Start())
            {
                Console.WriteLine("启动失败!"); 
                return;
            } 
            Console.WriteLine("启动成功，按Q退出!"); 
            appServer.NewSessionConnected += new SessionHandler<AppSession>(appServer_NewSessionConnected);
            appServer.SessionClosed += appServer_NewSessionClosed; 
            appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(appServer_NewRequestReceived); 
            while (Console.ReadKey().KeyChar != 'q')
            { 
                continue;
            } 
            //停止
            appServer.Stop(); 
            Console.WriteLine("服务已停止");
            Console.ReadKey();
        } 
        static void appServer_NewSessionConnected(AppSession session)
        {
            var count = appServer.SessionCount;
            Console.WriteLine($"服务端得到来自客户端的连接成功 ，当前会话数量：" + count);  
            //这里也可以向会话的stream里写入数据，如果在这里向流写入数据，则客户端需要在Send之前先接收一次，不然的话，Send后接收的就是这条数据了
            session.Send("连接成功");
        } 
        static void appServer_NewSessionClosed(AppSession session, CloseReason aaa)
        {
            var count = appServer.SessionCount;
            Console.WriteLine($"服务端 失去 来自客户端的连接" + session.SessionID + aaa.ToString()+ " 当前会话数量：" + count); 
        } 
        static void appServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine($"Key:" + requestInfo.Key + $" Body:" + requestInfo.Body);
            session.Send("我是返回值：" + requestInfo.Body);
        } 
    }
}
