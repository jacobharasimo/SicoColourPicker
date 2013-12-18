﻿using System.Collections.ObjectModel;
using System.Windows;

namespace SicoColourPicker
{
    public class ColourList : CustomClass
    {
        private int _id;
        private bool _isExtra;
        private ObservableCollection<SicoColour> _colours;
        private ObservableCollection<string> _hue;
        
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value == _id) return;
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }
        public bool IsExtra
        {
            get
            {
                return _isExtra;
            }
            set
            {
                if (value == _isExtra) return;
                _isExtra = value;
                NotifyPropertyChanged("IsExtra");
            }
        }
        public ObservableCollection<SicoColour> Colours
        {
            get
            {
                return _colours;
            }
            set
            {
                if (value == _colours) return;                                
                _colours = value;
                if (value.Count == 1) {
                    _colours = value;
                }

                NotifyPropertyChanged("Colours");
            }
        }
        public ObservableCollection<string> Hue
        {
            get
            {
                return _hue;
            }
            set
            {
                if (value == _hue) return;
                _hue = value;
                NotifyPropertyChanged("Hue");
            }
        }
    }
}
