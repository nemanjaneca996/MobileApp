using ResvoyageMobileApp.ViewModels.Flight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResvoyageMobileApp.Views.Flight
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlightFiltersPage : ContentPage
    {
        public FlightFiltersPage(FlightResultViewModel results)
        {
            InitializeComponent();
            BindingContext = new FlightFiltersViewModel(results);
        }
    }
}