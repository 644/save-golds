using System;
using System.Windows.Forms;

namespace SaveGolds
{
    public partial class SaveGolds : Form
    {
        private LiveSplitClient Client;

        public SaveGolds(LiveSplitClient client)
        {
            Client = client;
            InitializeComponent();
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
