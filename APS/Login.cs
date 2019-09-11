using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APS
{
    public partial class Login : Form
    {
        string user;
        int Width = 300;

        public Login()
        {
            InitializeComponent();
            //user = inUser;
            Width = 162;
        }

        public Login(string inUser)
        {
            InitializeComponent();
            //user = inUser;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Width = Width;
            //comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
