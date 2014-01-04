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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows.Media.Effects;
using System.Windows.Browser;

namespace SicoColourPicker
{
    public partial class MainPage
    {
        private bool isWood;
        private readonly string _culture;
        private FrameworkElement _hueSwatchCard;
        private bool _isMouseCaptured;        
        private double _mouseHorizontalPosition;
        private bool _isExtra;
        private double _colorZoneWidth;
        private double _colorZoneHeight;        
        private readonly ObservableCollection<ColourList> _hueSwatches = new ObservableCollection<ColourList>();        
        private readonly ObservableCollection<ColourList> _colours = new ObservableCollection<ColourList>();
        private string _hash = String.Empty;
        public event PropertyChangedEventHandler PropertyChanged;
        public List<String> InitParams = new List<string>();
        public string Hash { get { return _hash; } set 
        { 
            if (value != string.Empty) {
                _hash = value.Split('#')[1];
            }
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
                _isExtra = value;
                NotifyPropertyChanged("IsExtra");
            }
        }
        public double ColorZoneWidth { 
            get { 
                return _colorZoneWidth; 
            } set { 
                _colorZoneWidth = value;
                NotifyPropertyChanged("ColorZoneWidth");
            } 
        }
        public double ColorZoneHeight
        {
            get
            {
                return _colorZoneHeight;
            }
            set
            {
                _colorZoneHeight = value;
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
                _hueSwatches.Clear();
                foreach (var item in value)
                {                    
                    _hueSwatches.Add(item);
					CreateHueRow(item);
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
                _colours.Clear();
                foreach (var item in value)
                {
                    _colours.Add(item);
                    if (isWood){
                        CreateWoodCardRow(item);                    
                    }
                    else
                    {
                        CreateColorCardRow(item);                    
                    }
                    
                }                
            }
        }
        private readonly WebClient _coloursWc = new WebClient();
        private readonly WebClient _coloursDetailWc = new WebClient();
        public string ServerUrl {get { return App.Current.Resources["ServerUrl"].ToString(); } }
        private readonly Random _random = new Random();
        public MainPage()
        {
            InitializeComponent();
            isWood = GetQueryString("mode") == "wood";
            busyIndicator.IsBusy = true;            
            Hash = HtmlPage.Document.DocumentUri.Fragment;
            _culture = HtmlPage.Document.DocumentUri.ToString().Contains("fr-ca") ? "fr-ca" : "en-ca";
            FetchColors(GetQueryString("hue"), GetQueryString("mode"), GetQueryString("withExtra"), GetQueryString("card"));            
            PaintCardZone.LayoutUpdated+=PaintCardZone_LayoutUpdated;
            ColorDetailView.BackButtonClick += CloseColourDetail;
            ColorDetailView.NextSwatchClick += ColorDetailView_NextSwatchClick;
            ColorDetailView.PreviousSwatchClick += ColorDetailView_PreviousSwatchClick;
        }
        void ColorDetailView_PreviousSwatchClick(object sender, EventArgs e)
        {
            var colourCode = ColorDetailView.SelectedColour.PrevColorCode;
            if (String.IsNullOrEmpty(colourCode)) return;
            busyIndicator.IsBusy = true;
            Trace("previous Color: " + colourCode);
            var param = string.Format(ServerUrl + "/SicoApi/SicoColours/?Culture={0}&code={1}&Unused={2}", _culture, colourCode, _random.Next().ToString());
            _coloursDetailWc.DownloadStringCompleted += ColoursDetailWC_DownloadStringCompleted;
            _coloursDetailWc.DownloadStringAsync(new Uri(param, UriKind.RelativeOrAbsolute));
        }
        void ColorDetailView_NextSwatchClick(object sender, EventArgs e)
        {
            var colourCode = ColorDetailView.SelectedColour.NextColorCode;
            if (String.IsNullOrEmpty(colourCode)) return;
            busyIndicator.IsBusy = true;
            Trace("next Color: " + colourCode);
            var param = string.Format(ServerUrl + "/SicoApi/SicoColours/?Culture={0}&code={1}&Unused={2}", _culture, colourCode, _random.Next().ToString());
            _coloursDetailWc.DownloadStringCompleted += ColoursDetailWC_DownloadStringCompleted;
            _coloursDetailWc.DownloadStringAsync(new Uri(param, UriKind.RelativeOrAbsolute));
            
        }
        static string GetQueryString(string key) {
            var result = string.Empty;
            if (HtmlPage.Document.QueryString.ContainsKey(key))
            {
                result = HtmlPage.Document.QueryString[key];
            }
            return result;
        }
        private void FetchColors(string hue, string mode, string withExtra, string card)
        {
            string p;
            if (withExtra == string.Empty) {
                withExtra = "false";
            }
            if (mode != "wood")
            {
                _coloursWc.DownloadStringCompleted += wc_DownloadColorStringCompleted;
                p = string.Format(ServerUrl + "/SicoApi/SicoColours/?Culture={0}&withExtra={1}&Unused={2}", _culture,
                    withExtra, _random.Next());
            }
            else
            {
                _coloursWc.DownloadStringCompleted += wc_DownloadWoodStringCompleted;
                p = string.Format(ServerUrl + "/SicoApi/SicoWoodstains/?Culture={0}&card={1}&Unused={2}", _culture, card, _random.Next());
            }
            if (hue != string.Empty) {
                p = string.Format(ServerUrl + "/SicoApi/SicoColours/?Culture={0}&hue={1}&withExtra={2}&Unused={3}", _culture, hue, withExtra, _random.Next());                
            }            
            _coloursWc.DownloadStringAsync(new Uri(p,UriKind.RelativeOrAbsolute));            
        }
        private void ColoursDetailWC_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                var webex = (WebException)e.Error;
                var webrs = (HttpWebResponse)webex.Response;
                var sr = new System.IO.StreamReader(webrs.GetResponseStream());
                var str = sr.ReadToEnd();
                throw new Exception(String.Format("{0} - {1}: {2}", ((int)Enum.Parse(typeof(HttpStatusCode), webrs.StatusCode.ToString(), true)), webrs.StatusDescription, str), e.Error);
            }
            Dispatcher.BeginInvoke(() =>
            {
                LayoutRoot.Visibility = Visibility.Collapsed;
                ColorDetailView.Visibility = Visibility.Visible;
                busyIndicator.IsBusy = false;
            });
            var item = JsonConvert.DeserializeObject<SicoColorExtended>(JsonConvert.DeserializeObject<WebResponse>(e.Result).Results.ToString());
            ColorDetailView.SelectedColour = item;


        }
        private void CloseColourDetail(object sender, EventArgs e)
        {
            ColorDetailView.Visibility = Visibility.Collapsed;
            LayoutRoot.Visibility = Visibility.Visible;
            ColorDetailView.SelectedColour = null;
        }
        public void PaintCardZone_LayoutUpdated(object sender, EventArgs e)
        {
            if (_hueSwatchCard!=null)
            {
                Jogger.Width = _hueSwatchCard.ActualWidth * 6.5;                    
            }
            
        }
        public void wc_DownloadWoodStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                var webex = (WebException)e.Error;
                var webrs = (HttpWebResponse)webex.Response;
                var sr = new System.IO.StreamReader(webrs.GetResponseStream());
                var str = sr.ReadToEnd();
                throw new Exception(String.Format("{0} - {1}: {2}", ((int)Enum.Parse(typeof(HttpStatusCode), webrs.StatusCode.ToString(), true)), webrs.StatusDescription, str), e.Error);
            }
            ColorCardZone.Height = (68 + 5) * 6;
            if (IsExtra)
            {
                ColorCardZone.Height = (68 + 5) * 8;
            }
            var results = GetResults<WoodList>(e.Result);
            var conversion = WoodList.ToColourList(results);
            var count = results.Count;
            ColorCardZone.Width = (count) * (166 + 15);
            HueSwatches = conversion;
            Colours = conversion;
        }
        public void wc_DownloadColorStringCompleted(object sender, DownloadStringCompletedEventArgs e)
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
            var results = GetResults<ColourList>(e.Result);
            var count = results.Count;
            ColorCardZone.Width = (count) * (166 + 15);
            HueSwatches = results;
            Colours = results;                        
        }       
        private static ObservableCollection<T> GetResults<T>(string result)
        {
            return JsonConvert.DeserializeObject<ObservableCollection<T>>(JsonConvert.DeserializeObject<WebResponse>(result).Results.ToString());
        }
        public void CreateWoodCardRow(ColourList cl)
        {
            var card = GetQueryString("card");

            var woodtextureBackground = new ImageBrush();
            woodtextureBackground.Stretch = Stretch.Fill;
            woodtextureBackground.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString(card+"-wood-texture.png");

            const int borderRadiusAmount = 5;
            var rad = new CornerRadius(borderRadiusAmount);
            //Create the container            
            var colorCard = new Grid { Margin = new Thickness(0, 0, 15, 0) };
            //create the swatches
            foreach (var swatch in cl.Colours)
            {
                var item = swatch;
                var shadowColor = ToColorFromHex("#494949");
                var fontColor = new SolidColorBrush(Colors.White);
                if (swatch.Font.ToLower() == "dark-font")
                {
                    shadowColor = ToColorFromHex("#ffffff");
                    fontColor = new SolidColorBrush(ToColorFromHex("#494949"));
                }
                var rowIndex = cl.Colours.IndexOf(swatch);
                colorCard.RowDefinitions.Add(new RowDefinition());
                var border = new Border { Cursor = Cursors.Hand, Background = new SolidColorBrush(ToColorFromHex(swatch.Background)), CornerRadius = rad };
                border.MouseLeftButtonUp += delegate { swatch_Click(item); };
                var inner = new Border {CornerRadius = rad, Background = woodtextureBackground};

                var cardData = new StackPanel();
                cardData.Children.Add(new TextBlock { Text = swatch.DisplayCode, Margin = new Thickness(15, 15, 0, 0), Foreground = fontColor, Effect = new DropShadowEffect { BlurRadius = 2, ShadowDepth = 1, Color = shadowColor } });
                cardData.Children.Add(new TextBlock { Text = swatch.Name, Margin = new Thickness(15, 0, 0, 0), Foreground = fontColor, Effect = new DropShadowEffect { BlurRadius = 2, ShadowDepth = 1, Color = shadowColor } });
                var cardMargin = new Thickness(0, 0, 0, 5);
                inner.Child = cardData;
                border.Child = inner;
                inner.Margin = cardMargin;
                colorCard.Children.Add(border);
                Grid.SetRow(border, rowIndex);
            }
            ColorCardZone.ColumnDefinitions.Add(new ColumnDefinition());
            var newColumnIndex = ColorCardZone.ColumnDefinitions.Count - 1;
            ColorCardZone.Children.Add(colorCard);
            Grid.SetColumn(colorCard, newColumnIndex);
            //When the last card is inserted dispatch a timer to update the display
            if (Colours.IndexOf(cl) == Colours.Count - 1)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    busyIndicator.IsBusy = false;
                });
            }
        }
        public void CreateColorCardRow(ColourList cl)
        {            
            const int borderRadiusAmount = 5;
            //Create the container            
            var colorCard = new Grid { Margin = new Thickness(0, 0, 15, 0) };
            //create the swatches
            foreach (var swatch in cl.Colours)
            {
                var item = swatch;
                var shadowColor = ToColorFromHex("#494949");
                var fontColor = new SolidColorBrush(Colors.White);
                if (swatch.Font.ToLower() == "dark-font")
                {
                    shadowColor = ToColorFromHex("#ffffff");
                    fontColor = new SolidColorBrush(ToColorFromHex("#494949"));
                }
                var rowIndex = cl.Colours.IndexOf(swatch);
                colorCard.RowDefinitions.Add(new RowDefinition());
                var border = new Border { Cursor = Cursors.Hand, Background = new SolidColorBrush(ToColorFromHex(swatch.Background)) };
                border.MouseLeftButtonUp += delegate { swatch_Click(item); };

                var cardData = new StackPanel();
                cardData.Children.Add(new TextBlock { Text = swatch.DisplayCode, Margin = new Thickness(5, 5, 0, 0), Foreground = fontColor, Effect = new DropShadowEffect { BlurRadius = 2, ShadowDepth = 1, Color = shadowColor } });
                cardData.Children.Add(new TextBlock { Text = swatch.Name, Margin = new Thickness(5, 0, 0, 0), Foreground = fontColor, Effect = new DropShadowEffect { BlurRadius = 2, ShadowDepth = 1, Color = shadowColor } });
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
            ColorCardZone.ColumnDefinitions.Add(new ColumnDefinition());
            var newColumnIndex = ColorCardZone.ColumnDefinitions.Count - 1;
            ColorCardZone.Children.Add(colorCard);
            Grid.SetColumn(colorCard, newColumnIndex);
            //When the last card is inserted dispatch a timer to update the display
            if (Colours.IndexOf(cl) == Colours.Count - 1)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    busyIndicator.IsBusy = false;
                });
            }
        }
        public void swatch_Click(object sender)
        {           
            busyIndicator.IsBusy = true;                        
            var item = sender as SicoColour;
            if (item == null)
            {
                throw new Exception("Null swatch clicked");
            }
            Trace("clicked a Color: " + item.ColorCode);
            var param = string.Format(ServerUrl + "/SicoApi/SicoColours/?Culture={0}&code={1}&Unused={2}", _culture, item.ColorCode, _random.Next().ToString());
            _coloursDetailWc.DownloadStringCompleted += ColoursDetailWC_DownloadStringCompleted;
            _coloursDetailWc.DownloadStringAsync(new Uri(param, UriKind.RelativeOrAbsolute));                        
        }
		public void CreateHueRow(ColourList cl){
            //Create the container
            var colorCard = new Grid();
            //create the swatches
            foreach (var swatch in cl.Colours)
            {
                var rowIndex = cl.Colours.IndexOf(swatch);
                colorCard.RowDefinitions.Add(new RowDefinition());
                var colorBlock = new Rectangle
                {
                    Fill = new SolidColorBrush(ToColorFromHex(swatch.Background)),
                    Stroke = new SolidColorBrush(ToColorFromHex(swatch.Background))
                };
                colorCard.Children.Add(colorBlock);
                Grid.SetRow(colorBlock, rowIndex);                                
            }            
            PaintCardZone.ColumnDefinitions.Add(new ColumnDefinition());
            var newColumnIndex = PaintCardZone.ColumnDefinitions.Count - 1;
            PaintCardZone.Children.Add(colorCard);
		    _hueSwatchCard = colorCard;

            
            
            Grid.SetColumn(colorCard, newColumnIndex);            
		}
        public void Handle_MouseDown(object sender, MouseEventArgs args)
        {
            var item = sender as JogDialer;
            if (item == null)
            {
                throw new Exception("Null jogger");
            }
            _mouseHorizontalPosition = args.GetPosition(null).X;
            _isMouseCaptured = true;
            item.CaptureMouse();
        }
        public void Handle_MouseMove(object sender, MouseEventArgs args)
        {
            if (!_isMouseCaptured) return;
            var item = sender as JogDialer;
            if (item == null) { throw new Exception("Null jogger on mouse move");}
            var parent = item.Parent as Canvas;
            if (parent == null) { throw new Exception("Null jogger parent on mouse move");}
            var deltaH = args.GetPosition(null).X - _mouseHorizontalPosition;
            var newLeft = deltaH + (double)item.GetValue(Canvas.LeftProperty);                                                
            var offset = (Jogger.ActualWidth);
            var percentMoved = ((double)item.GetValue(Canvas.LeftProperty)) / (parent.ActualWidth - offset);

            Trace("percent moved:" + percentMoved);                
            var maxRight = parent.ActualWidth - item.ActualWidth;
            var colorCardsPosition = ((ColorCardZone.ActualWidth-parent.Width) * percentMoved) * -1;
            //inforce left limit
            newLeft = Math.Max(Canvas.GetLeft(parent),Math.Min(newLeft, maxRight));
            //trace(string.Format("Moved: {0}% | Jogger Left: {1} | Colour Ribbon Left: {2} | Colour Ribbon Width: {3}",percentMoved,newLeft,colorCardsPosition, ColorCardZone.ActualWidth));
            item.SetValue(Canvas.LeftProperty, newLeft);                
            //move the card list (reverse direction)
            ColorCardZone.SetValue(Canvas.LeftProperty, colorCardsPosition);
            // Update position global variables.
            _mouseHorizontalPosition = args.GetPosition(null).X;
        }
        public void Handle_MouseUp(object sender, MouseEventArgs args)
        {
            var item = sender as JogDialer;
            if (item == null){throw new Exception("Null jogger on mouse up");}
            _isMouseCaptured = false;
            item.ReleaseMouseCapture();
            _mouseHorizontalPosition = -1;
        }        
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void Trace(object output){
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
            int num;
            // get the number out of the string 
            if(!Int32.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out num))
            {
                throw new ArgumentException("Color not in a recognized Hex format.");
            }
            var pieces = new int[4];
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