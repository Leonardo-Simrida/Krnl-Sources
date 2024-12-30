using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

            foreach (FileInfo fileInfo1 in new DirectoryInfo("./scripts").GetFiles("*.txt"))
            {
                this.Scriptbox.Items.Add(fileInfo1.Name);
            }

            foreach (FileInfo fileInfo2 in new DirectoryInfo("./scripts").GetFiles("*.lua"))
            {
                this.Scriptbox.Items.Add(fileInfo2.Name);
            }
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

        private void AddTab_Click(object sender, RoutedEventArgs e)
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

        private void CloseTab_Click(object sender, RoutedEventArgs e)
        {
            TabItem heheitem = (TabItem)Tabs.SelectedItem;
            Tabs.Items.Remove(heheitem);
            TabCount--;
        }

        private void CloseKrnl_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeKrnl_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ExecuteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            GetViewAPI().SetText("");
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "File Select";
            openFileDialog.Filter = "Txt Files (*.txt)|*.txt|Lua Files (*.lua)|*.lua|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string text = File.ReadAllText(openFileDialog.FileName);
                GetViewAPI().SetText(text);
            }
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "File Save";
            saveFileDialog.Filter = "Txt Files (*.txt)|*.txt|Lua Files (*.lua)|*.lua|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(contents: await GetViewAPI().GetText(), path: saveFileDialog.FileName);
            }
        }

        private void InjectBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OptionsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Scriptbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = File.ReadAllText("./scripts/" + Scriptbox.SelectedItem);
            GetViewAPI().SetText(text);
        }
    }
}
