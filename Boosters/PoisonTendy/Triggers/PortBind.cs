using SingleDose.Triggers;
using System.Collections.Generic;

namespace PoisonTendy.Triggers
{
    internal class PortBind : ITrigger
    {
        string ITrigger.TriggerName => "PortBind";

        string ITrigger.TriggerDescription => @"Listen on a specified port - begin execution when traffic is received.";

        string ITrigger.Base => @"
            System.Net.Sockets.TcpListener l = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, {{PORT}});
            l.Start();
            System.Net.Sockets.Socket s = l.AcceptSocket();

            l.Stop();
            s.Close();
            ";

        List<string> ITrigger.ReqQuestions => new List<string>()
        {
            "Enter a port:" //{{PORT}}
        };

        List<string> ITrigger.ReqPatterns => new List<string>()
        {
            "{{PORT}}"
        };
    }
}
