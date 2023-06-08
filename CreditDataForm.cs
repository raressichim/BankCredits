using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Proiect_PAW_Sichim_Rares_1059E
{
    public partial class CreditDataForm : Form
    {
        private List<Client> clients;
        public CreditDataForm(List<Client> clients)
        {
            InitializeComponent();
            this.clients = clients;
        }
        SqlConnection connection = new SqlConnection("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = BankCredits; Integrated Security = True; Connect Timeout = 30; Encrypt=False;");
        SqlCommand cmd;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int creditID = Convert.ToInt32(tbCreditID.Text);
                int clientID = Convert.ToInt32(tbClientID.Text);
                float amount = float.Parse(tbAmount.Text);
                int period = Convert.ToInt32(tbPeriod.Text);
                CreditType type;
                if (String.Equals(comboBox1.SelectedItem, "Personal"))
                {
                    type = CreditType.Personal;
                }
                else if (String.Equals(comboBox1.SelectedItem, "Bussines"))
                {
                    type = CreditType.Bussines;
                }
                else if (String.Equals(comboBox1.SelectedItem, "Mortgage"))
                {
                    type = CreditType.Mortgage;
                }
                else
                {
                    type = CreditType.Other;
                }

                Credit c = new Credit(creditID, clientID, type, amount, period);
                bool isCreditOk = false;
                foreach (Client client in clients)
                {
                    if (client.ID == clientID)
                    {
                        if (client.IsCreditworthy())
                        {
                            string confirmMessage = $"The credit's interest rate will be {c.InterestRate}%.\n\nDo you want to add this credit?";
                            DialogResult result = MessageBox.Show(confirmMessage, "Confirmare adăugare credit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (result == DialogResult.OK)
                            {
                                connection.Open();
                                cmd = new SqlCommand("insert into dbo.Credits(creditID,idClient,type,amount,period,interestRate) values (@creditID,@idClient,@type,@amount,@period,@interestRate)", connection);
                                cmd.Parameters.AddWithValue("@creditID", creditID);
                                cmd.Parameters.AddWithValue("@idClient", clientID);
                                cmd.Parameters.AddWithValue("@type", type.ToString());
                                cmd.Parameters.AddWithValue("@amount", amount);
                                cmd.Parameters.AddWithValue("@period", period);
                                cmd.Parameters.AddWithValue("@interestRate", c.InterestRate);
                                cmd.ExecuteNonQuery();
                                cmd = new SqlCommand("update dbo.Clients set debt=debt+@amount where id=@id", connection);
                                cmd.Parameters.AddWithValue("@amount", amount);
                                cmd.Parameters.AddWithValue("@id", clientID);
                                cmd.ExecuteNonQuery();
                                connection.Close();
                                client.addCredit(c);
                                MessageBox.Show("The credit has successfully been registered");
                            }
                            else
                            {
                                MessageBox.Show("The credit was not registered");
                            }
                            isCreditOk = true;
                            break;
                        }
                        else
                        {
                            MessageBox.Show("The client is not eligible for a credit");
                            isCreditOk = true;
                            break;
                        }
                    }
                }
                if (!isCreditOk)
                {
                    MessageBox.Show("The client you are trying to assign the credit to doesn't exist ");
                }

            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // We verify if the error number is for duplicate key
                {
                    MessageBox.Show("Cannot insert duplicate key value. The credit with the same ID already exists.");
                }
                else
                {
                    MessageBox.Show("An error occurred while adding the credit: " + ex.Message);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("There has been an error which occured when you tried to register a credit");
            }
            tbClientID.Clear();
            tbCreditID.Clear();
            tbPeriod.Clear();
            tbAmount.Clear();
            this.Close();
        }
    }
}
