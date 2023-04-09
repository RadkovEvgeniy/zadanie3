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
    public partial class Form1 : Form
    {
        DB_Connection DataBase = new DB_Connection();

        int selectedrow;

        enum rowstate
        {
            Existed,
            New,
            Modified,
            ModifiedNew,
            Deleted
        }

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            createcolumns();
            createcolumns2();
            refreshdatagrid(dataGridView1);
            refreshdatagrid2();
            textBox3.Visible = false;
        }

        private void createcolumns()
        {
            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("title", "Название");
            dataGridView1.Columns.Add("descriptions", "Описание");
            dataGridView1.Columns.Add("New", String.Empty);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[3].Visible = false;
        }

        private void createcolumns2()
        {
            dataGridView2.Columns.Add("id", "id");
            dataGridView2.Columns.Add("title", "Название");
            var checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.HeaderText = "Выполнено?";
            dataGridView2.Columns.Add(checkColumn);
            dataGridView2.Columns[0].Visible = false;
        }

        private void readsinglerow(DataGridView dgw, IDataRecord rec)
        {
            dgw.Rows.Add(rec.GetInt32(0), rec.GetString(1), rec.GetString(2), rowstate.ModifiedNew);
        }

        private void readsinglerow2(IDataRecord rec)
        {
            dataGridView2.Rows.Add(rec.GetInt32(0), rec.GetString(1), rec.GetBoolean(2));
        }

        private void refreshdatagrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            String query = $"select * from Projects";

            SqlCommand com = new SqlCommand(query, DataBase.GetConnection());

            DataBase.OpenConnection();

            SqlDataReader read = com.ExecuteReader();

            while(read.Read())
            {
                readsinglerow(dgw, read);
            }

            read.Close();
        }

        private void refreshdatagrid2()
        {
            dataGridView2.Rows.Clear();

            String query = $"select * from Task";

            SqlCommand com = new SqlCommand(query, DataBase.GetConnection());

            DataBase.OpenConnection();

            SqlDataReader read = com.ExecuteReader();

            while(read.Read())
            {
                readsinglerow2(read);
            }

            read.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedrow = e.RowIndex;

            if(e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedrow];

                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[0].Value.ToString();
            }
        }

        private void DeleteData()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if(dataGridView1.Rows[index].Cells[0].Value.ToString() == String.Empty)
            {
                dataGridView1.Rows[index].Cells[3].Value = rowstate.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[3].Value = rowstate.Deleted;
        }

        private void ChangeData()
        {
            var selectedrowindex = dataGridView1.CurrentCell.RowIndex;

            var id = textBox3.Text;
            var title_p = textBox1.Text;
            var descriptions = textBox2.Text;

            if(dataGridView1.Rows[selectedrowindex].Cells[0].Value.ToString() != String.Empty)
            {
                dataGridView1.Rows[selectedrowindex].SetValues(id, title_p, descriptions);
                dataGridView1.Rows[selectedrowindex].Cells[3].Value = rowstate.Modified;
            }
        }

        private void UpdateData()
        {
            DataBase.OpenConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var row = (rowstate)dataGridView1.Rows[index].Cells[3].Value;

                if (row == rowstate.Existed)
                    continue;

                if (row == rowstate.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deletequery = $"delete from Projects where id = {id}";

                    var command = new SqlCommand(deletequery, DataBase.GetConnection());
                    command.ExecuteNonQuery();
                }


                if (row == rowstate.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var title_p = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var descriptions = dataGridView1.Rows[index].Cells[2].Value.ToString();

                    var changequery = $"update Projects set title = '{title_p}', descriptions = '{descriptions}' where id = '{id}'";

                    var command = new SqlCommand(changequery, DataBase.GetConnection());
                    command.ExecuteNonQuery();
                }
            }
            DataBase.CloseConnection();
        }

        private void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Add_project ap = new Add_project();
            ap.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteData();
            Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateData();
            refreshdatagrid(dataGridView1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
           DataBase.OpenConnection();

            for(int index = 0; index < dataGridView2.Rows.Count; index++)
            {
                var id = dataGridView2.Rows[index].Cells[0].Value.ToString();
                var title = dataGridView2.Rows[index].Cells[1].Value.ToString();
                var isDone = dataGridView2.Rows[index].Cells[2].Value.ToString();

                var changequery = $"update Task set isDone = '{isDone}', title = '{title}' where id = '{id}'";

                var command = new SqlCommand(changequery, DataBase.GetConnection());
                command.ExecuteNonQuery();
            }

            DataBase.CloseConnection();

            refreshdatagrid2();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataBase.OpenConnection();

            var selectedrowindex = dataGridView2.CurrentCell.RowIndex;

            var id = Convert.ToInt32(dataGridView2.Rows[selectedrowindex].Cells[0].Value);
            var deletequery = $"delete from Task where id = '{id}'";

            var command = new SqlCommand(deletequery, DataBase.GetConnection());
            command.ExecuteNonQuery();

            DataBase.CloseConnection();

            refreshdatagrid2();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Add_Task at = new Add_Task();
            at.ShowDialog();
        }
    }
}
