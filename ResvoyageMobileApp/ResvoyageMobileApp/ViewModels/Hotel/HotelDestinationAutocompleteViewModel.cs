using ResvoyageMobileApp.Models.Hotel;
using ResvoyageMobileApp.Services.Hotel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ResvoyageMobileApp.ViewModels.Hotel
{
    public class HotelDestinationAutocompleteViewModel : BaseViewModel
    {
        private HotelDestinationService destinationService;
        public HotelDestinationAutocompleteViewModel(HotelRequestViewModel request)
        {
            _request = request;
            destinationService = new HotelDestinationService();
        }
        private HotelRequestViewModel _request;

        public HotelRequestViewModel Request
        {
            get { return _request; }
            set { SetValue(ref _request, value); }
        }
        private string _search;
        public string Search
        {
            get { return _search; }
            set
            {
                if (_search != value)
                    getDestinations(value);
                SetValue(ref _search, value);
            }
        }
        private ObservableCollection<HotelCityData> _results;

        public ObservableCollection<HotelCityData> Results
        {
            get { return _results; }
            set { SetValue(ref _results, value); }
        }
        private HotelCityData _selectedDestination;

        public HotelCityData SelectedDestination
        {
            get { return _selectedDestination; }
            set
            {
                SetValue(ref _selectedDestination, value);
                if (value != null)
                {
                    _request.CityName = value.Name;
                    _request.PlaceId = value.PlaceId;
                    _request.HotelCityCode = value.Code;
                    var navigation = Application.Current.MainPage as Shell;
                    navigation.Navigation.PopAsync(true);
                }
            }
        }

        private async void getDestinations(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var response = await destinationService.GetDestinationAutocompleteResults(value);
                    if (response != null && response.Resutls != null && response.Resutls.Count > 0)
                    {
                        var validDestinations = new List<HotelCityData>();

                        foreach (var destination in response.Resutls)
                        {
                            if (!string.IsNullOrEmpty(destination.Name))
                                validDestinations.Add(destination);
                        }

                        if (validDestinations?.Count > 0)
                            Results = new ObservableCollection<HotelCityData>(validDestinations as List<HotelCityData>);
                        else
                            Results = null;
                    }
                    else
                    {
                        Results = null;
                    }
                    
                }
                else
                {
                    Results = new ObservableCollection<HotelCityData>();
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
            }
        }
    }
}
