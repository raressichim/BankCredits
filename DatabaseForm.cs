using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_PAW_Sichim_Rares_1059E
{
    public partial class DatabaseForm : Form
    {
        public DatabaseForm()
        {
            InitializeComponent();
        }
        SqlConnection connection=new SqlConnection("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = BankCredits; Integrated Security = True; Connect Timeout = 30; Encrypt=False;");
        SqlDataAdapter adapter;
        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            DataTable dt = new DataTable();
            adapter = new SqlDataAdapter("SELECT * FROM dbo.Clients", connection);
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;

            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connection.Open();
            DataTable dt = new DataTable();
            adapter = new SqlDataAdapter("SELECT * FROM dbo.Credits", connection);
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;

            connection.Close();
        }
    }
}
