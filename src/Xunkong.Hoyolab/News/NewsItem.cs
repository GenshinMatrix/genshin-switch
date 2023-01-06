namespace Xunkong.Hoyolab.News;

public class NewsItem
{

    /// <summary>
    /// 新闻内容
    /// </summary>
    [JsonPropertyName("post")]
    public NewsPost? Post { get; set; }

}
