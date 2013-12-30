using System.Collections.ObjectModel;

namespace SicoColourPicker
{
    public class WebResponse : CustomClass
    {
        private string _message;
        private bool _success;
        private object _results;


        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value == _message) return;
                _message = value; 
                NotifyPropertyChanged("Message");
            }
        }
        public bool Success
        {
            get
            {
                return _success;
            }
            set
            {
                if (value == _success) return;
                _success = value; 
                NotifyPropertyChanged("Success");
            }
        }
        public object Results
        {
            get
            {
                return _results;
            }
            set
            {
                if (value == _results) return;
                _results = value; 
                NotifyPropertyChanged("Results");
            }
        }
    }
}
