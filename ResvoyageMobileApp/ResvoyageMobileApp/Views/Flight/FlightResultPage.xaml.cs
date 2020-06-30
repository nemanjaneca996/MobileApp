using ResvoyageMobileApp.Services.Flight;
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
    public partial class FlightResultPage : ContentPage
    {
        private FlightResultViewModel _vm;
        public FlightResultPage(FlightRequestViewModel request)
        {           
            InitializeComponent();
            _vm = new FlightResultViewModel(null, request);
            _vm.Request = request;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            FlightSearchService searchService = new FlightSearchService();
            VisibilityResults.IsVisible = false;
            waitScreen.IsVisible = true;
            Shell.SetNavBarIsVisible(this, false);
            if (BindingContext == null)
            {
                try
                {
                    var response = await searchService.GetFlightsResponseAsync(_vm.Request);
                    if (response.Error == null && response.FlightList != null)
                    {
                        BindingContext = new FlightResultViewModel(response.FlightList, _vm.Request);
                    }
                    else if (response.Error != null && response.Error.ErrorMessage != null)
                    {
                        waitScreen.IsVisible = false;
                        VisibilityResults.IsVisible = true;
                        Shell.SetNavBarIsVisible(this, true);
                        await DisplayAlert("Error", response.Error.ErrorMessage, "Ok");

                        await Shell.Current.Navigation.PopAsync(true);
                    }
                }
                catch (Exception e)
                {
                    waitScreen.IsVisible = false;
                    VisibilityResults.IsVisible = true;
                    Shell.SetNavBarIsVisible(this, true);
                    await DisplayAlert("Error", e.Message, "Ok");

                    await Shell.Current.Navigation.PopAsync(true);
                }
                
            }
            waitScreen.IsVisible = false;
            VisibilityResults.IsVisible = true;
            Shell.SetNavBarIsVisible(this, true);

        } 
    }
}