using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ColourDetailView
    {
        public static readonly DependencyProperty IsVisibileProperty =
           DependencyProperty.Register("IsVisibile", 
              typeof(Boolean), typeof(ColourDetailView), 
              new PropertyMetadata(null));
        public static readonly DependencyProperty IsPreviousColorProperty =
           DependencyProperty.Register("IsPreviousColor",
              typeof(Boolean), typeof(ColourDetailView),
              new PropertyMetadata(null));
        public static readonly DependencyProperty IsNextColorProperty =
         DependencyProperty.Register("IsNextColor",
            typeof(Boolean), typeof(ColourDetailView),
            new PropertyMetadata(null));
        public static readonly DependencyProperty SelectedColourProperty =
          DependencyProperty.Register("SelectedColour",
             typeof(SicoColorExtended), typeof(ColourDetailView),
             new PropertyMetadata(null));
        public bool IsVisibile
        {
            get { return (bool)GetValue(IsVisibileProperty); }
            set { SetValue(IsVisibileProperty, value); }
        }
        public bool IsPreviousColor
        {
            get { return (bool)GetValue(IsPreviousColorProperty); }
            set { SetValue(IsPreviousColorProperty, value); }
        }      
        public bool IsNextColor
        {
            get { return (bool)GetValue(IsNextColorProperty); }
            set { SetValue(IsNextColorProperty, value); }
        }
        public event EventHandler BackButtonClick;
        public event EventHandler NextSwatchClick;
        public event EventHandler PreviousSwatchClick;        
        public SicoColorExtended SelectedColour
        {
            get {
                return (SicoColorExtended)GetValue(SelectedColourProperty); 
            }
            set { 
                SetValue(SelectedColourProperty, value); 
            }
        }        
        public ColourDetailView()
        {
            InitializeComponent();
            Back.Click += (s, e) => {
                if (BackButtonClick != null) {
                    BackButtonClick(s, e);
                }                    
            };            
        }
        private void HideInfo_Click(object sender, RoutedEventArgs e)
        {
            IsVisibile = !IsVisibile;
        }
        private void NextSwatch_Click(object sender, RoutedEventArgs e)
        {
            if (NextSwatchClick != null)
            {
                NextSwatchClick(sender, e);
            }  
        }
        private void PreviousSwatch_Click(object sender, RoutedEventArgs e)
        {
            if (PreviousSwatchClick != null)
            {
                PreviousSwatchClick(sender, e);
            }  
        }
    }
}
