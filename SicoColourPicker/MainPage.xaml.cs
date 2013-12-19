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
using System.Windows.Media.Effects;

namespace SicoColourPicker
{
    public partial class MainPage : UserControl
    {
        private ObservableCollection<FrameworkElement> HueCards = new ObservableCollection<FrameworkElement>();
        private ObservableCollection<FrameworkElement> ColorCards = new ObservableCollection<FrameworkElement>();
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
            wc.DownloadStringAsync(new Uri("http://jharasimo.ecentricarts.com/SicoApi/SicoColours/withExtra=false"));            
            PaintCardZone.LayoutUpdated+=PaintCardZone_LayoutUpdated;
        }

        private void PaintCardZone_LayoutUpdated(object sender, EventArgs e)
        {
            if (HueCards.Count > 0) {                
                Jogger.Width = HueCards[0].ActualWidth * 6.5;
            }            
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
            HueSwatches = JsonConvert.DeserializeObject<WebResponse>(e.Result).Results;
            Colours = JsonConvert.DeserializeObject<WebResponse>(e.Result).Results;
            
        }

        private void setJoggerSize(FrameworkElement sample) {
            trace(sample.ActualWidth);
        }
        public void createColorCardRow(ColourList cl)
        {
            var borderRadiusAmount = 5;
            //Create the container            
            var colorCard = new Grid();
            colorCard.Margin = new Thickness(0, 0, 15, 0);
            //create the swatches
            foreach (var swatch in cl.Colours)
            {
                var shadowColor = ToColorFromHex("#494949");
                var fontColor = new SolidColorBrush(Colors.White);
                if (swatch.Font.ToLower() == "dark-font")
                {
                    shadowColor = ToColorFromHex("#ffffff");
                    fontColor = new SolidColorBrush(ToColorFromHex("#494949"));
                }
                var rowIndex = cl.Colours.IndexOf(swatch);
                colorCard.RowDefinitions.Add(new RowDefinition());
                var border = new Border();

                border.MouseLeftButtonUp += delegate { swatch_Click(swatch); };
                border.Background = new SolidColorBrush(ToColorFromHex(swatch.Background));
                var cardData = new StackPanel();
                cardData.Children.Add(new TextBlock() { Text = swatch.DisplayCode, Margin = new Thickness(5, 5, 0, 0), Foreground = fontColor, Effect = new DropShadowEffect() { BlurRadius = 2, ShadowDepth = 1, Color = shadowColor } });
                cardData.Children.Add(new TextBlock() { Text = swatch.Name, Margin = new Thickness(5, 0, 0, 0), Foreground = fontColor, Effect = new DropShadowEffect() { BlurRadius = 2, ShadowDepth = 1, Color = shadowColor } });
                var cardMargin = new Thickness(0, 0, 0, 5);
                if (rowIndex + 1 == 1)
                {
                    border.CornerRadius = new CornerRadius(borderRadiusAmount, borderRadiusAmount, 0, 0);
                }
                else if (rowIndex + 1 == cl.Colours.Count)
                {
                    cardMargin = new Thickness(0);
                    border.CornerRadius = new CornerRadius(0, 0, borderRadiusAmount, borderRadiusAmount);
                }
                if (cl.Colours.Count == 1)
                {
                    border.CornerRadius = new CornerRadius(borderRadiusAmount);
                }
                border.Child = cardData;
                border.Margin = cardMargin;
                colorCard.Children.Add(border);
                Grid.SetRow(border, rowIndex);
            }
            ColorCardZone.ColumnDefinitions.Add(new ColumnDefinition() { });
            var newColumnIndex = ColorCardZone.ColumnDefinitions.Count - 1;
            ColorCardZone.Children.Add(colorCard);
            Grid.SetColumn(colorCard, newColumnIndex);
        }

        void swatch_Click(object sender)
        {
            var item = sender as SicoColour;
            trace("clicked a Color: " + item.ColorCode);
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
            HueCards.Add(colorCard);
            
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
                var offset = (Jogger.ActualWidth);
                var percentMoved = ((double)item.GetValue(Canvas.LeftProperty)) / (parent.ActualWidth - offset);

                trace("percent moved:" + percentMoved);                
                var maxRight = parent.ActualWidth - item.ActualWidth;
                var colorCardsPosition = ((ColorCardZone.ActualWidth-parent.Width) * percentMoved) * -1;

                // Calculate the current position of the object.                                
                
                //inforce left limit
                newLeft = Math.Max(Canvas.GetLeft(parent),Math.Min(newLeft, maxRight));
                
                //trace(string.Format("Moved: {0}% | Jogger Left: {1} | Colour Ribbon Left: {2} | Colour Ribbon Width: {3}",percentMoved,newLeft,colorCardsPosition, ColorCardZone.ActualWidth));
                item.SetValue(Canvas.LeftProperty, newLeft);                
                //move the card list (reverse direction)
                ColorCardZone.SetValue(Canvas.LeftProperty, colorCardsPosition);
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
