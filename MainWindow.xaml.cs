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
        ObservableCollection<Instance_Feed> feed_l = new ObservableCollection<Instance_Feed>();
        int count = 0;

        public MainWindow()
        {
            InitializeComponent();
            //listRss.ItemsSource = workers_l;
            listRss.ItemsSource = feed_l;

        }
       

        public void Output_Rss( ref List<Instance_Feed> list_rss)
        {
            foreach (Instance_Feed i in list_rss)
            {
                feed_l.Add(i);
            }
            
        }
       
        
       
        private async void button_start_rss_fider_Click(object sender, RoutedEventArgs e)
        {
            Feed_RSS newsFeedService = new Feed_RSS("https://habr.com/rss/interesting/");
            var result = await newsFeedService.GetNewsFeed();

             foreach (var i in result)
            {
                feed_l.Add(i);
            }

            int fdf = feed_l.Count;
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
