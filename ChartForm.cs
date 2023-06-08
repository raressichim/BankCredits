using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Proiect_PAW_Sichim_Rares_1059E
{
    public partial class ChartForm : Form
    {
        private List<Client> clients;
        public ChartForm(List<Client> clients)
        {
            InitializeComponent();
            this.clients = clients;
        }


        private void button1_Click(object sender, EventArgs e)
        {

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            ChartArea chartArea = chart1.ChartAreas.Add("DebtChartArea");

            Series series = chart1.Series.Add("DebtValue");
            series.ChartType = SeriesChartType.Column;

            foreach (Client client in clients)
            {
                series.Points.AddXY(client.Name, client.Debt);
            }

            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisY.Title = "Debt";

            chart1.Invalidate();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PrintDocument printDoc = new PrintDocument();

            printDoc.PrintPage += (senderPrint, ePrint) =>
            {
                Graphics graphics = ePrint.Graphics;
                Rectangle printArea = ePrint.MarginBounds;
                chart1.Printing.PrintPaint(graphics, printArea);
            };

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }
    }
}
