using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace cvekŠpraljaJakovZavrsniRad
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        DateTime datum = DateTime.Today;

        



        private void Form3_Load(object sender, EventArgs e)
        {
            richTextBox1.AppendText("Datum: " + DateTime.Today.ToString().Substring(0, 9) + "\n\r");

            string putanja = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Promet");
            XDocument dokument = XDocument.Load(putanja + "/Racuni.Xml");
            var promet =
                from Racun in dokument.Element("racunLista").Elements("Racun")
                where Racun.Element("Total").Value != ""
                where Racun.Element("Test").Value == DateTime.Today.ToString()
                select Racun;

            int total2 = 0;

            foreach (XElement total in promet)
            {
                string iznos = (string)total.Element("Total");

                int iznos2 = Int32.Parse(iznos);

                total2 += iznos2;

                richTextBox1.AppendText("Iznos računa: " + (string)total.Element("Total") + ".00 kn"
                    + "\n\r");
            }

            richTextBox1.AppendText("Ukupan promet: " + total2 + ".00 kn");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";

            richTextBox1.AppendText("Datum: " + DateTime.Today.ToString().Substring(0, 9) + "\n\r");

            string putanja = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Promet");
            XDocument dokument = XDocument.Load(putanja + "/Racuni.Xml");
            var promet =
                from Racun in dokument.Element("racunLista").Elements("Racun")
                where Racun.Element("Total").Value != ""
                where Racun.Element("Test").Value == DateTime.Today.ToString()
                select Racun;

            int total2 = 0;

            foreach (XElement total in promet)
            {
                string iznos = (string)total.Element("Total");

                int iznos2 = Int32.Parse(iznos);

                total2 += iznos2;

                richTextBox1.AppendText("Iznos računa: " + (string)total.Element("Total") + ".00 kn"
                    +  "\n\r");
            }

            richTextBox1.AppendText("Ukupan promet: " + total2 + ".00 kn");

            

            
            
          

            
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrintaj_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            char[] param = { '\n' };
            if (printDialog1.PrinterSettings.PrintRange == PrintRange.Selection)
            {
                lines = richTextBox1.SelectedText.Split(param);
            }
            else
            {
                lines = richTextBox1.Text.Split(param);
            }
            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }
        }
        private int linesPrinted;
        private string[] lines;
        private void OnPrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;
            Brush brush = new SolidBrush(richTextBox1.ForeColor);
            while (linesPrinted < lines.Length)
            {
                e.Graphics.DrawString(lines[linesPrinted++],
                    richTextBox1.Font, brush, x, y);
                y += 15;
                if (y >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
            linesPrinted = 0;
            e.HasMorePages = false;
        }
    }
}
