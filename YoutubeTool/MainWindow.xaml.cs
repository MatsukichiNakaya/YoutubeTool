using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Project.IO;
using RSSReader.Model;

namespace YoutubeTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private String APIKey { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            var axIWebBrowser2 = typeof(WebBrowser).GetProperty("AxIWebBrowser2",
                                    BindingFlags.Instance | BindingFlags.NonPublic);
            var comObj = axIWebBrowser2.GetValue(this.MainBorwser, null);
            comObj.GetType().InvokeMember("Silent", BindingFlags.SetProperty,
                                            null, comObj, new Object[] { true });

            this.MainBorwser.MessageHook += MainBorwser_MessageHook;

            this.APIKey = File.ReadAllText(@".\Dat\apikey.dat", Encoding.UTF8);

            MoveTopPage();
        }

        /// <summary>
        /// ブラウザのメッセージ受信
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr MainBorwser_MessageHook(IntPtr hwnd, Int32 msg, 
                                                IntPtr wParam, IntPtr lParam,
                                                ref Boolean handled)
        {
            return IntPtr.Zero;
        }

        /// <summary>
        /// ウインドウ 読み込み完了イベント
        /// </summary>
        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            // デバッグ用のコントロール（飾り）にテキスト読み込み
            this.HtmlBox.Text = File.ReadAllText(@".\html\channel_home.htm", Encoding.UTF8);
        }

        /// <summary>
        /// ブラウザ webページ読み込み完了イベント
        /// </summary>
        private void MainBorwser_LoadCompleted(Object sender, NavigationEventArgs e)
        {
            var name = Path.GetFileName(e.Uri?.AbsoluteUri ?? "");
            if (name == "channel_home.htm") {
                ChannelListSet();
            }
        }

        /// <summary>
        /// 登録しているチャンネルのリストを書き出す
        /// </summary>
        private void ChannelListSet()
        {
            // テストデータ
            var csv = CSV.ReadCell(@".\Dat\channel.csv", Encoding.UTF8);

            var sbName = new StringBuilder();
            var sbID = new StringBuilder();

            for (Int32 r = 0; r < csv.Length; r++) {
                sbName.Append(csv[r][0]).Append(",");
                sbID.Append("https://www.youtube.com/channel/").Append(csv[r][1]).Append(",");
            }
            sbName.Remove(sbName.Length - 1, 1);
            sbID.Remove(sbID.Length - 1, 1);

            // リストを出力する
            JavaScript.Call(this.MainBorwser, "listadd",
                            sbName.ToString(), sbID.ToString());
        }

        /// <summary>
        /// チャンネルのデータ書き出し
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        private async Task ChannelInfoSetAsync(String channelID)
        {
            try {
                // チャンネル情報の取得
                var response = await YoutubeApi.GetChannelInfoAsync(this.APIKey, channelID);
                if (0 < (response?.Items?.Count ?? 0)) {
                    var channelInfo = response.Items[0];
                    // ページ内のデータを書き換える
                    JavaScript.Call(this.MainBorwser, "infochange",
                                    channelInfo.Snippet.Title,
                                    channelInfo.BrandingSettings.Image.BannerImageUrl,
                                    //channelInfo.Thumbnails.Medium.Url,
                                    channelInfo.Snippet.Description.Replace("\n", "<br>"));
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// RSSフィードを使用して最新の動画を取得する
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        private FeedItem[] GetItems(String channelID)
        {
            var url = $@"https://www.youtube.com/feeds/videos.xml?channel_id={channelID}";
            return RSS.ReadFeedItems(url).ToArray();
        }

        /// <summary>
        /// RSSフィードの情報をJavascript用に変換する
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private String[] ConvertFeedItems(FeedItem[] items)
        {
            var result = new String[6];
            var title = new StringBuilder();
            var date = new StringBuilder();
            var elap = new StringBuilder();
            var thumb = new StringBuilder();
            var link = new StringBuilder();
            var desc = new StringBuilder();

            foreach (var item in items) {
                title.Append(item.Title).Append(",");
                date.Append(item.PublishDate).Append(",");
                elap.Append(item.ElapsedTime).Append(",");
                thumb.Append(item.ExtraItems[8].Attributes["url"]).Append(",");
                link.Append(item.Link).Append(",");
                if (100 < item.Summary.Length) {
                    desc.Append(item.Summary.Substring(0, 99)).Append("…").Append(",");
                }
                else {
                    desc.Append(item.Summary).Append(",");
                }
            }

            result[0] = title.Remove(title.Length - 1, 1).ToString();
            result[1] = date.Remove(date.Length - 1, 1).ToString();
            result[2] = elap.Remove(elap.Length - 1, 1).ToString();
            result[3] = thumb.Remove(thumb.Length - 1, 1).ToString();
            result[4] = link.Remove(link.Length - 1, 1).ToString();
            result[5] = desc.Remove(desc.Length - 1, 1).ToString();

            return result;
        }

        /// <summary>
        /// 取得した動画一覧をページ上に追加する
        /// </summary>
        /// <param name="channelID"></param>
        private void SetNewVideo(String channelID)
        {
            try {
                var items = GetItems(channelID);
                var args = ConvertFeedItems(items);
                JavaScript.Call(this.MainBorwser, "setvideolist", args);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }



        private void MoveTopPage()
        {
            // 初期ページを読み込み
            var pagePath = String.Format("file:///{0}",
                                            Path.GetFullPath(@".\html\channel_home.htm")
                                                .Replace(@"\", "/"));
            this.MainBorwser.Navigate(new Uri(pagePath));
        }

        /// <summary>
        /// スクリプト実行ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptButton_Click(Object sender, RoutedEventArgs e)
        {
            // テキストボックスの引数を配列に変換
            var args = String.IsNullOrEmpty(this.ArgBox.Text)
                        ? null
                        : this.ArgBox.Text.Split(',');
            JavaScript.Call(this.MainBorwser, this.TitleBox.Text, args);
        }

        /// <summary>
        /// ページ遷移イベント
        /// </summary>
        /// <remarks>
        /// リンクの形式によって動作を変える
        /// </remarks>
        private void MainBorwser_Navigating(Object sender, NavigatingCancelEventArgs e)
        {
            switch (e.Uri.Scheme) {
                case "file":
                    // ローカルファイルならページ遷移する
                    break;
                default:
                    // httpなどweb上になるページは遷移しない
                    e.Cancel = true;
                    SwitchScript(e.Uri);
                    break;
            }
        }

        private async void SwitchScript(Uri uri)
        {
            var seg = uri.Segments[1];

            switch (seg) {
                case "channel/":
                    await ChannelInfoSetAsync(uri.Segments[2]);
                    SetNewVideo(uri.Segments[2]);
                    break;
                case "watch":
                    NavigateVideo(uri);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 動画再生ページへ移動
        /// </summary>
        /// <param name="uri"></param>
        private void NavigateVideo(Uri uri)
        {
            var pagePath = String.Format("file:///{0}",
                                           Path.GetFullPath(@".\html\play_page.htm")
                                               .Replace(@"\", "/"));
            var param = $"?id={uri.PathAndQuery.Split('=')[1]}";
            var link = new Uri(pagePath + param);

            this.MainBorwser.Navigate(link);
        }

        private void UpdateButton_Click(Object sender, RoutedEventArgs e)
        {
            // 
            MoveTopPage();
        }

        private void ToolButton_Click(Object sender, RoutedEventArgs e)
        {
            if (0 < this.RowDefScript.Height.Value) {
                this.RowDefScript.Height = new GridLength(0);
            }
            else {
                this.RowDefScript.Height = new GridLength(20);
            }
        }
    }
}
