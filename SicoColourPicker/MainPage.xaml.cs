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
        double swatchWidth;
        bool isMouseCaptured;
        double mouseVerticalPosition;
        double mouseHorizontalPosition;
        private bool _isExtra = false;
        private double _ColorZoneWidth;
        private double _ColorZoneHeight;        
        private ObservableCollection<ColourList> _hueSwatches = new ObservableCollection<ColourList>();        
        private ObservableCollection<ColourList> _colours = new ObservableCollection<ColourList>();

        public bool IsExtra
        {
            get
            {
                return _isExtra;
            }
            set
            {
                _isExtra = value;
                NotifyPropertyChanged("IsExtra");
            }
        }
        public double ColorZoneWidth { 
            get { 
                return _ColorZoneWidth; 
            } set { 
                _ColorZoneWidth = value;
                NotifyPropertyChanged("ColorZoneWidth");
            } 
        }
        public double ColorZoneHeight
        {
            get
            {
                return _ColorZoneHeight;
            }
            set
            {
                _ColorZoneHeight = value;
                NotifyPropertyChanged("ColorZoneHeight");
            }
        }
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
					createHueRow(item);
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
                    createColorCardRow(item);
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
            ColorCardZone.Height = (68 + 5) * 6;
            if (IsExtra) {
                ColorCardZone.Height = (68+5)*8;
            }
            var count = JsonConvert.DeserializeObject<WebResponse>(e.Result).Results.Count;
            ColorCardZone.Width = (count) * (166 + 15);
            trace(string.Format("total width is: {0} for #{1} cards", ColorCardZone.Width, count));
            
            HueSwatches = JsonConvert.DeserializeObject<WebResponse>(e.Result).Results;
            Colours = JsonConvert.DeserializeObject<WebResponse>(e.Result).Results;           
        }

        public void createColorCardRow(ColourList cl) {
            var borderRadiusAmount = 5;
            //Create the container            
            var colorCard = new Grid();
            colorCard.Margin = new Thickness(0, 0, 15, 0);
            //create the swatches
            foreach (var swatch in cl.Colours)
            {
                var rowIndex = cl.Colours.IndexOf(swatch);
                colorCard.RowDefinitions.Add(new RowDefinition());
                var border = new Border();                                
                border.Background = new SolidColorBrush(ToColorFromHex(swatch.Background));                
                var colorBlock = new Rectangle();
                colorBlock.Fill = new SolidColorBrush(Colors.Transparent);                
                var cardMargin = new Thickness(0, 0, 0, 5);
                if (rowIndex + 1 == 1) {
                    border.CornerRadius = new CornerRadius(borderRadiusAmount, borderRadiusAmount, 0, 0);
                }
                else if (rowIndex + 1 == cl.Colours.Count) {
                    cardMargin = new Thickness(0);
                    border.CornerRadius = new CornerRadius(0, 0, borderRadiusAmount, borderRadiusAmount);
                }
                if (cl.Colours.Count==1) {
                    border.CornerRadius = new CornerRadius(borderRadiusAmount);
                }


                border.Margin = cardMargin;
                border.Child = colorBlock;
                colorCard.Children.Add(border);
                Grid.SetRow(border, rowIndex);
            }
            ColorCardZone.ColumnDefinitions.Add(new ColumnDefinition() { });
            var newColumnIndex = ColorCardZone.ColumnDefinitions.Count - 1;
            ColorCardZone.Children.Add(colorCard);
            Grid.SetColumn(colorCard, newColumnIndex);
        }
		public void createHueRow(ColourList cl){
            //Create the container
            var colorCard = new Grid();
            //create the swatches
            foreach (var swatch in cl.Colours)
            {
                var rowIndex = cl.Colours.IndexOf(swatch);
                colorCard.RowDefinitions.Add(new RowDefinition());
                var colorBlock = new Rectangle();
                colorBlock.Fill = new SolidColorBrush(ToColorFromHex(swatch.Background));
                colorBlock.Stroke = new SolidColorBrush(ToColorFromHex(swatch.Background));
                colorCard.Children.Add(colorBlock);
                Grid.SetRow(colorBlock, rowIndex);                                
            }
            PaintCardZone.ColumnDefinitions.Add(new ColumnDefinition() { });
            var newColumnIndex = PaintCardZone.ColumnDefinitions.Count - 1;
            PaintCardZone.Children.Add(colorCard);
            Grid.SetColumn(colorCard, newColumnIndex);
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
            if (isMouseCaptured)
            {
                var item = sender as JogDialer;
                var parent = item.Parent as Canvas;                
                double deltaH = args.GetPosition(null).X - mouseHorizontalPosition;
                double newLeft = deltaH + (double)item.GetValue(Canvas.LeftProperty);                                
                var rightPos = (double)item.GetValue(Canvas.LeftProperty);
                var offset = (item.ActualWidth/2)-6;                
                
                var percentMoved = Math.Max(0, Math.Min(1,newLeft / (parent.ActualWidth - offset)));
                var maxRight = parent.ActualWidth - item.ActualWidth;
                var colorCardsPosition = (ColorCardZone.ActualWidth * percentMoved) * -1;
                // Calculate the current position of the object.                                
                
                //inforce left limit
                newLeft = Math.Max(Canvas.GetLeft(parent),Math.Min(newLeft, maxRight));                
                trace("Percent Moved: " + percentMoved);                      
                item.SetValue(Canvas.LeftProperty, newLeft);                
                //move the card list (reverse direction)
                ColorCardZone.SetValue(Canvas.LeftProperty, (colorCardsPosition));
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
        private void trace(object output){
            System.Diagnostics.Debug.WriteLine(output.ToString());
        }

        public static Color ToColorFromHex ( string hex )
{
    if(string.IsNullOrEmpty(hex))
    {
        throw new ArgumentNullException("hex");
    }

    // remove any "#" characters
    while(hex.StartsWith("#"))
    {
        hex = hex.Substring(1);
    }

    int num = 0;
    // get the number out of the string 
    if(!Int32.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out num))
    {
        throw new ArgumentException("Color not in a recognized Hex format.");
    }

    int[] pieces = new int[4];
    if(hex.Length > 7)
    {
        pieces[0] = ((num >> 24) & 0x000000ff);
        pieces[1] = ((num >> 16) & 0x000000ff);
        pieces[2] = ((num >> 8) & 0x000000ff);
        pieces[3] = (num & 0x000000ff);
    }
    else if(hex.Length > 5)
    {
        pieces[0] = 255;
        pieces[1] = ((num >> 16) & 0x000000ff);
        pieces[2] = ((num >> 8) & 0x000000ff);
        pieces[3] = (num & 0x000000ff);
    }
    else if(hex.Length == 3)
    {
        pieces[0] = 255;
        pieces[1] = ((num >> 8) & 0x0000000f);
        pieces[1] += pieces[1] * 16;
        pieces[2] = ((num >> 4) & 0x000000f);
        pieces[2] += pieces[2] * 16;
        pieces[3] = (num & 0x000000f);
        pieces[3] += pieces[3] * 16;
    }
    return Color.FromArgb((byte) pieces[0], (byte) pieces[1], (byte) pieces[2], (byte) pieces[3]);
}
    }
}
