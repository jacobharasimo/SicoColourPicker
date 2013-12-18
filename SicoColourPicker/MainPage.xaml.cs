using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.ComponentModel;

namespace SicoColourPicker
{
    public partial class MainPage : UserControl
    {
        bool isMouseCaptured;
        double mouseVerticalPosition;
        double mouseHorizontalPosition;

        private ObservableCollection<ColourList> _hueSwatches = new ObservableCollection<ColourList>();
        
        private ObservableCollection<ColourList> _colours = new ObservableCollection<ColourList>();
        public ObservableCollection<ColourList> HueSwatches
        {
            get
            {
                return _hueSwatches;
            }
            set
            {
                if (value == _hueSwatches) return;
                foreach (var item in value)
                {
                    _hueSwatches.Add(item);
                }
            }
        }
        public ObservableCollection<ColourList> Colours
        {
            get
            {
                return _colours;
            }
            set
            {
                if (value == _colours) return;
                foreach (var item in value)
                {
                    _colours.Add(item);
                }
            }
        }               
        
        public MainPage()
        {
            InitializeComponent();
            var wc = new WebClient();
            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
            wc.DownloadStringAsync(new Uri("http://jharasimo.ecentricarts.com/SicoApi/SicoColours/?hue=red"));
        }        
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null) {
                var webex = (WebException)e.Error;
                var webrs = (HttpWebResponse)webex.Response;
                var sr = new System.IO.StreamReader(webrs.GetResponseStream());
                var str = sr.ReadToEnd();
                throw new Exception(String.Format("{0} - {1}: {2}", ((int)Enum.Parse(typeof(HttpStatusCode), webrs.StatusCode.ToString(), true)), webrs.StatusDescription, str), e.Error);
            }

            HueSwatches = JsonConvert.DeserializeObject<WebResponse>(e.Result).Results;
            Colours = JsonConvert.DeserializeObject<WebResponse>(e.Result).Results;
            var dec = System.Decimal.Divide(Convert.ToDecimal(userControl.ActualWidth), HueSwatches.Count);            
            double sw =Convert.ToDouble(dec);            
            foreach (var item in HueSwatches)
            {
                var large = item.Colours.Count == 1;
                foreach(var chip in item.Colours){
                    if (large) {
                        chip.SwatchHeight = HuesList.Height;                            
                    }
                    else{
                        chip.SwatchHeight = HuesList.Height / item.Colours.Count;
                    }
                    chip.SwatchWidth =sw;                    
                    
                }
            }            
            foreach (var item in Colours)
            {
                var large = item.Colours.Count == 1;
                foreach (var chip in item.Colours)
                {
                    if (large)
                    {
                        chip.SwatchHeight = ColourList.ActualHeight;
                    }
                    else
                    {
                        chip.SwatchHeight = 68;
                    }
                    chip.SwatchWidth = 166;
                }
            }
           
            
        }

        public void Handle_MouseDown(object sender, MouseEventArgs args)
        {
            var item = sender as JogDialer;
            mouseVerticalPosition = args.GetPosition(null).Y;
            mouseHorizontalPosition = args.GetPosition(null).X;
            isMouseCaptured = true;
            item.CaptureMouse();
        }
        public void Handle_MouseMove(object sender, MouseEventArgs args)
        {
            var item = sender as JogDialer;
            var parent = item.Parent as Canvas;
            var maxRight = parent.ActualWidth;
            

            if (isMouseCaptured)
            {
                // Calculate the current position of the object.
                var rightPos = (double)item.GetValue(Canvas.LeftProperty) + item.ActualWidth;
                double deltaH = args.GetPosition(null).X - mouseHorizontalPosition;
                double newLeft = deltaH + (double)item.GetValue(Canvas.LeftProperty);                
                //inforce left limit
                newLeft = Math.Max(Canvas.GetLeft(parent),Math.Min(newLeft, maxRight));
                trace("right limit: " + (Canvas.GetLeft(parent) + parent.ActualWidth));
                // Set new position of object.
                
                
                //var LowerLeftPos = Math.Max(0, (newLeft - offset) * scaleFactor);
                //move the jogger
                item.SetValue(Canvas.LeftProperty, newLeft);
                //move the card list (reverse direction)
              //  ColourList.SetValue(Canvas.LeftProperty, (LowerLeftPos * -1));
                // Update position global variables.
                mouseHorizontalPosition = args.GetPosition(null).X;
            }
        }
        public void Handle_MouseUp(object sender, MouseEventArgs args)
        {
            var item = sender as JogDialer;
            isMouseCaptured = false;
            item.ReleaseMouseCapture();
            mouseVerticalPosition = -1;
            mouseHorizontalPosition = -1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }        

        private void trace(string output){
            System.Diagnostics.Debug.WriteLine(output);
        }
    }
}
