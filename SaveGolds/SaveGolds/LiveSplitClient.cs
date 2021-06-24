using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SaveGolds
{
    public class LiveSplitClient : IDisposable
    {
        private Socket LiveSplitSocket;

        public LiveSplitClient(string server="localhost", int port=16834)
        {
            var hostEntry = Dns.GetHostEntry(server);
            var ip = hostEntry.AddressList.First(address => !address.ToString().Contains("::"));
            var ipe = new IPEndPoint(ip, port);
            var tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            tempSocket.Connect(ipe);

            if (!tempSocket.Connected)
            {
                throw new LiveSplitConnectionException(server, port); 
            }

            LiveSplitSocket = tempSocket;
        }

        ~LiveSplitClient()
        {
            LiveSplitSocket.Shutdown(SocketShutdown.Both);
            LiveSplitSocket.Close();
        }

        public string GetSplitIndex() => LiveSplitRequest("getsplitindex\r\n");

        public string GetPreviousSplitName() => LiveSplitRequest("getprevioussplitname\r\n");

        public string GetLastSplitTime() => LiveSplitRequest("getlastsplittime\r\n");

        public string GetCurrentTime() => LiveSplitRequest("getcurrenttime\r\n");

        public string GetComparisonSplitTime() => LiveSplitRequest("getcomparisonsplittime\r\n");

        public string GetCurrentTimerPhase() => LiveSplitRequest("getcurrenttimerphase\r\n");

        private string LiveSplitRequest(string request)
        {
            byte[] send = Encoding.ASCII.GetBytes(request);
            byte[] receive = new byte[256];

            LiveSplitSocket.Send(send);

            int bytes = LiveSplitSocket.Receive(receive);
            return Encoding.ASCII.GetString(receive, 0, bytes);
        }
    }

    public class LiveSplitConnectionException : Exception
    {
        public LiveSplitConnectionException(string server, int port)
            : base($"Failed to connect to LiveSplit Server at {server} port {port}! Please make sure your address and port are correct and try again.")
        { }
    }
}
