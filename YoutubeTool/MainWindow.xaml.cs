using System;
using System.Threading.Tasks;
using System.Windows;
using Project.Serialization.Json;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Windows.Controls;
using System.Reflection;

namespace YoutubeTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    //[System.Runtime.InteropServices.ComVisible(true)]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var axIWebBrowser2 = typeof(WebBrowser).GetProperty("AxIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            var comObj = axIWebBrowser2.GetValue(this.MainBorwser, null);
            comObj.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, comObj, new Object[] { true });

            this.MainBorwser.MessageHook += MainBorwser_MessageHook;
            this.MainBorwser.LoadCompleted += MainBorwser_LoadCompleted;
        }

        private IntPtr MainBorwser_MessageHook(IntPtr hwnd, Int32 msg, 
                                                IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {


            return IntPtr.Zero;
        }

        private void MainBorwser_LoadCompleted(Object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // CSS読み込み
            


            Console.WriteLine("Completed");
        }

        #region youtube
        private async void Button_Click(Object sender, RoutedEventArgs e)
        {
            try {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer() {
                    ApiKey = "APIKEY_INPUT"
                });

                var searchListRequest = youtubeService.Search.List("snippet");
                searchListRequest.Q = "word";
                searchListRequest.Type = "video";
                searchListRequest.MaxResults = 10;
                // チャンネル指定
                //searchListRequest.ChannelId = "UC2ZVDmnoZAOdLt7kI7Uaqog";

                var searchListResponse = await searchListRequest.ExecuteAsync();

                foreach (var searchResult in searchListResponse.Items) {
                    Console.WriteLine($"{searchResult.Id.VideoId}, {searchResult.Snippet.Title}");
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            this.HtmlBox.Text =
                System.IO.File.ReadAllText(@".\template.htm",
                System.Text.Encoding.UTF8);
        }

        private void UpdateButton_Click(Object sender, RoutedEventArgs e)
        {
            //this.MainBorwser.ObjectForScripting = this;
            this.MainBorwser.NavigateToString(this.HtmlBox.Text);
        }

        private void ScriptButton_Click(Object sender, RoutedEventArgs e)
        {
            JavaScript.Call(this.MainBorwser, "changeVideo", new String[] { "H4uwW4HjPUI" });
        }
    }
}
