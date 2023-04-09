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
    public partial class Add_Task : Form
    {
        DB_Connection DataBase = new DB_Connection();

        public Add_Task()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBase.OpenConnection();

            var title = textBox1.Text;

            var addquery = $"insert into Task(title, isDone) values ('{title}', 0)";

            var command = new SqlCommand(addquery, DataBase.GetConnection());

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Задача успешно добавлена");
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
