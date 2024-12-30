using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Krnl_WRD
{
    public class WebViewAPI : WebView2
    {
        public bool isDOMLoaded { get; set; } = false;
        private string ToSetText;
        private string LatestRecievedText;

        public event EventHandler EditorReady;

        public WebViewAPI()
        {
            this.Source = new Uri($"{Directory.GetCurrentDirectory()}\\bin\\monaco\\monaco.html");
            this.DefaultBackgroundColor = System.Drawing.Color.FromArgb(255, 25, 25, 25);
            this.CoreWebView2InitializationCompleted += WebViewAPI_CoreWebView2InitializationCompleted;
        }

        protected virtual void OnEditorReady()
        {
            EventHandler handler = EditorReady;
            if (handler != null)
                handler(this, new EventArgs());
        }

        private void WebViewAPI_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            this.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            this.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            this.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            this.CoreWebView2.Settings.AreDevToolsEnabled = false;
        }

        private void CoreWebView2_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e) => LatestRecievedText = e.TryGetWebMessageAsString();

        private async void CoreWebView2_DOMContentLoaded(object sender, Microsoft.Web.WebView2.Core.CoreWebView2DOMContentLoadedEventArgs e)
        {
            await Task.Delay(1000);
            isDOMLoaded = true;
            SetText(ToSetText);
            OnEditorReady();
        }

        public async Task<string> GetText()
        {
            if (isDOMLoaded)
            {
                await this.ExecuteScriptAsync("window.chrome.webview.postMessage(editor.getValue())");
                await Task.Delay(200);

                return LatestRecievedText;
            }
            return string.Empty;
        }

        public async void SetText(string text)
        {
            if (isDOMLoaded)
                await CoreWebView2.ExecuteScriptAsync($"SetText(\"{HttpUtility.JavaScriptStringEncode(text)}\")");
        }

        public void AddIntellisense(string label, Types type, string description, string insert)
        {
            if (isDOMLoaded)
            {
                string selectedType = type.ToString();
                if (type == Types.None)
                    selectedType = "";
                this.ExecuteScriptAsync($"AddIntellisense({label}, {selectedType}, {description}, {insert});");
            }
        }

        public void Refresh()
        {
            if (isDOMLoaded)
                this.ExecuteScriptAsync($"Refresh();");
        }
    }

    public enum Types
    {
        Class,
        Color,
        Constructor,
        Enum,
        Field,
        File,
        Folder,
        Function,
        Interface,
        Keyword,
        Method,
        Module,
        Property,
        Reference,
        Snippet,
        Text,
        Unit,
        Value,
        Variable,
        None
    }
}
