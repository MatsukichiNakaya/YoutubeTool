using System;
using System.Threading.Tasks;
using System.Windows;
using Project.Serialization.Json;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Windows.Controls;
using System.Reflection;
using Project.IO;

//using DotNetOpenAuth.OAuth2;

//using Google.Apis.Authentication;
//using Google.Apis.Authentication.OAuth2;
//using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
//using Google.Apis.Samples.Helper;
//using Google.Apis.Util;
//using Google.Apis.YouTube.v3.Data;

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

        }

        #region youtube
        private async void Button_Click(Object sender, RoutedEventArgs e)
        {
            try {
                var apikey = System.IO.File.ReadAllText(@".\apikey.dat", System.Text.Encoding.UTF8);
                var youtubeService = new YouTubeService(new BaseClientService.Initializer() {
                    ApiKey = apikey
                });

                //var searchListRequest = youtubeService.Search.List("snippet");
                ////searchListRequest.Q = "";
                //searchListRequest.Type = "channels";
                //searchListRequest.MaxResults = 10;
                //// チャンネル指定
                //searchListRequest.ChannelId = "UC2ZVDmnoZAOdLt7kI7Uaqog";

                var searchListRequest = youtubeService.Channels.List("snippet");
                searchListRequest.Id = "UC2ZVDmnoZAOdLt7kI7Uaqog";

                var searchListResponse = await searchListRequest.ExecuteAsync();

                foreach (var searchResult in searchListResponse.Items) {
                    //Console.WriteLine($"{searchResult.Id.VideoId}, {searchResult.Snippet.Title}");
                    Console.WriteLine("");
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
                System.IO.File.ReadAllText(@".\top_page.htm",
                System.Text.Encoding.UTF8);
        }

        private void UpdateButton_Click(Object sender, RoutedEventArgs e)
        {
            // this.MainBorwser.NavigateToString(this.HtmlBox.Text);
            this.MainBorwser.Navigate("/top_page.htm");
        }

        private void ScriptButton_Click(Object sender, RoutedEventArgs e)
        {
            JavaScript.Call(this.MainBorwser, this.TitleBox.Text, new String[] { "H4uwW4HjPUI" });
        }

        private void MainBorwser_Navigating(Object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //var uri =  e.Uri?.AbsoluteUri ?? "";
            //Console.WriteLine(uri);
        }
    }
}
