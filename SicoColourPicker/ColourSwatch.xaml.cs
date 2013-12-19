using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SicoColourPicker
{
    public partial class ColourSwatch : UserControl
    {
        public ColourSwatch()
        {
            InitializeComponent();
        }

        public Brush SwatchColor { get; set; }
        public Thickness CardMargin { get; set; }
        public Brush FontColor { get; set; }
        public CornerRadius CornerRadius { get; set; }
        public string ColorNameString { get; set; }
        public string ColorCodeString { get; set; }
        public string DropShadowColor { get; set; }        
    }
}
