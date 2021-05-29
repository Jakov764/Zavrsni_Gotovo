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
    public partial class Form2 : Form
    {
        DateTime datum = DateTime.Today;

        List<Racun> listRacun = new List<Racun>();

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(List<Sladoled> racun)
        {
            this.racun = racun;
            InitializeComponent();
        }

        List<Sladoled> racun = new List<Sladoled>();

        public void Form2_Load(object sender, EventArgs e)
        {
            double total = 0.00;

            richTextBox1.Clear();

            foreach (Sladoled sladoled in racun)
            {
                double cijena = sladoled.Cijena;
                total += cijena;
                richTextBox1.AppendText(sladoled.ToString() + "\n\r");
            }

            Racun rac = new Racun(total, DateTime.Now);

            

            listRacun.Add(rac);

            richTextBox1.AppendText(rac.ToString());

            

            

            


        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {


            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                string putanja = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Promet");
                if (!Directory.Exists(putanja))
                {
                    Directory.CreateDirectory(putanja);
                }

                if (!File.Exists(putanja + "/Racuni.xml"))
                {
                    

                    var xmlRacuni = new XDocument(new XElement("racunLista",
                    from Racun in listRacun
                    select new XElement("Racun",
                    new XElement("RB", Racun.Broj),
                    new XElement("Datum", Racun.Datum),
                    new XElement("Total", Racun.Total),
                    new XElement("Test", DateTime.Today.ToString()))));

                    xmlRacuni.Save(putanja + "/Racuni.xml");
                }
                else
                {
                    XDocument racuni = XDocument.Load(putanja + "/Racuni.xml");

                    var noviRacun = from Racun in listRacun
                                    select new XElement("Racun",
                                    new XElement("RB", Racun.Broj),
                                    new XElement("Datum", Racun.Datum),
                                    new XElement("Total", Racun.Total),
                                    new XElement("Test", DateTime.Today.ToString()));

                    racuni.Element("racunLista").Add(noviRacun);


                    racuni.Save(putanja + "/Racuni.xml");
                }

                printDocument1.Print();
            }
            
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
