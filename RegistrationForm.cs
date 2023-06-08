using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proiect_PAW_Sichim_Rares_1059E
{
    public partial class RegistrationForm : Form
    {
        private List<Client> clients;

        public RegistrationForm(List<Client> clients)
        {
            InitializeComponent();
            this.clients = clients;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ClientDataForm clientDataForm = new ClientDataForm(clients);
            clientDataForm.ShowDialog();
            clientDataForm.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreditDataForm creditDataForm = new CreditDataForm(clients);
            creditDataForm.ShowDialog();
            creditDataForm.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DebtForm debtForm = new DebtForm();
            debtForm.ShowDialog();
            debtForm.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DatabaseForm data = new DatabaseForm();
            data.ShowDialog();
        }
        SqlConnection connection = new SqlConnection("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = BankCredits; Integrated Security = True; Connect Timeout = 30; Encrypt=False;");
        SqlCommand cmd;


        private void saveClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                using (connection)
                {
                    connection.Open();
                    using (cmd = new SqlCommand("select * from dbo.clients "))
                    {
                        cmd.Connection = connection;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
                            {
                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        writer.WriteLine(reader[i].ToString());
                                    }
                                    writer.WriteLine();
                                }
                            }
                        }
                    }
                }
            }
        }


        private void saveCreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Be aware that in order to save the credits it is recommended to also save the clients");
            saveFileDialog1.Filter = "(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (connection)
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("select * from dbo.credits", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
                            {
                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        writer.WriteLine(reader[i].ToString());
                                    }
                                    writer.WriteLine();
                                }
                            }
                        }
                    }
                }
            }
        }


        private void restoreClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bool isRestored = false;
                    using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BankCredits;Integrated Security=True;Connect Timeout=30;Encrypt=False;"))
                    {
                        connection.Open();
                        using (StreamReader reader = new StreamReader(openFileDialog1.FileName))
                        {
                            string line;
                            List<string> data = new List<string>();

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (!string.IsNullOrWhiteSpace(line))
                                {
                                    data.Add(line);
                                }
                                else if (data.Count >= 4)
                                {
                                    int id;
                                    float income, debt;

                                    if (int.TryParse(data[0], out id) && float.TryParse(data[2], out income) && float.TryParse(data[3], out debt))
                                    {
                                        SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Clients (id, name, income, debt) VALUES (@id, @name, @income, @debt)", connection);
                                        cmd.Parameters.AddWithValue("@id", id);
                                        cmd.Parameters.AddWithValue("@name", data[1]);
                                        cmd.Parameters.AddWithValue("@income", income);
                                        cmd.Parameters.AddWithValue("@debt", debt);
                                        cmd.ExecuteNonQuery();
                                        isRestored = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("There is a problem in the file you tried to restore the data from");
                                    }

                                    data.Clear();
                                }
                            }
                        }
                        if (isRestored)
                        {

                            using (connection)
                            {
                                cmd = new SqlCommand("select * from dbo.clients", connection);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {

                                    int id = Convert.ToInt32(reader["Id"]);
                                    string name = (string)reader["Name"];
                                    float income = (float)(double)reader["Income"];
                                    List<Credit> credits = new List<Credit>();
                                    Client c = new Client(id, name, income, credits);
                                    c.Debt = (float)(double)reader["Debt"];
                                    clients.Add(c);
                                }

                            }

                        }
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // We verify if the error number is for duplicate key
                    {
                        MessageBox.Show("Cannot insert duplicate key value. The clients you are trying to restore already exist");
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while restoring the clients: " + ex.Message);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("There has been an error which occured when you tried to restore the clients");
                }
            }
        }

        private void resoreCreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    using (connection)
                    {
                        connection.Open();
                        using (StreamReader reader = new StreamReader(openFileDialog1.FileName))
                        {
                            string line;
                            List<string> data = new List<string>();
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (!string.IsNullOrWhiteSpace(line))
                                {
                                    data.Add(line);
                                }
                                else if (data.Count >= 6)
                                {
                                    int creditID, clientID, period;
                                    float amount, interestRate;
                                   
                                    if (int.TryParse(data[0], out creditID) && int.TryParse(data[1], out clientID)  && float.TryParse(data[3], out amount) && int.TryParse(data[4], out period) && float.TryParse(data[5], out interestRate))
                                    {
                                        cmd = new SqlCommand("insert into dbo.Credits(creditID,idClient,type,amount,period,interestRate) values (@creditID,@idClient,@type,@amount,@period,@interestRate)", connection);
                                        cmd.Parameters.AddWithValue("@creditID", creditID);
                                        cmd.Parameters.AddWithValue("@idClient", clientID);
                                        cmd.Parameters.AddWithValue("@type", data[2]);
                                        cmd.Parameters.AddWithValue("@amount", amount);
                                        cmd.Parameters.AddWithValue("@period", period);
                                        cmd.Parameters.AddWithValue("@interestRate", interestRate);
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        MessageBox.Show("There is a problem in the file you tried to restore the data from");
                                    }

                                    data.Clear();
                                }
                            }
                        }

                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // We verify if the error number is for duplicate key
                    {
                        MessageBox.Show("Cannot insert duplicate key value. The credits you are trying to restore already exist");
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while restoring the credits: " + ex.Message);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("There has been an error which occurred when you tried to restore the credits");
                }
            }
        }

        private void clientsChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartForm chartForm = new ChartForm(clients);
            chartForm.ShowDialog();
        }

        private void saveClientsXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                using (XmlWriter writer = XmlWriter.Create("clienti.xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Clienti");

                    foreach (Client client in clients)
                    {
                        writer.WriteStartElement("Client");

                        writer.WriteStartElement("Id");
                        writer.WriteString(client.ID.ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("Name");
                        writer.WriteString(client.Name);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Income");
                        writer.WriteString(client.Income.ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("Debt");
                        writer.WriteString(client.Debt.ToString());
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
        }
    }
}

        
    



