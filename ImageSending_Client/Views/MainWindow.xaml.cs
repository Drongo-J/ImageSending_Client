﻿using ImageSending_Client.ViewModels;
using ImageSending_Client.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageSending_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.MyGrid = MyGrid;
            var dragDropImageUC = new DragDropImageUC();
            var drapDropImageUCVM = new DragDropImageUCViewModel();
            dragDropImageUC.DataContext = drapDropImageUCVM;
            App.ChangePage(dragDropImageUC);
        }
    }
}
