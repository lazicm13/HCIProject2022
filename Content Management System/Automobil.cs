using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Content_Management_System
{
    public class Automobil : INotifyPropertyChanged
    {
        private int godinaOsnivanja;
        public int GodinaOsnivanja
        {
            get { return godinaOsnivanja; }
            set
            {
                if(godinaOsnivanja != value)
                {
                    godinaOsnivanja = value;
                    OnPropertyChanged();
                }
            }
        }

        private string nazivAutomobila;
        public string NazivAutomobila
        {
            get { return nazivAutomobila; }
            set
            {
                if (nazivAutomobila != value)
                {
                    nazivAutomobila = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _datumDodavanja;
        public DateTime DatumDodavanja
        {
            get { return _datumDodavanja; }
            set
            {
                if (_datumDodavanja != value)
                {
                    _datumDodavanja = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public Automobil()
        {
            IsSelected = false;
        }

        public Automobil(string naziv, string imagePath, string filePath, int godinaOsnivanja)
        {
            NazivAutomobila = naziv;
            DatumDodavanja = DateTime.Now;
            ImagePath = imagePath;
            FilePath = filePath;
            IsSelected = false;
            GodinaOsnivanja = godinaOsnivanja;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
