using System;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YoutubeTool
{
    public class YoutubeApi
    {
        public static async Task<ChannelListResponse> GetChannelInfoAsync(String apikey, String channelID)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer() {
                ApiKey = apikey
            });

            // チャンネルの情報とバナー画像の情報を取得する
            var searchListRequest = youtubeService.Channels.List("snippet,brandingSettings");
            searchListRequest.Id = channelID;
            return await searchListRequest.ExecuteAsync();
        }

        public static async Task<SearchListResponse> GetVideoList(String apikey, String channelID)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer() {
                ApiKey = apikey
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Type = "video";
            searchListRequest.MaxResults = 10;
            searchListRequest.ChannelId = channelID;

            return await searchListRequest.ExecuteAsync();
        }
    }
}
