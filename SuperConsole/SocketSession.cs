using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketServerSessionMode
{
    public class SocketSession : AppSession<SocketSession>
    {
        public override void Send(string message)
        {
            Console.WriteLine("发送消息:" + message);
            base.Send(message);
        } 
        protected override void OnSessionStarted()
        {
            Console.WriteLine("Session已启动");  
            base.OnSessionStarted();
        } 
        protected override void OnInit()
        {
            this.Charset = Encoding.GetEncoding("gb2312");
            base.OnInit();
        }
        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        { 
            Console.WriteLine($"遇到未知的请求 Key:" + requestInfo.Key + $" Body:" + requestInfo.Body);
            base.HandleUnknownRequest(requestInfo);
        }    
    }
}
