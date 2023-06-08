using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Proiect_PAW_Sichim_Rares_1059E
{
    public partial class ClientDataForm : Form
    {
        private List<Client> clients ;

        public ClientDataForm(List<Client> clients)
        {
            InitializeComponent();
            this.clients = clients;
        }
        SqlConnection connection=new SqlConnection("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = BankCredits; Integrated Security = True; Connect Timeout = 30; Encrypt=False;");
        SqlCommand cmd;
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(tbID.Text);
                string name = tbName.Text;
                float income = float.Parse(tbIncome.Text);
                List<Credit> credits = new List<Credit>();
                connection.Open();
                cmd = new SqlCommand("insert into dbo.Clients(id,name,income,debt) values (@id,@name,@income,@debt)", connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@income", income);
                cmd.Parameters.AddWithValue("@debt", 0);
                cmd.ExecuteNonQuery();
                connection.Close();
                Client client = new Client(id, name, income, credits);
                clients.Add(client);
                MessageBox.Show("The client has been successfully added");
                
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // We verify if the error number is for duplicate key
                {
                    MessageBox.Show("Cannot insert duplicate key value. The client with the same ID already exists.");
                }
                else
                {
                    MessageBox.Show("An error occurred while adding the client: " + ex.Message);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There has been an error which occured when you tried to register a client");
            }
            tbID.Clear();
            tbName.Clear();
            tbIncome.Clear();
            this.Close();
        }
    }
}
