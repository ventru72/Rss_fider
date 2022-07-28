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
using System.Diagnostics;
using System.Net;


namespace RSS_Fider
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Worker> workers_l = new ObservableCollection<Worker>();
        ObservableCollection<Instance_Feed> feed_l = new ObservableCollection<Instance_Feed>();
        int count = 0;
        public StringBuilder sb = new StringBuilder(" ");
        public string link = string.Empty;
        public MainWindow()
        {
           
            InitializeComponent();
            //listRss.ItemsSource = workers_l;
            listRss.ItemsSource = feed_l;
            
        }


       void hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            //  var b = sender as Button;
            //  b.IsEnabled = false;
                //Feed_RSS feed_RSS_setting = new Feed_RSS();
                //feed_RSS_setting = feed_RSS_setting.Deser();

                //WebProxy wp = new WebProxy(feed_RSS_setting.Proxy_Ip, true);
                //wp.Credentials = new NetworkCredential(feed_RSS_setting.Proxy_User, feed_RSS_setting.Proxy_Password);
                string path = (sender as Hyperlink).Tag as string;
                //WebRequest wrq = WebRequest.Create(path);
                //wrq.Proxy = wp;
                //WebResponse wrs = wrq.GetResponse();
                
                Process.Start(path);
                //b.IsEnabled = true;
            }
            catch 
            {
                MessageBox.Show("Ошибка соединения, проверьте настройки прокси сервера.");
            }

          // Process.Start(path); //открытие ссылки в браузере
        }
        private async void button_start_rss_fider_Click(object sender, RoutedEventArgs e)
        {
            Feed_RSS feed_RSS_setting = new Feed_RSS();
            feed_RSS_setting = feed_RSS_setting.Deser();
            bool exit = false;


            Feed_RSS newsFeedService = new Feed_RSS("https://habr.com/rss/interesting/");
            while (exit != true)
            {
                var result = await newsFeedService.GetNewsFeed();

                foreach (var i in result)
                {
                    feed_l.Add(i);
                    link = i.Uri;
                }

                int fdf = feed_l.Count;

               
               
                await Task.Delay(feed_RSS_setting.Update*3600);
                feed_l.Clear();
                ++count;
                Title = $"Количество апдейтов = {count}";
            }
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
