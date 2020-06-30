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
    public partial class FlightSearchPage : TabbedPage
    {
        private FlightSearchViewModel _vm = new FlightSearchViewModel();
        public FlightSearchPage()
        {
            InitializeComponent();
            OneWay.BindingContext = new FlightSearchViewModel() { SearchType = "OneWay"};
            RoundTrip.BindingContext = new FlightSearchViewModel() { SearchType = "RoundTrip" };
            MultiCity.BindingContext = new FlightSearchViewModel() { SearchType = "MultiCity" };
        }
        public FlightSearchPage(FlightRequestViewModel requestViewModel)
        {
            InitializeComponent();
            OneWay.BindingContext = new FlightSearchViewModel(requestViewModel) { SearchType = "OneWay" };
            RoundTrip.BindingContext = new FlightSearchViewModel(requestViewModel) { SearchType = "RoundTrip" };
            MultiCity.BindingContext = new FlightSearchViewModel(requestViewModel) { SearchType = "MultiCity" };
        }
    }
}