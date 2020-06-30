using ResvoyageMobileApp.Services.Hotel;
using ResvoyageMobileApp.ViewModels.Hotel;
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
    public partial class HotelResultPage : ContentPage
    {
        private HotelResultViewModel _vm;
        private HotelSearchService _hotelSearchService;
        public HotelResultPage(HotelRequestViewModel request)
        {
            InitializeComponent();
            _vm = new HotelResultViewModel(request, null);
            _hotelSearchService = new HotelSearchService();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            waitScreen.IsVisible = true;
            VisibilityResults.IsVisible = false;
            Shell.SetNavBarIsVisible(this, false);
            if (BindingContext == null)
            {
                try
                {
                    var response = await _hotelSearchService.GetHotelResponseAsync(_vm.Request);
                    if (response.Error == null && response.Hotels != null)
                    {
                        BindingContext = new HotelResultViewModel(_vm.Request, response);
                    }
                    else if (response.Error != null && response.Error.ErrorMessage != null)
                    {
                        waitScreen.IsVisible = false;
                        Shell.SetNavBarIsVisible(this, true);
                        await DisplayAlert("Error", response.Error.ErrorMessage, "Ok");
                        await Shell.Current.Navigation.PopAsync(true);
                    }
                }
                catch (Exception e)
                {
                    waitScreen.IsVisible = false;
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