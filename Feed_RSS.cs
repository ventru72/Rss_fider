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
using HtmlTextBlock;
//Install-Package Microsoft.SyndicationFeed.ReaderWriter -Version 1.0.2
//
namespace RSS_Fider
{
    public class Feed_RSS
    {
        public int Update { get; set; }
        public string Feed_Uri { get; set; }
        public string Proxy_Ip { get; set; }
        public string Proxy_User { get; set; }
        public string Proxy_Password { get; set; }
        public int Proxy_Port { get; set; }

       //private readonly string Feed_Uri;
        public Feed_RSS() {}
        public Feed_RSS(string Url, string Proxy_Ip, string Proxy_User, string Proxy_Password, int Proxy_Port)
        {
            this.Feed_Uri = Url;
            this.Proxy_Ip = Proxy_Ip;
            this.Proxy_User = Proxy_User;
            this.Proxy_Password = Proxy_Password;
            this.Proxy_Port = Proxy_Port;
        }
        public Feed_RSS(string Feed_Uri)
        {
            this.Feed_Uri = Feed_Uri;
        }
        //метод создания основы для файла настроек
        public void Initializing_Settings()
        {
            Feed_RSS feed_RSS_setting = new Feed_RSS();
            string path = "setting.xml";
            XmlSerializer deser = new XmlSerializer(typeof(Feed_RSS), path);
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                deser.Serialize(stream, feed_RSS_setting);
            }
        }
        //метод создания чтения файла настроек
        public Feed_RSS Deser()
        {

            string path = "setting.xml";
            XmlSerializer deser = new XmlSerializer(typeof(Feed_RSS), path);
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Feed_RSS feed_RSS_setting = deser.Deserialize(stream) as Feed_RSS;
                return feed_RSS_setting;
            }
        }
        //метод убирания HTML тегов
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
       /// <summary>
       /// чтение RSS, основной метод
       /// </summary>
       /// <returns></returns>
        public  async Task<List<Instance_Feed>>  GetNewsFeed()
        {
             Feed_RSS feed_RSS_setting = Deser();
            if (feed_RSS_setting.Update == 0) feed_RSS_setting.Update = 1;

            List<Instance_Feed> rssNewsItems = new List<Instance_Feed>();
            //проверяем есть установлен прокси или нет
            if (feed_RSS_setting.Proxy_Ip != string.Empty)
            {
                var webProxy = new WebProxy(feed_RSS_setting.Proxy_Ip);
                webProxy.Credentials = new NetworkCredential(feed_RSS_setting.Proxy_User, feed_RSS_setting.Proxy_Password);

           
                string feedString;

                using (var webClient = new WebClient())
            {
                webClient.Proxy = webProxy;
                webClient.Encoding = System.Text.Encoding.UTF8;
               
                feedString = webClient.DownloadString(feed_RSS_setting.Feed_Uri);
            }
            var stringReader = new StringReader(feedString);
               await Feed_Proxy(stringReader);
            }
            else
            { 
                string stringReader = feed_RSS_setting.Feed_Uri;
                await Feed_No_Proxy(stringReader);
            }
            // метод фид - логика выполнения
            async Task<List<Instance_Feed>> Feed(XmlReader xmlReader)
            {

                RssFeedReader feedReader = new RssFeedReader(xmlReader);
                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == SyndicationElementType.Item)
                    {
                        Instance_Feed rssItem = new Instance_Feed();
                        ISyndicationItem item = await feedReader.ReadItem();
                       
                       
                        rssItem.Discription_News = StripHTML(item.Description).TrimStart('\r','\n');
                        //  rssItem.Discription_News = SubDiscription(item.Description);
                        rssItem.Title = item.Title;
                        rssItem.Uri = item.Id;
                        rssItem.PublishDate = item.Published;
                        rssNewsItems.Add(rssItem);
                    }
                }
                return rssNewsItems;
            }
            // использование фида с прокси
            async Task<List<Instance_Feed>> Feed_Proxy(StringReader strReader)
            {
                using (XmlReader xmlReader = XmlReader.Create(strReader, new XmlReaderSettings() { Async = true }))
                {
                    await Feed(xmlReader);
                    return rssNewsItems;
                }
            }

            // использование фида без прокси
            async Task<List<Instance_Feed>> Feed_No_Proxy(string strReader)
            {
                using (XmlReader xmlReader = XmlReader.Create(strReader, new XmlReaderSettings() { Async = true }))
                {
                    await Feed(xmlReader);
                    return rssNewsItems;
                }
            }
            return rssNewsItems;

        }
    }
}




