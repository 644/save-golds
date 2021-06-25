using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace SaveGolds
{
    public partial class SaveGolds : Form
    {
        private LiveSplitClient Client;
        private FFmpegClient FFmpegClient;

        public SaveGolds(LiveSplitClient client, FFmpegClient ffmpegClient)
        {
            Client = client;
            FFmpegClient = ffmpegClient;
            InitializeComponent();
            CheckSplits();
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void CheckSplits()
        {
            int previndex = 0;
            while(Client.LiveSplitSocket.Connected)
            {
                await Task.Delay(500);
                int index = Int32.Parse(await Client.GetSplitIndex());
                string status = await Client.GetCurrentTimerPhaseAsync();
                status = status.Replace(System.Environment.NewLine, "");
                if (index != previndex && status.Equals("Running"))
                {
                    previndex = index;
                    if (!richTextBox1.IsDisposed)
                    {
                        string ts = await Client.GetCurrentTime();
                        string prevname = await Client.GetPreviousSplitName();
                        string[] timestamp = ts.Split('.');
                        var time = timestamp[0];
                        var mili = timestamp[1];
                        int[] ssmmhh = { 0, 0, 0 };
                        var hhmmss = time.Split(':');
                        var reversed = hhmmss.Reverse();
                        int i = 0;
                        reversed.ToList().ForEach(x => ssmmhh[i++] = int.Parse(x));
                        var seconds = (int)(new TimeSpan(ssmmhh[2], ssmmhh[1], ssmmhh[0])).TotalSeconds;
                        richTextBox1.Text = "Split at: " + seconds + "." + mili + Environment.NewLine;
                        richTextBox1.AppendText("Split name: " + prevname + Environment.NewLine);
                        var fugg = FFmpegClient.CutVid(@"D:\replays\m9.mp4", @"D:\replays\m9-copy.mp4", 10);
                        richTextBox1.AppendText(fugg);
                        // TODO: compare splits in JSON file and begin cutting process if better
                    }
                }
            }
        }
    }
}
