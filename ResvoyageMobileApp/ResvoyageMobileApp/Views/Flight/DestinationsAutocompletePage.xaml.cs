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
    public partial class DestinationsAutocompletePage : ContentPage
    {
        public DestinationsAutocompletePage(FlightRequestViewModel request, string type, int? segment=null)
        {
            InitializeComponent();
            BindingContext = new DestinationAutocompleteViewModel(request, type, segment);
            if (type == "departure")
            {
                DepartureEntry.IsVisible = true;
                DepartureEntry.Focus();
                ArrivalEntry.IsVisible = false;
            }
            if (type == "arrival")
            {
                DepartureEntry.IsVisible = false;
                ArrivalEntry.IsVisible = true;
                ArrivalEntry.Focus();
            }
        }

    }
}