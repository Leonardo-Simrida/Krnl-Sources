using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Krnl_WRD
{
    public partial class MainWindow : Window
    {
        public int TabCount { get; set; } = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private WebViewAPI GetViewAPI()
        {
            return Tabs.SelectedContent as WebViewAPI;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Hello, User by clicking (YES) you are accepting the fact that a false positive will show up when scaning the application. However if you don't want to see a false positive when you scaning Krnl click (NO) to exit the app. Thanks!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes) { }
            else if (messageBoxResult == MessageBoxResult.No)
            {
                Application.Current.Shutdown();
            }

            AddingStartTab();
        }

        private void AddingStartTab()
        {
            WebViewAPI webViewAPI = new WebViewAPI();
            TabItem tabItem = new TabItem()
            {
                Header = "Script " + TabCount + ".lua" as string,
                Height = 15,
                Content = webViewAPI
            };
            Tabs.Items.Add(tabItem);
            Tabs.SelectedItem = tabItem;
            TabCount++;
        }

        private void Topbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
