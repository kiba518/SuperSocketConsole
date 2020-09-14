using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;

namespace SuperSocketServerSessionMode
{
    public class SocketCommand : CommandBase<SocketSession, StringRequestInfo>
    {
        public override void ExecuteCommand(SocketSession session, StringRequestInfo requestInfo)
        {
            //根据参数个数或者其他条件判断，来进行一些自己的操作
            Console.WriteLine($"调用成功 Key:" + requestInfo.Key + $" Body:" + requestInfo.Body); 
            session.Send("已经成功接收到你的请求\r\n"); 
        }
    }
}