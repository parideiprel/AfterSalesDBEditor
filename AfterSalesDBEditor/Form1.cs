using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace AfterSalesDBEditor
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
            button1.Visible = false;




        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (CataloghiEntities listaCataloghi = new CataloghiEntities())
            {
                string header = textBox1.Text.Substring(0, 1);
                int number;
                int.TryParse(textBox1.Text.Substring(1, 3), out number);

                string headerNew = textBox2.Text.Substring(0, 1);
                int numberNew;
                int.TryParse(textBox2.Text.Substring(1, 3), out numberNew);

                var cataQuery1 = from Cataloghi in listaCataloghi.Cataloghis where Cataloghi.PrefissoNome == header && Cataloghi.SuffissoNome == number orderby Cataloghi.SuffissoNome select Cataloghi;

                foreach (var catal in cataQuery1)
                {
                    catal.PrefissoNome = headerNew;
                    catal.SuffissoNome = numberNew;
                }

                try
                {
                    listaCataloghi.SaveChanges();
                    MessageBox.Show("Variazione eseguita!", "Esito...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    pictureBox1.Image = null;
                    pictureBox2.Image = null;
                    textBox1.Focus();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                listaCataloghi.Dispose();
            }

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            //lasciando il controllo vado a verificare nel DB quanto richiesto

            if (textBox1.Text.Length == 4 && char.IsLetter(textBox1.Text, 0))
            {
                using (CataloghiEntities listaCataloghi = new CataloghiEntities())
                {
                    textBox1.Text = textBox1.Text.ToUpper();
                    string header = textBox1.Text.Substring(0, 1);
                    int number;
                    int.TryParse(textBox1.Text.Substring(1, 3), out number);

                    var cataQuery1 = from Cataloghi in listaCataloghi.Cataloghis where Cataloghi.PrefissoNome == header && Cataloghi.SuffissoNome == number orderby Cataloghi.SuffissoNome select Cataloghi;

                    //cataQuery1.ToList();
                    Console.WriteLine("Count: " + cataQuery1.ToList().Count);
                    if (cataQuery1.ToList().Count > 0)
                    {
                        //ho almeno 1 record quindi il catalogo inserito esiste
                        foreach (var catal in cataQuery1)
                        {
                            if (catal.DataChiusura==null && catal.IdUtenteChiusura==null)
                            {
                                pictureBox1.Image = AfterSalesDBEditor.Properties.Resources.checked_checkbox_64;
                            }
                            else
                            {
                                pictureBox1.Image = AfterSalesDBEditor.Properties.Resources.x_mark_5_64;
                                MessageBox.Show("ERRORE: il catalogo inserito E'CHIUSO! Controllare inserimento!", "ERRORE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                textBox1.Focus();
                            }
                        }

                        //MessageBox.Show("EUREKA!", "EUREKA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        pictureBox1.Image = AfterSalesDBEditor.Properties.Resources.checked_checkbox_64;

                    }
                    else
                    {
                        pictureBox1.Image = AfterSalesDBEditor.Properties.Resources.x_mark_5_64;
                        MessageBox.Show("ERRORE: il catalogo inserito non esiste! Controllare inserimento!", "ERRORE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        textBox1.Focus();
                    }


                    listaCataloghi.Dispose();
                }
            }
            else
            {
                pictureBox1.Image = AfterSalesDBEditor.Properties.Resources.x_mark_5_64;
                MessageBox.Show("ATTENZIONE!! Dati inseriti non coerenti con quanto richiesto.\r\nControllare l'inserimento!", "ATTENZIONE!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox1.Focus();
            }


        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            //    //lasciando il controllo verifico che l'inserito sia coerente. se si abilito pulsante
            //    if (textBox2.Text.Length == 4 && char.IsLetter(textBox2.Text, 0))
            //    {
            //        textBox2.Text = textBox2.Text.ToUpper();

            //        if (char.IsLetter(textBox2.Text, 0) && char.IsNumber(textBox2.Text, 1) && char.IsNumber(textBox2.Text, 2) && char.IsNumber(textBox2.Text, 3))
            //        {


            //        }
            //        else
            //        {

            //        }
            //    }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //controllo quando cambia il testo e quando è conforme abilito il pulsante
            if (textBox2.Text.Length == 4 && char.IsLetter(textBox2.Text, 0) && char.IsNumber(textBox2.Text, 1) && char.IsNumber(textBox2.Text, 2) && char.IsNumber(textBox2.Text, 3))
            {
                pictureBox2.Image = AfterSalesDBEditor.Properties.Resources.checked_checkbox_64;
                textBox2.Text = textBox2.Text.ToUpper();
                button1.Visible = true;
                button1.Enabled = true;
            }
            else
            {
                pictureBox2.Image = AfterSalesDBEditor.Properties.Resources.x_mark_5_64;
                button1.Visible = false;
                button1.Enabled = false;

            }
        }
    }
}
