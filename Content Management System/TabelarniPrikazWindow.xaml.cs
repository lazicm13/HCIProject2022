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
using System.Windows.Navigation;
using System.ComponentModel;
using System.IO;

namespace Content_Management_System
{
    /// <summary>
    /// Interaction logic for TabelarniPrikazWindow.xaml
    /// </summary>
    /// 
    
    public partial class TabelarniPrikazWindow : Window
    {
        private DataIO serializer = new DataIO();
        public static BindingList<Automobil> Automobili { get; set; }

        public bool admin;

        public TabelarniPrikazWindow()
        {
            Automobili = serializer.DeSerializeObject<BindingList<Automobil>>("automobili.xml");
            if(Automobili == null)
            {
                Automobili = new BindingList<Automobil>();
                
            }
            DataContext = this;
            
            
            foreach(Automobil auto in Automobili)
            {
                if(auto.IsSelected)
                {
                    auto.IsSelected = false;
                }
            }

            InitializeComponent();

        }

        public void ReadOnly(string user)
        {
            if (user.ToLower() == "korisnik")
            {
                buttonDodaj.Visibility = Visibility.Hidden;
                buttonObrisi.Visibility = Visibility.Hidden;
                dgCheckBoxColumn.Visibility = Visibility.Hidden;
                admin = false;
            }
            else
            {
                admin = true;
            }
        }

        private void buttonIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonObrisi_Click(object sender, RoutedEventArgs e)
        {
            List<Automobil> itemsToRemove = new List<Automobil>();
            foreach (var item in dataGridAutomobili.Items)
            {
                // Get the value of the property bound to the DataGridCheckBoxColumn for the current item
                bool isSelected = (bool)item.GetType().GetProperty("IsSelected").GetValue(item);

                if (isSelected)
                {
                    // Add the current item to the list of items to remove
                    itemsToRemove.Add((Automobil)item);
                    File.Delete(((Automobil)item).FilePath);
                }
            }

            // Remove the items from the Automobili list
            foreach (var item in itemsToRemove)
            {
                Automobili.Remove(item);
            }

            if(itemsToRemove.Count != 0)
                MessageBox.Show("Uspešno brisanje!", "Brisanje", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void buttonDodaj_Click(object sender, RoutedEventArgs e)
        {
            DodajWindow wind = new DodajWindow();
            wind.ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            serializer.SerializeObject<BindingList<Automobil>>(Automobili, "automobili.xml");
        }

        private void hyperlink_clicked(object sender,  RoutedEventArgs e)
        {
            int index = dataGridAutomobili.SelectedIndex;
            if(admin)
            {
                DodajWindow wind = new DodajWindow();
                wind.textBoxNaziv.Text = Automobili[index].NazivAutomobila.ToString();
                wind.selectedImage.Source = new BitmapImage(new Uri(Automobili[index].ImagePath));
                wind.buttonDodaj.Content = "Izmeni";
                wind.labelNaslov.Content = "Izmena";
                wind.txtGodinaOsnivanja.Text = Automobili[index].GodinaOsnivanja.ToString();
                wind.txtGodinaOsnivanja.Foreground = Brushes.Black;
                wind.textBoxNaziv.Foreground = Brushes.Black;
                TextRange range;
                FileStream fStream;

                if (File.Exists(Automobili[index].FilePath))
                {
                    range = new TextRange(wind.richTextBoxOpis.Document.ContentStart, wind.richTextBoxOpis.Document.ContentEnd);
                    fStream = new FileStream(Automobili[index].FilePath, FileMode.Open);

                    try
                    {
                        range.Load(fStream, DataFormats.Rtf);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show($"Failed to load file: {ex.Message}");
                    }
                    finally
                    {
                        fStream.Close();
                    }
                }
                else
                {
                    MessageBox.Show($"File not found: {Automobili[index].FilePath}");
                }

                wind.ShowDialog();
            }else
            {
                PodaciWindow podaciWind = new PodaciWindow();
                podaciWind.labelMarkaBind.Content = Automobili[index].NazivAutomobila.ToString();
                podaciWind.labelGodinaOsnivanjaBind.Content = Automobili[index].GodinaOsnivanja;
                podaciWind.showImage.Source = new BitmapImage(new Uri(Automobili[index].ImagePath));
                

                TextRange range;
                FileStream fStream;

                if (File.Exists(Automobili[index].FilePath))
                {
                    range = new TextRange(podaciWind.rtbOpis.Document.ContentStart, podaciWind.rtbOpis.Document.ContentEnd);
                    fStream = new FileStream(Automobili[index].FilePath, FileMode.Open);

                    try
                    {
                        range.Load(fStream, DataFormats.Rtf);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show($"Failed to load file: {ex.Message}");
                    }
                    finally
                    {
                        fStream.Close();
                    }
                }
                else
                {
                    MessageBox.Show($"File not found: {Automobili[index].FilePath}");
                }

                podaciWind.ShowDialog();
            }
        }
    }
}
