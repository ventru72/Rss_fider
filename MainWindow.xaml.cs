﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Diagnostics;



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
            listRss.ItemsSource = feed_l;
        }

       void hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = (sender as Hyperlink).Tag as string;
                Process.Start(path);
            }
            catch 
            {
                MessageBox.Show("Ошибка соединения!");
            }
        }
        private async void button_start_rss_fider_Click(object sender, RoutedEventArgs e)
        {
            Feed_RSS feed_RSS_setting = new Feed_RSS();
            feed_RSS_setting = feed_RSS_setting.Deser();
            bool exit = false;
            try
            {
            Feed_RSS newsFeedService = new Feed_RSS();
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
            catch
            {
                MessageBox.Show("Ошибка соединения! Проверьте настройки прокси сервера!");
            }
        }

    }
}
