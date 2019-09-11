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
    public partial class AddUserForm : Form
    {
        BindingSource userBS = new BindingSource();

        public AddUserForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.usersTableAdapter.Fill(this.mainDBDataSet.users);
            userBS.DataSource = Startup.myData.mainDBdataset;
            userBS.DataMember = "users";
            GetRoles();
            GetEditions();
            txbUser.DataBindings.Add(new Binding("Text", userBS, "name"));
            cmbEdition.DataBindings.Add(new Binding("SelectedItem", userBS, "editions"));
            cmbRole.DataBindings.Add(new Binding("SelectedItem", userBS, "role"));
            txbPass.DataBindings.Add(new Binding("Text", userBS, "pass"));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            userBS.EndEdit();
            Startup.myData.RefreshTableAdapters();
            System.Media.SystemSounds.Beep.Play();
            tsLabel.Text = "Изменения внесены";
            btnAdd.Enabled = false;
            this.usersTableAdapter.Fill(this.mainDBDataSet.users);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            int index = userBS.Find("name", Startup.User);
            if (index != -1)
            {
                userBS.Position = index;
                btnAdd.Text = "Изменить";
                //cmbRole.Enabled = false;
                //cmbEdition.Enabled = false;
                //txbPass.Enabled = false;
            }
            else
            {
                userBS.AddNew();
                txbUser.Text = Startup.User;
                txbPass.Text = "1";
            }
            tsLabel.Text = "Готово";
        }

        private void GetRoles()
        {
            //List<string> items = Startup.myData.FoundRoles();
            foreach (string s in Startup.myData.FoundRoles())
                cmbRole.Items.Add(s);
        }

        private void GetEditions()
        {
            foreach (string s in Startup.myData.GetEditionsList())
                cmbEdition.Items.Add(s);
        }
    }
}
