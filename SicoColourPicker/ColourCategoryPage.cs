namespace SicoColourPicker
{
    public class ColourCategoryPage : CustomClass
    {
        private string _name;
        private string _titleTheme;
        private string _thumbnail;
        private string _documentPath;
        private string _typeName;
        private string _categoryname;
        private string _subcategoryName;
        public string Name {
            get { 
                return _name; }
            set { 
                _name = value; 
                NotifyPropertyChanged("Name"); }
        }
        public string TitleTheme {
            get
            {
                return _titleTheme;
            }
            set
            {
                _titleTheme = value;
                NotifyPropertyChanged("TitleTheme");
            }
        }
        public string Thumbnail {
            get
            {
                return _thumbnail;
            }
            set
            {
                _thumbnail = value;
                NotifyPropertyChanged("Thumbnail");
            }
        }
        public string DocumentPath {
            get
            {
                return _documentPath;
            }
            set
            {
                _documentPath = value;
                NotifyPropertyChanged("DocumentPath");
            }
        }
        public string TypeName {
            get
            {
                return _typeName;
            }
            set
            {
                _typeName = value;
                NotifyPropertyChanged("TypeName");
            }
        }
        public string CategoryName {
            get
            {
                return _categoryname;
            }
            set
            {
                _categoryname = value;
                NotifyPropertyChanged("CategoryName");
            }
        }
        public string SubcategoryName {
            get
            {
                return _subcategoryName;
            }
            set
            {
                _subcategoryName = value;
                NotifyPropertyChanged("SubcategoryName");
            }
        }
    }
}
