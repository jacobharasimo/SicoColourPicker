using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Browser;

namespace SicoColourPicker
{
    public partial class App : Application
    {
        
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
            
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var cultureString = HtmlPage.Document.DocumentUri.ToString().Contains("fr-ca") ? "fr" : "en";
            var culture = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;  
            foreach (var item in e.InitParams)
            {
                this.Resources.Add(item.Key, item.Value);
            }
            this.RootVisual = new MainPage();
            // Display the URL parameters.            
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (System.Diagnostics.Debugger.IsAttached) return;
            // NOTE: This will allow the application to continue running after an exception has been thrown
            // but not handled. 
            // For production applications this error handling should be replaced with something that will 
            // report the error to the website and stop the application.
            e.Handled = true;
            Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                var errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
