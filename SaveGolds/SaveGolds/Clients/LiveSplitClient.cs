using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SaveGolds.Clients
{
    public class LiveSplitClient 
    {
        public Socket LiveSplitSocket;

        public LiveSplitClient(string server="localhost", int port=16834)
        {
            var hostEntry = Dns.GetHostEntry(server);
            var ip = hostEntry.AddressList.First(address => !address.ToString().Contains("::"));
            var ipe = new IPEndPoint(ip, port);
            var tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            tempSocket.ReceiveTimeout = 500;
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

        public async Task<string> GetSplitIndex() => await LiveSplitRequest("getsplitindex\r\n");

        public async Task<string> GetPreviousSplitName() => await LiveSplitRequest("getprevioussplitname\r\n");

        public async Task<string> GetLastSplitTime() => await LiveSplitRequest("getlastsplittime\r\n");

        public async Task<string> GetCurrentTime() => await LiveSplitRequest("getcurrenttime\r\n");

        public async Task<string> GetComparisonSplitTime() => await LiveSplitRequest("getcomparisonsplittime\r\n");

        public async Task<string> GetCurrentTimerPhaseAsync() => await LiveSplitRequest("getcurrenttimerphase\r\n");

        private async Task<string> LiveSplitRequest(string request)
        {
            byte[] send = Encoding.ASCII.GetBytes(request);
            byte[] receive = new byte[256];

            LiveSplitSocket.Send(send);

            try {
                int bytes = LiveSplitSocket.Receive(receive);
                return await Task.FromResult(Encoding.ASCII.GetString(receive, 0, bytes));
            } catch (System.Net.Sockets.SocketException) {
                return "-1";
            }
        }
    }

    public class LiveSplitConnectionException : Exception
    {
        public LiveSplitConnectionException(string server, int port)
            : base($"Failed to connect to LiveSplit Server at {server} port {port}! Please make sure your address and port are correct and try again.")
        { }
    }
}
