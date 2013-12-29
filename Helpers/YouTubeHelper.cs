using MyToolkit.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Wp8Shared.Helpers
{
    class MyEpisode
    {
        public int Id { get; set; }
        public string OrigId { get; set; }
        public string Url { get; set; }
        public string Desc { get; set; }
        public string AppName { get; set; }
        public string YouTubeSearchHint { get; set; }
        public string YouTubeId { get; set; }
        public double Accuracy { get; set; }
        public override string ToString()
        {
            return base.ToString();
        }
    }

    //public delegate void OnItemFixedHandler(bool success, string title, string oldId, string newId);

    public class YouTubeHelper
    {
        //public event OnItemFixedHandler OnItemFixed;

        public static string GetYoutubeID(string uri)
        {
            ////http://www.youtube.com/watch?v=1CuGUN_rmpE
            //string id = "Uh_tZEkIVS4";
            if (string.IsNullOrEmpty(uri)) throw new ArgumentException("uri not valid");
            return uri.Substring(uri.IndexOf('=') + 1);
        }

        public static string BuildYoutubeID(string id)
        {
            return string.Format("http://www.youtube.com/watch?v={0}", id);
        }

        //public static void RetrieveYoutubeId(string title, Action<bool, string> completed = null)
        //{
        //    string youtubeResponse = "";
        //    string query = string.Format("http://www.youtube.com/results?search_query={0}", title.Replace(" ", "+").Replace("'", ""));

        //    var request = new HttpGetRequest(query);
        //    Http.Get(request, result =>
        //    {
        //        if (!result.Successful)
        //        {
        //            //if (OnItemFixed != null) OnItemFixed(false, title, currentYoutubeId, string.Empty);
        //            completed(false, string.Empty);
        //            return;
        //        }

        //        youtubeResponse = result.Response;
        //        int start = youtubeResponse.IndexOf("<ol id=\"search-results\"");
        //        int end = youtubeResponse.IndexOf("</ol>");
        //        string searchResults = youtubeResponse.Substring(start, end - start + "</ol>".Length);

        //        StringBuilder sb = new StringBuilder();
        //        foreach (var line in searchResults.Split('\n'))
        //        {
        //            string newline = line;
        //            if (line.Contains("<img"))
        //            {
        //                int endTagPos = line.IndexOf(">", line.IndexOf("<img"));
        //                newline = newline.Insert(endTagPos, "/");
        //            }
        //            sb.AppendLine(newline);
        //        }
        //        string newRes = sb.ToString();
        //        XDocument doc;
        //        try
        //        {
        //            doc = XDocument.Parse(newRes);
        //        }
        //        catch (Exception ex)
        //        {
        //            //if (OnItemFixed != null) OnItemFixed(false, title, currentYoutubeId, string.Empty);
        //            completed(false, ex.Message);
        //            return;
        //        }
        //        ////var episodesElements = doc.Descendants("li").Where(d => d.Attribute("data-context-item-id") != null && d.Attribute("data-context-item-title") != null).Take(3);                
        //        var episodesElements = doc.Descendants("li").Where(d => d.Attribute("data-context-item-id") != null && d.Attribute("data-context-item-title") != null).Take(1);

        //        List<MyEpisode> episodesList = new List<MyEpisode>();
        //        //http://www.tsjensen.com/blog/post/2011/05/27/Four+Functions+For+Finding+Fuzzy+String+Matches+In+C+Extensions.aspx
        //        foreach (var item in episodesElements)
        //        {
        //            episodesList.Add(new MyEpisode()
        //            {
        //                Desc = item.Attribute("data-context-item-title").Value,
        //                YouTubeId = item.Attribute("data-context-item-id").Value,
        //                Accuracy = item.Attribute("data-context-item-title").Value.DiceCoefficient(title)
        //            });
        //        }
                
        //        //var sortedList = episodesList.OrderByDescending(el => el.Accuracy);
        //        //completed(true, sortedList.First().YouTubeId);

        //        completed(true, episodesList.First().YouTubeId);
        //    });

        //}

    }
}
