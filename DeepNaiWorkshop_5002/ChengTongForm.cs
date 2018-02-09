using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepNaiWorkshop_5002
{
    public partial class ChengTongForm : Form
    {
        public ChengTongForm()
        {
            InitializeComponent();
        }

        private void ChengTongForm_Load(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate("https://u16673263.ctfile.com/fs/16673263-236849443");
            this.webBrowser1.ScriptErrorsSuppressed = true;
        }
    }
}
