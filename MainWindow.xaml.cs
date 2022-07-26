using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.ServiceModel.Syndication;
using System.Xml;
using System.IO;
using System.Web;
using System.Net.Http;


namespace RSS_Fider
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Worker> workers_l = new ObservableCollection<Worker>();
        ObservableCollection<Instance_Feed> instance_feed = new ObservableCollection<Instance_Feed>();
        int count = 0;

        public MainWindow()
        {
            InitializeComponent();
            //listRss.ItemsSource = workers_l;
            listRss.ItemsSource = instance_feed;
        }
        public void Fid()
        {
            //SyndicationFeed feed = new SyndicationFeed();
            ////Название, которое будет видно в любой читалке
            //feed.Title = new TextSyndicationContent("Мой тестовый RSS фид.");
            ////Блок копирайтов
            //feed.Copyright = new TextSyndicationContent(" 2008");
            ////Описание и название генератора фида.
            //feed.Description = new TextSyndicationContent("Автоматически сгенерированый фид");
            //feed.Generator = "Maxter's RSS Feed Generator";
            ////Ну и ссылка на источник
            //SyndicationLink link = new SyndicationLink();
            //link.Title = "Habr.ru";
            //link.Uri = new Uri("habrahabr.ru");
            //feed.Links.Add(link);

            //SyndicationItem item = new SyndicationItem();

            //item.Id = Guid.NewGuid().ToString();
            //item.Title = new TextSyndicationContent("Привет, ХабраХабр!");
            //item.Summary = new TextSyndicationContent("Тут идет короткое описание");
            //item.Content = new TextSyndicationContent("А тут полный текст");

            ////добавление записи к фиду
            //List<SyndicationItem> items = new List<SyndicationItem>();
            //items.Add(item);
            //feed.Items = items;

            ////Ну а теперь выбираем формат фида и сеарилизуем его. Пускай это будет RSS 2.0:
            //Response.Clear();
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.ContentType = "text / xml";

            //XmlWriter rssWriter = XmlWriter.Create(Response.Output);
            //Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(feed);
            //rssFormatter.WriteTo(rssWriter);
            //rssWriter.Close();

            //Response.End();
        }


        public void Output_Rss( ref List<Instance_Feed> list_rss)
        {
            foreach (Instance_Feed i in list_rss)
            {
                instance_feed.Add(i);
            }
            
        }
        public void Start_Rss_Fider()
        {
            Feed_RSS newsFeedService = new Feed_RSS("https://habr.com/rss/interesting/");
            newsFeedService.GetNewsFeed();
        }
        


        private void button_Click(object sender, RoutedEventArgs e)
        {
            string dfdf= string.Empty;
            String.IsNullOrEmpty(dfdf);
            MessageBox.Show("Арарарар");
        }
       
        private void button_start_rss_fider_Click(object sender, RoutedEventArgs e)
        {   
            Start_Rss_Fider();
            Worker worker = new Worker();
            worker.First_Name = $"Имя{++count}";
            worker.Age = count + 10;
            workers_l.Add(worker);
           

            Title = $"{workers_l.Count}";
        }
        //private void button_start_rss_fider_Click(object sender, RoutedEventArgs e)
        //{
        //    Start_Rss_Fider();
        //    Worker worker = new Worker();
        //    worker.First_Name = $"Имя{++count}";
        //    worker.Age = count + 10;
        //    workers_l.Add(worker);


        //    Title = $"{workers_l.Count}";
        //}
    }
}
