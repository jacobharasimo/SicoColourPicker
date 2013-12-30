using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace SicoColourPicker
{
    public class SicoColorExtended : SicoColour
    {
        private string _PositionInDisplay;
        private string _PrevColorCode;
        private string _NextColorCode;
        private string _RecommendedPrimer;
        private string _PrimerSymbol;
        private string _PrimerNote;
        private string _IsExtraNote;
        private bool _RequiresAdditionalCoat;
        private string _RequiresAddtionalCoatNote;
        private bool _IsAvailableOnlyInGallon;
        private string _IsAvailableOnlyInGallonNote;
        private List<SicoColour> _ComplementaryColours = new List<SicoColour>();
        private List<SicoColour> _TriadColours = new List<SicoColour>();
        private List<SicoColour> _ShadesColours = new List<SicoColour>();
        private List<ColourCategoryPage> _CategoryPages = new List<ColourCategoryPage>();

        public string PositionInDisplay 
        {
            get { 
                return _PositionInDisplay; 
            }
            set {
                _PositionInDisplay = value;
                NotifyPropertyChanged("PositionInDisplay");
            } 
        } // e.g. "A37", position of chip in store display
        public string PrevColorCode
        {
            get
            {
                return _PrevColorCode;
            }
            set
            {
                _PrevColorCode = value;
                NotifyPropertyChanged("PrevColorCode");
            }
        } // link to prev Chip in palette
        public string NextColorCode
        {
            get
            {
                return _NextColorCode;
            }
            set
            {
                _NextColorCode = value;
                NotifyPropertyChanged("NextColorCode");
            }
        } // link to next Chip in palette
        public string RecommendedPrimer
        {
            get
            {
                return _RecommendedPrimer;
            }
            set
            {
                _RecommendedPrimer = value;
                NotifyPropertyChanged("RecommendedPrimer");
            }
        } // AE, AT or empty
        public string PrimerSymbol
        {
            get
            {
                return _PrimerSymbol;
            }
            set
            {
                _PrimerSymbol = value;
                NotifyPropertyChanged("PrimerSymbol");
            }
        } // solid triangle, outlined triangle or empty
        public string PrimerNote
        {
            get
            {
                return _PrimerNote;
            }
            set
            {
                _PrimerNote = value;
                NotifyPropertyChanged("PrimerNote");
            }
        } // word version from UI Culture string, e.g. "This colour requires a base coat of Goprime all-in-one Alkyd Emulsion"
        public string IsExtraNote
        {
            get
            {
                return _IsExtraNote;
            }
            set
            {
                _IsExtraNote = value;
                NotifyPropertyChanged("IsExtraNote");
            }
        } // e.g. "Attention: this colour is available in stores, but not as a Colour Chip"
        public bool RequiresAdditionalCoat
        {
            get
            {
                return _RequiresAdditionalCoat;
            }
            set
            {
                _RequiresAdditionalCoat = value;
                NotifyPropertyChanged("RequiresAdditionalCoat");
            }
        }
        public string RequiresAddtionalCoatNote
        {
            get
            {
                return _RequiresAddtionalCoatNote;
            }
            set
            {
                _RequiresAddtionalCoatNote = value;
                NotifyPropertyChanged("RequiresAddtionalCoatNote");
            }
        } // e.g. "This colour may require additional coat(s)"
        public bool IsAvailableOnlyInGallon
        {
            get
            {
                return _IsAvailableOnlyInGallon;
            }
            set
            {
                _IsAvailableOnlyInGallon = value;
                NotifyPropertyChanged("IsAvailableOnlyInGallon");
            }
        }
        public string IsAvailableOnlyInGallonNote
        {
            get
            {
                return _IsAvailableOnlyInGallonNote;
            }
            set
            {
                _IsAvailableOnlyInGallonNote = value;
                NotifyPropertyChanged("IsAvailableOnlyInGallonNote");
            }
        } // e.g. "This colour is available in 3.78L size only"
        public List<SicoColour> ComplementaryColours
        {
            get
            {
                return _ComplementaryColours;
            }
            set
            {
                _ComplementaryColours = value;
                NotifyPropertyChanged("ComplementaryColours");
            }
        } // aka Duo colour; expect one ColorCode value but use List for uniformity
        public List<SicoColour> TriadColours
        {
            get
            {
                return _TriadColours;
            }
            set
            {
                _TriadColours = value;
                NotifyPropertyChanged("TriadColours");
            }
        } // expect 2 ColorCode values
        public List<SicoColour> ShadesColours
        {
            get
            {
                return _ShadesColours;
            }
            set
            {
                _ShadesColours = value;
                NotifyPropertyChanged("ShadesColours");
            }
        } // expect 2 ColorCode values
        public List<ColourCategoryPage> CategoryPages
        {
            get
            {
                return _CategoryPages;
            }
            set
            {
                _CategoryPages = value;
                NotifyPropertyChanged("CategoryPages");
            }
        } // e.g. page info for related Colour by Hue, Inspiration by Room
		public SicoColorExtended() : base(){}		
    }
}
