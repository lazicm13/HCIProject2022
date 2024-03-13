using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Content_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string adminKorisnickoIme = "admin";
        private string adminLozinka = "123";

        private string korisnikKorisnickoIme = "korisnik";
        private string korisnikLozinka = "123";
        

        public string AdminKorisnickoIme { get => adminKorisnickoIme; set => adminKorisnickoIme = value; }
        public string AdminLozinka { get => adminLozinka; set => adminLozinka = value; }

        public string KorisnikKorisnickoIme { get => korisnikKorisnickoIme; set => korisnikKorisnickoIme = value; }
        public string KorisnikLozinka { get => korisnikLozinka; set => korisnikLozinka = value; }

        

        public MainWindow()
        {
            InitializeComponent();

            textBoxKorisnickoIme.Text = "Unesite korisničko ime";
            textBoxKorisnickoIme.Foreground = Brushes.LightSlateGray;
            textBoxKorisnickoIme.FontWeight = FontWeights.Normal;
            textBoxKorisnickoIme.FontSize = 12;
        }

        private bool Validate()
        {
            bool result = true;

            if (textBoxKorisnickoIme.Text.Trim().Equals("") || textBoxKorisnickoIme.Text.Trim().Equals("Unesite korisničko ime"))
            {
                result = false;
                labelKorisnickoImeGreska.Content = "Polje ne sme biti prazno!";
                textBoxKorisnickoIme.BorderBrush = Brushes.Red;
            }
            else if (!textBoxKorisnickoIme.Text.ToLower().Equals(AdminKorisnickoIme) && !textBoxKorisnickoIme.Text.ToLower().Equals(KorisnikKorisnickoIme) && !textBoxKorisnickoIme.Text.Trim().Equals("Unesite korisničko ime")) 
            {
                labelKorisnickoImeGreska.Content = "Nepostojeće korisničko ime!";
                textBoxKorisnickoIme.BorderBrush = Brushes.Red;
                result = false;
            }
            else
            {
                labelKorisnickoImeGreska.Content = "";
                textBoxKorisnickoIme.BorderBrush = Brushes.Green;
            }

            if(passwordBoxLozinka.Password.Trim().Equals(""))
            {
                result = false;
                labelLozinkaGreska.Content = "Polje ne sme biti prazno!";
                passwordBoxLozinka.BorderBrush = Brushes.Red;
            }
            else if ((textBoxKorisnickoIme.Text.ToLower().Equals(AdminKorisnickoIme) && !passwordBoxLozinka.Password.Equals(AdminLozinka)) || (textBoxKorisnickoIme.Text.ToLower().Equals(KorisnikKorisnickoIme) && !passwordBoxLozinka.Password.Equals(KorisnikLozinka)))
            {
                labelLozinkaGreska.Content = "Pogrešna lozinka!";
                result = false;
                passwordBoxLozinka.BorderBrush = Brushes.Red;
            }
            else if(!(textBoxKorisnickoIme.Text.ToLower().Equals(AdminKorisnickoIme) && passwordBoxLozinka.Password.Equals(AdminLozinka)) && !(textBoxKorisnickoIme.Text.ToLower().Equals(KorisnikKorisnickoIme) && passwordBoxLozinka.Password.Equals(KorisnikLozinka)))
            {
                labelLozinkaGreska.Content = "Pogrešno korisničko ime ili lozinka!";
                labelKorisnickoImeGreska.Content = "";
                result = false;
                passwordBoxLozinka.BorderBrush = Brushes.Red;
            }
            else
            {
                labelLozinkaGreska.Content = "";
                passwordBoxLozinka.BorderBrush = Brushes.Green;
            }
            
            return result;
        }

        private void buttonPrijava_Click(object sender, RoutedEventArgs e)
        {
            if(Validate())
            {
                TabelarniPrikazWindow tblPrikazWind = new TabelarniPrikazWindow();
                tblPrikazWind.ReadOnly(textBoxKorisnickoIme.Text);
                tblPrikazWind.ShowDialog();
            }
            
            
        }

        private void buttonIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBoxKorisnickoIme_GotFocus(object sender, RoutedEventArgs e)
        {
            if(textBoxKorisnickoIme.Text.Trim().Equals("Unesite korisničko ime"))
            {
                textBoxKorisnickoIme.Text = "";
                textBoxKorisnickoIme.Foreground = Brushes.Black;
                textBoxKorisnickoIme.FontWeight = FontWeights.Bold;
                textBoxKorisnickoIme.FontSize = 18;
            }
        }

        private void textBoxKorisnickoIme_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxKorisnickoIme.Text.Trim().Equals(""))
            {
                textBoxKorisnickoIme.Text = "Unesite korisničko ime";
                textBoxKorisnickoIme.Foreground = Brushes.SlateGray;
                textBoxKorisnickoIme.FontWeight = FontWeights.Normal;
                textBoxKorisnickoIme.FontSize = 12;
            }
        }

        private void passwordBoxLozinka_GotFocus(object sender, RoutedEventArgs e)
        {
            if(labelLozinkaBox.Content.ToString().Equals("Unesite lozinku"))
            {
                labelLozinkaBox.Visibility = Visibility.Hidden;
            }
        }

        private void passwordBoxLozinka_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBoxLozinka.Password.Trim().Equals(string.Empty)) 
            {
                labelLozinkaBox.Visibility = Visibility.Visible;
            }
        }

        private void labelLozinkaBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            passwordBoxLozinka.Focus();
        }

        private void passwordBoxLozinka_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Validate())
                {
                    TabelarniPrikazWindow wind = new TabelarniPrikazWindow();
                    wind.ReadOnly(textBoxKorisnickoIme.Text);
                    wind.ShowDialog();
                }
            }
        }
    }
}
