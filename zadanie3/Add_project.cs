using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace zadanie3
{
    public partial class Add_project : Form
    {
        DB_Connection DataBase = new DB_Connection();

        public Add_project()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBase.OpenConnection();

            var title = textBox1.Text;
            var descript = textBox2.Text;

            var addquery = $"insert into Projects(title, descriptions) values ('{title}', '{descript}')";

            var command = new SqlCommand(addquery, DataBase.GetConnection());

            if(command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Проект успешно добавлен");
            }
            else
            {
                MessageBox.Show("Произошла ошибка");
            }
            DataBase.CloseConnection();
            this.Hide();
        }
    }
}
