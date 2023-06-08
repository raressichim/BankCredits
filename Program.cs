using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proiect_PAW_Sichim_Rares_1059E
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SqlConnection connection = new SqlConnection("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = BankCredits; Integrated Security = True; Connect Timeout = 30; Encrypt=False;");
            SqlCommand cmd;
            List<Client> clients = new List<Client>();

            using (connection)
            {
                connection.Open();
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

            //connection.Open();
            //cmd = new SqlCommand("delete from clients", connection);
            //cmd.ExecuteNonQuery();

            ////connection.Open();
            //cmd = new SqlCommand("delete from credits", connection);
            //cmd.ExecuteNonQuery();


            Application.Run(new RegistrationForm(clients));

        }
    }
}
