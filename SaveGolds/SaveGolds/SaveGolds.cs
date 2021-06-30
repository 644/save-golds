using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using SaveGolds.Clients;
using SaveGolds.Data;

namespace SaveGolds
{
    public partial class SaveGolds : Form
    {
        private LiveSplitClient Client;
        private FFmpegClient FFmpegClient;
        private SaveGoldsConfigManager ConfigManager;
        private GoldSplitSet? ActiveSplitSet;

        public SaveGolds(LiveSplitClient client, FFmpegClient ffmpegClient)
        {
            Client = client;
            FFmpegClient = ffmpegClient;
            ActiveSplitSet = null;
            InitializeComponent();
        }

        #region Form Control Methods 

        private void SaveGolds_Load(object sender, EventArgs e)
        {
            ConfigManager = new SaveGoldsConfigManager();
            UpdateGameListBox();
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "lss files (*.lss)|*.lss|All files (*.*)|*.*";

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        ConfigManager.RegisterSplits(filePath);
                    }
                    catch (InvalidSplitFileException invalidSplitFileError)
                    {
                        ShowErrorBox("Invalid Split File", invalidSplitFileError.Message);
                        return;
                    }
                    catch (GameExistsException gameExistsException)
                    {
                        ShowErrorBox("Game Exists", gameExistsException.Message);
                        return;
                    }

                    UpdateGameListBox();
                }
            }
        }

        private void startSessionButton_Click(object sender, EventArgs e)
        {
            if(gameListBox.SelectedIndex == -1)
            {
                ShowWarningBox("Please Select Game", "You must select a game to start a session with it");
                return;
            }

            string selectedGame = gameListBox.SelectedItem.ToString();
            string gameConfigPath = ConfigManager.GetSplitConfigPath(selectedGame);
            ActiveSplitSet = GoldSplitSetManager.LoadGoldSplitSet(gameConfigPath);

            messageLabel.Show();
            endSessionButton.Show();

            CheckSplits();
        }

        private void endSessionButton_Click(object sender, EventArgs e)
        {
            ActiveSplitSet = null;
            messageLabel.Hide();
            endSessionButton.Hide();
        }

        private void deleteSplitsButton_Click(object sender, EventArgs e)
        {
            if(gameListBox.SelectedIndex == -1)
            {
                ShowWarningBox("Please Select Game", "You must select a game to start a session with it");
                return;
            }

            string selectedGame = gameListBox.SelectedItem.ToString();
            ConfigManager.DeleteRegisteredSplits(selectedGame);
            UpdateGameListBox();
        }

        #endregion

        #region Functionality Methods

        private async void CheckSplits()
        {
            int previndex = 0;
            while(Client.LiveSplitSocket.Connected && ActiveSplitSet != null)
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

        private void ShowWarningBox(string caption, string message)
        {
            MessageBox.Show(
                message, 
                caption, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Warning
            );
        }

        private void ShowErrorBox(string caption, string message)
        {
            MessageBox.Show(
                message, 
                caption, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error
            );
        }

        private void UpdateGameListBox()
        {
            gameListBox.Items.Clear();
            foreach(string splitsKey in ConfigManager.GetRegisteredSplits())
            {
                gameListBox.Items.Add(splitsKey);
            }
        }

        #endregion
    }
}
