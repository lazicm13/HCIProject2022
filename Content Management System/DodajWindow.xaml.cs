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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Content_Management_System
{
    /// <summary>
    /// Interaction logic for DodajWindow.xaml
    /// </summary>
    public partial class DodajWindow : Window
    {
        public string naziv;
        

        public DodajWindow()
        {
            InitializeComponent();
            textBoxNaziv.Text = "Unesite marku automobila";
            textBoxNaziv.Foreground = Brushes.LightSlateGray;

            txtGodinaOsnivanja.Text = "Unesite godinu osnivanja";
            txtGodinaOsnivanja.Foreground = Brushes.LightSlateGray;

            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);

            // popunjavanje combo boxa bojama
            foreach (var colorProperty in typeof(Colors).GetProperties())
            {
                Color color = (Color)colorProperty.GetValue(null);
                SolidColorBrush brush = new SolidColorBrush(color);
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = colorProperty.Name,
                    Background = brush,
                    Foreground = Brushes.Black,
                    Tag = brush
                };
                cmbFontColor.Items.Add(item);

                if (color == Colors.Black) 
                {
                    cmbFontColor.SelectedIndex = cmbFontColor.Items.Count - 1; 
                }
            }




            List<double> fontSizes = new List<double>();
            for (double i = 1; i <= 72; i += 0.5)
            {
                fontSizes.Add(i);
            }
            cmbFontSize.ItemsSource = fontSizes;
        }

        private void buttonDodaj_Click(object sender, RoutedEventArgs e)
        {
            string rtfFileName = textBoxNaziv.Text + ".rtf";
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rtfFileName);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = textBoxNaziv.Text + ".rtf";
            saveFileDialog.DefaultExt = ".rtf";
            saveFileDialog.Filter = "RTF files (*.rtf)|*.rtf";


            TextRange range = new TextRange(richTextBoxOpis.Document.ContentStart, richTextBoxOpis.Document.ContentEnd);
            if (Validate() && buttonDodaj.Content.ToString().Equals("Dodaj"))
            {
                Uri uri = new Uri(selectedImage.Source.ToString());
                string path = uri.LocalPath;

                TabelarniPrikazWindow.Automobili.Add(new Automobil(textBoxNaziv.Text, path, filePath, Convert.ToInt32(txtGodinaOsnivanja.Text)));
                using (FileStream fstream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    range.Save(fstream, System.Windows.DataFormats.Rtf);
                }
               

                System.Windows.MessageBox.Show("Uspešno ste dodali novu marku automobila!", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else if(Validate() && buttonDodaj.Content.ToString().Equals("Izmeni"))
            {
                Uri uri = new Uri(selectedImage.Source.ToString());
                string path = uri.LocalPath;
                foreach(Automobil auto in TabelarniPrikazWindow.Automobili)
                {
                    if(auto.NazivAutomobila == naziv)
                    {
                        auto.NazivAutomobila = textBoxNaziv.Text;
                        auto.IsSelected = false;
                        auto.ImagePath = path;
                        auto.GodinaOsnivanja = Convert.ToInt32(txtGodinaOsnivanja.Text);
                        string rtfFileName1 = textBoxNaziv.Text + ".rtf";
                        string filePath1 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rtfFileName1);
                        
                        File.Delete(auto.FilePath);
                        auto.FilePath = filePath1;
                        using (FileStream fstream = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                        {
                            range.Save(fstream, System.Windows.DataFormats.Rtf);
                        }
                    }
                }
                System.Windows.MessageBox.Show("Uspešna izmena!", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void buttonIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool Validate()
        {
            bool result = true;

            if (textBoxNaziv.Text.Trim().Equals("") || textBoxNaziv.Text.Trim().Equals("Unesite marku automobila"))
            {
                result = false;
                labelNazivGreska.Content = "Polje ne sme biti prazno!";
                textBoxNaziv.BorderBrush = Brushes.Red;
            }
            else
            {
                labelNazivGreska.Content = "";
                textBoxNaziv.BorderBrush = Brushes.Gray;
                
            }
            if(selectedImage.Source == null)
            {
                labelSlikaGreska.Content = "Slika je obavezna!";
                buttonPronadjiSliku.BorderBrush = Brushes.Red;
                buttonPronadjiSliku.BorderThickness = new Thickness(2);
                result = false;
            }

            foreach (Automobil auto in TabelarniPrikazWindow.Automobili)
            {
                if (auto.NazivAutomobila.ToLower() == textBoxNaziv.Text.ToLower() && !buttonDodaj.Content.ToString().Equals("Izmeni"))
                {
                    labelNazivGreska.Content = "Automobil sa unetim nazivom već postoji!";
                    result = false;
                    textBoxNaziv.BorderBrush = Brushes.Red;
                }
            }
            int number;
            if (txtGodinaOsnivanja.Text.Trim().Equals("") || txtGodinaOsnivanja.Text.Trim().Equals("Unesite godinu osnivanja"))
            {
                labelGodinaOsnivanjaGreska.Content = "Polje ne sme biti prazno!";
                txtGodinaOsnivanja.BorderBrush = Brushes.Red;
                result = false;
            }
            else if (Int32.TryParse(txtGodinaOsnivanja.Text, out number) == false)
            {
                labelGodinaOsnivanjaGreska.Content = "Polje mora biti broj!";
                txtGodinaOsnivanja.BorderBrush = Brushes.Red;
                result = false;
            }
            else
            {
                if(number > 2023 || number < 1851)
                {
                    if (number < 1851)
                    {
                        labelGodinaOsnivanjaGreska.Content = "Godina osnivanja mora biti veća od 1850.";
                    }
                    else
                    {
                        labelGodinaOsnivanjaGreska.Content = "Godina osnivanja mora biti manja od 2024.";
                        
                    }
                    txtGodinaOsnivanja.BorderBrush = Brushes.Red;
                    result = false;
                }
                else
                {
                    labelGodinaOsnivanjaGreska.Content = "";
                    txtGodinaOsnivanja.BorderBrush = Brushes.Gray;
                }
            }

            if (string.IsNullOrWhiteSpace(new TextRange(richTextBoxOpis.Document.ContentStart, richTextBoxOpis.Document.ContentEnd).Text))
            {
                
                rtbGreska.Content = "Polje ne sme biti prazno!";
                richTextBoxOpis.BorderBrush = Brushes.Red;
                result = false;
            }
            else
            {
                rtbGreska.Content = "";
                richTextBoxOpis.BorderBrush = Brushes.Gray;
            }

            return result;
        }

        private void textBoxNaziv_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxNaziv.Text.Trim().Equals("Unesite marku automobila"))
            {
                textBoxNaziv.Text = "";
                textBoxNaziv.Foreground = Brushes.Black;
            }
        }

        private void textBoxNaziv_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxNaziv.Text.Trim().Equals(string.Empty))
            {
                textBoxNaziv.Text = "Unesite marku automobila";
                textBoxNaziv.Foreground = Brushes.LightSlateGray;
            }
        }

        private void richTextBoxOpis_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = richTextBoxOpis.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = richTextBoxOpis.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;

            temp = richTextBoxOpis.Selection.GetPropertyValue(Inline.ForegroundProperty);
            cmbFontColor.SelectedItem = temp;

            temp = richTextBoxOpis.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.SelectedItem = temp;


            int wordCount = 0;
            string[] delimiters = new string[] { " ", "\r", "\n", "\t" };
            string text = new TextRange(richTextBoxOpis.Document.ContentStart, richTextBoxOpis.Document.ContentEnd).Text;
            wordCount = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

            wordCountTextBlock.Text = "Broj reči: " + wordCount;
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null && !richTextBoxOpis.Selection.IsEmpty)
            {
                richTextBoxOpis.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
            }
        }

        private void buttonPronadjiSliku_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            openFileDialog.ShowDialog();

            selectedImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
        }

        private void cmbFontColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontColor.SelectedItem != null && !richTextBoxOpis.Selection.IsEmpty)
            {
                ComboBoxItem comboBoxItem = cmbFontColor.SelectedItem as ComboBoxItem;
                Brush brush = comboBoxItem.Tag as SolidColorBrush;
                richTextBoxOpis.Selection.ApplyPropertyValue(Inline.ForegroundProperty, brush);
            }
        }

        private void cmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbFontSize.SelectedItem != null && !richTextBoxOpis.Selection.IsEmpty)
            {
                ComboBoxItem comboBoxItem = cmbFontSize.SelectedItem as ComboBoxItem;
                richTextBoxOpis.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.SelectedItem);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            naziv = textBoxNaziv.Text;
        }

        private void txtGodinaOsnivanja_GotFocus(object sender, RoutedEventArgs e)
        {
            if(txtGodinaOsnivanja.Text.Trim().Equals("Unesite godinu osnivanja"))
            {
                txtGodinaOsnivanja.Text = "";
                txtGodinaOsnivanja.Foreground = Brushes.Black;
            }
        }

        private void txtGodinaOsnivanja_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtGodinaOsnivanja.Text.Trim().Equals(string.Empty))
            {
                txtGodinaOsnivanja.Text = "Unesite godinu osnivanja";
                txtGodinaOsnivanja.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}

