﻿using System;
using System.Collections.Generic;
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

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для TimeControl.xaml
    /// </summary>
    public partial class TimeControl : UserControl
    {
        public TimeControl()
        {
            InitializeComponent();
        }

        private void UIElement_OnTouchDown(object sender, TouchEventArgs e)
        {
            ((Expander) sender).IsExpanded = !((Expander) sender).IsExpanded;
        }
    }
}