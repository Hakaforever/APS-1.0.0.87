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
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
        }

        private void Users_Load(object sender, EventArgs e)
        {
// TODO: данная строка кода позволяет загрузить данные в таблицу "mainDBDataSet.sessions". При необходимости она может быть перемещена или удалена.
            this.sessionsTableAdapter.Fill(this.mainDBDataSet.sessions);
            this.editionsTableAdapter.Fill(this.mainDBDataSet.editions);
            this.usersTableAdapter.Fill(this.mainDBDataSet.users);
            
            rolesBinding.DataSource = Startup.myData.FoundRoles();
            var x = from usr in mainDBDataSet.users.AsEnumerable()
                    join sess in mainDBDataSet.sessions on usr.id equals sess.user_id
                    select new 
                    { 
                        name = usr.name,
                        //usr.pass, 
                        //usr.editions,
                        //usr.role,
                        status = sess.status
                    };

            if (rolesBinding.Count == 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);

            BindingSource userR = new BindingSource(x, null);
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.HeaderText = "Пользователь";
            column1.DataPropertyName = "name"; // Name of the property in Fruit
            dataGridView1.Columns.Add(column1);

            DataGridViewCheckBoxColumn color2 = new DataGridViewCheckBoxColumn();
            color2.HeaderText = "Подключен";
            color2.DataPropertyName = "status"; // Name of the property in Fruit
            dataGridView1.Columns.Add(color2);

            dataGridView1.DataSource = userR;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.usersBindingSource.EndEdit();
            this.usersTableAdapter.Update(mainDBDataSet);
            this.Close();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (e.Value != null)
            //{
            //    dataGridView1.Columns[1].Tag = e.Value;
            //    e.Value = new String('*', e.Value.ToString().Length);
            //}
            if (e.Value != null)
            {
                string ff = e.Value.ToString();
                //dataGridView1.Columns[4].State = ff.ToLower().Equals("true");
            }
        }
      
    }
}
