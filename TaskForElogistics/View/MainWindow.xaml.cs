using System;
using System.Windows;
using TaskForElogistics.ViewModel;
using Telerik.Windows.Controls;
using TaskForElogistics.Service;


namespace TaskForElogistics.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            StyleManager.ApplicationTheme = new FluentTheme();
            InitializeComponent();
            DataContext = new GetRequestVM(new DialogService(), new JsonFileService());
        }
       
    }
}
