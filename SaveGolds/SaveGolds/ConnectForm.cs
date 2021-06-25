using System;
using System.Windows.Forms;

namespace SaveGolds
{
    public partial class ConnectForm : Form
    {
        public ConnectForm()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string server = ipTextBox.Text;
            int port = int.Parse(portTextBox.Text);
            
            try
            {
                var client = new LiveSplitClient(server, port);
                var ffmpegClient = new FFmpegClient();
                var mainForm = new SaveGolds(client, ffmpegClient);
                mainForm.Show();
            } 
            catch(InvalidOperationException)
            {
                ShowErrorBox(
                    $"No viable IP Address found at {server}.",
                    "No Viable IP"
                );
            }
            catch(LiveSplitConnectionException connectionException)
            {
                ShowErrorBox(
                    connectionException.Message,
                    "Error Connecting to LiveSplit"
                );
            }
        }

        private void ShowErrorBox(string message, string caption)
        {
            MessageBox.Show(
                message, 
                caption, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error
            );
        }
    }
}
