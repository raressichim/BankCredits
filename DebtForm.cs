using System;
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
    public partial class DebtForm : Form
    {
        
        public DebtForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=BankCredits; Integrated Security=True; Connect Timeout=30; Encrypt=False;");
            SqlCommand cmd;
            connection.Open();
            cmd = new SqlCommand("SELECT debt FROM dbo.clients WHERE id=@id");
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBox1.Text));
            cmd.Connection = connection;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                MessageBox.Show("The client " + textBox1.Text + " has to pay: " + reader["Debt"] + " euros");
            }
            else
            {
                MessageBox.Show("Client not found!");
            }
            connection.Close();
            textBox1.Clear();
            this.Close();
        }
    }
}
