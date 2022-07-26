using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System.IO;
using System.Text.RegularExpressions;
//Install-Package Microsoft.SyndicationFeed.ReaderWriter -Version 1.0.2
//
namespace RSS_Fider
{
    public class Feed_RSS
    {

        public int Update { get; set; }
        public string Url { get; set; }
        public string Proxy_Ip { get; set; }
        public string Proxy_User { get; set; }
        public string Proxy_Password { get; set; }


        private readonly string Feed_Uri;
        public Feed_RSS()
        {

        }
        public Feed_RSS(string Proxy_Ip, string Proxy_User, string Proxy_Password)
        {
            this.Proxy_Ip = Proxy_Ip;
            this.Proxy_User = Proxy_User;
            this.Proxy_Password = Proxy_Password;
        }
        public Feed_RSS(string Feed_Uri)
        {
            this.Feed_Uri = Feed_Uri;
        }
        public void Initializing_Settings()
        {
            Feed_RSS feed_RSS_setting = new Feed_RSS("dfdf", "dfdf", "dfdf");
            string path = "setting.xml";
            XmlSerializer deser = new XmlSerializer(typeof(Feed_RSS), path);
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                deser.Serialize(stream, feed_RSS_setting);
            }
        }
        private Feed_RSS Deser()
        {

            string path = "setting.xml";
            XmlSerializer deser = new XmlSerializer(typeof(Feed_RSS), path);
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Feed_RSS feed_RSS_setting = deser.Deserialize(stream) as Feed_RSS;
                return feed_RSS_setting;
            }
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        public string SubDiscription(string str)
        {
           string result_str = str.Substring(3,str.IndexOf("<", 3));
           return result_str;
        }   
        public  async Task<List<Instance_Feed>>  GetNewsFeed()
        {
             Feed_RSS feed_RSS_setting = Deser();
            if (feed_RSS_setting.Update == 0) feed_RSS_setting.Update = 1;

            WebProxy wp = new WebProxy(feed_RSS_setting.Proxy_Ip, true);
            wp.Credentials = new NetworkCredential(feed_RSS_setting.Proxy_User, feed_RSS_setting.Proxy_Password);
            WebRequest wrq = WebRequest.Create("http://www.example.com");
            wrq.Proxy = wp;
            //WebResponse wrs = wrq.GetResponse();

            List<Instance_Feed> rssNewsItems = new List<Instance_Feed>();
            MainWindow mainWindow = new MainWindow();
            bool exit = false;
              using (XmlReader xmlReader = XmlReader.Create(Feed_Uri, new XmlReaderSettings() { Async = true }))
                {

                    RssFeedReader feedReader = new RssFeedReader(xmlReader);
                    while (await feedReader.Read())
                    {
                        if (feedReader.ElementType == SyndicationElementType.Item)
                        {
                            Instance_Feed rssItem = new Instance_Feed();
                            ISyndicationItem item = await feedReader.ReadItem();
                            
                            rssItem.Discription_News = StripHTML(item.Description);
                        //  rssItem.Discription_News = SubDiscription(item.Description);
                        rssItem.Title = item.Title;
                            rssItem.Uri = item.Id;
                            rssItem.PublishDate = item.Published;
                            rssNewsItems.Add(rssItem);
                        }
                    }
                return rssNewsItems;
                }
            
                //mainWindow.Output_Rss(ref rssNewsItems);
                 
           
        }
    }
}





//public async Task GetNewsFeed()
//{
//    Feed_RSS feed_RSS_setting = Deser();
//    if (feed_RSS_setting.Update == 0) feed_RSS_setting.Update = 1;

//    WebProxy wp = new WebProxy(feed_RSS_setting.Proxy_Ip, true);
//    wp.Credentials = new NetworkCredential(feed_RSS_setting.Proxy_User, feed_RSS_setting.Proxy_Password);
//    WebRequest wrq = WebRequest.Create("http://www.example.com");
//    wrq.Proxy = wp;
//    //WebResponse wrs = wrq.GetResponse();

//    List<Instance_Feed> rssNewsItems = new List<Instance_Feed>();
//    MainWindow mainWindow = new MainWindow();
//    bool exit = false;
//    while (exit != true)
//    {
//        using (XmlReader xmlReader = XmlReader.Create(Feed_Uri, new XmlReaderSettings() { Async = true }))
//        {

//            RssFeedReader feedReader = new RssFeedReader(xmlReader);
//            while (await feedReader.Read())
//            {
//                if (feedReader.ElementType == SyndicationElementType.Item)
//                {
//                    Instance_Feed rssItem = new Instance_Feed();
//                    ISyndicationItem item = await feedReader.ReadItem();
//                    rssItem.Discription_News = item.Description;
//                    rssItem.Title = item.Title;
//                    rssItem.Uri = item.Id;
//                    rssItem.PublishDate = item.Published;
//                    rssNewsItems.Add(rssItem);
//                }
//            }
//        }
//        mainWindow.Output_Rss(ref rssNewsItems);
//        await Task.Delay(feed_RSS_setting.Update * 3600);
//    }
//}