﻿using ResvoyageMobileApp.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResvoyageMobileApp.Views.Hotel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HotelAmenitiesFilterPage : ContentPage
    {
        public HotelAmenitiesFilterPage(FiltersViewModel filter)
        {
            InitializeComponent();
            BindingContext = filter;
        }
    }
}