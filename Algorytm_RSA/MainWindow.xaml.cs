using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Algorytm_RSA
{
    public partial class MainWindow : Window
    {
        int n;

        RSA rsa;
        public MainWindow()
        {
            InitializeComponent();

            rsa = new RSA(1000);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            n= rnd.Next(0, 10000);

            BigInteger p = rsa.primeNumbers[rnd.Next(0, 999)];
            BigInteger q = rsa.primeNumbers[rnd.Next(0, 999)];

            textbox_p.Text = p.ToString();
            textbox_q.Text = q.ToString();

            rsa.GenerateKey(p, q);

            //E,N - publiczny
            //D,N - prywatny

            textbox_d.Text = rsa.D.ToString();
            textbox_e.Text = rsa.E.ToString();
            textbox_phi.Text = rsa.PHI.ToString();
            textbox_n.Text = rsa.N.ToString();

            textbox_publiczny.Text = rsa.E.ToString() + "," + rsa.N.ToString();
            textbox_prywatny.Text = rsa.D.ToString() + "," + rsa.N.ToString();

            button_save_public.IsEnabled = true;
            button_save_private.IsEnabled = true;
        }


        private void Textbox_p_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textbox_p.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisywać tylko liczby.");
                textbox_p.Text = textbox_p.Text.Remove(textbox_p.Text.Length - 1);
            }
        }

        private void Textbox_q_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textbox_q.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                textbox_q.Text = textbox_q.Text.Remove(textbox_q.Text.Length - 1);
            }
        }

        string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );
            return textRange.Text;
        }

        public BigInteger[] load_from_file()
        {
            BigInteger[] wynik = new BigInteger[2];
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\RSA";
            openFileDialog.Filter = "Text |*.txt";
            openFileDialog.Title = "Open Text File";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        String line = sr.ReadToEnd();
                        String[] tablica = line.Split(',');
                        wynik[0] = BigInteger.Parse(tablica[0]);
                        wynik[1] = BigInteger.Parse(tablica[1]);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }

            }

            return wynik;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String text_do_zaszyfrowania = StringFromRichTextBox(RichTextBox_textToEncrypt);

            if(text_do_zaszyfrowania == "\r\n")
            {
                MessageBox.Show("Nie wpisano tekstu do zaszyfrowania");
                return;
            }

            BigInteger[] zmienne = load_from_file();

            RichTextBox_Encrypted.Document.Blocks.Clear();
            char[] seperatory = new char[] { ' ', ',' };
            string[] liczby = text_do_zaszyfrowania.Split(seperatory);
            BigInteger b;

            try
            {
                for(int i = 0; i < liczby.Length; i++)
                {
                    b = BigInteger.Parse(liczby[i]);
                    b = BigInteger.ModPow(b,zmienne[0], zmienne[1]);
                    RichTextBox_Encrypted.AppendText(b+",");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("UPS coś poszło nie tak \n\n\n" + error.Message);
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            String text_do_deszyfracji = StringFromRichTextBox(RichTextBox_Encrypted);

            if (text_do_deszyfracji == "\r\n")
            {
                MessageBox.Show("Nie wpisano tekstu do deszyfracji");
                return;
            }

            BigInteger[] zmienne = load_from_file();

            RichTextBox_Decrypted.Document.Blocks.Clear();
            char[] seperatory = new char[] {','};
            string[] liczby = text_do_deszyfracji.Split(seperatory);
            BigInteger b;
            try
            {
                for (int i = 0; i < liczby.Length-1; i++)
                {
                    b = BigInteger.Parse(liczby[i]);
                    b = BigInteger.ModPow(b, zmienne[0], zmienne[1]);


                    RichTextBox_Decrypted.AppendText(b + ",");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("UPS coś poszło nie tak \n\n\n" + error.Message);
            }
        }

        private void button_save_public_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Zapisz jako...";
            saveFileDialog.FileName = n + "_Klucz_Publiczny.txt";
            saveFileDialog.Filter = "Text |*.txt";
            saveFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\RSA";

            if (saveFileDialog.ShowDialog() == true)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());

                writer.WriteLine(textbox_publiczny.Text);

                writer.Dispose();

                writer.Close();
            }
        }

        private void button_save_private_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Zapisz jako...";
            saveFileDialog.FileName = n + "_Klucz_Prywatny_.txt";
            saveFileDialog.Filter = "Text |*.txt";
            saveFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\RSA";

            if (saveFileDialog.ShowDialog() == true)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());

                writer.WriteLine(textbox_prywatny.Text);

                writer.Dispose();

                writer.Close();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            RichTextBox_textToEncrypt.Document.Blocks.Clear();
            RichTextBox_textToEncrypt.AppendText("8,10,30,20,14,41,80,120");
        }
    }
}
