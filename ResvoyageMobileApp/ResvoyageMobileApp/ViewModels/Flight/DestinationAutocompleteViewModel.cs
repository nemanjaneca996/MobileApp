using ResvoyageMobileApp.Models;
using ResvoyageMobileApp.Services.Flight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ResvoyageMobileApp.ViewModels.Flight
{
    public class DestinationAutocompleteViewModel : BaseViewModel
    {
		private DestinationAutocompleteService desinationService;
		public DestinationAutocompleteViewModel(FlightRequestViewModel flightRequestView, string requestedType, int? segment = null)
		{
			_request = flightRequestView;
			_type = requestedType;
			_selectedDestination = new AirportInfo();
			desinationService = new DestinationAutocompleteService();
			if (segment != null)
			{
				_multiCitySegment = segment;
			}
		}
		private ObservableCollection<AirportInfo> _results;

		public ObservableCollection<AirportInfo> Results
		{
			get { return _results; }
			set { SetValue(ref _results, value); }
		}

		private FlightRequestViewModel _request;

		public FlightRequestViewModel Request
		{
			get { return _request; }
			set { SetValue(ref _request, value); }
		}
		private string _type;

		public string Type
		{
			get { return _type; }
			set { SetValue(ref _type, value); }
		}
		private int? _multiCitySegment;

		public int? MultiCitySegment
		{
			get { return _multiCitySegment; }
			set { SetValue(ref _multiCitySegment, value); }
		}
		private string _search;

		public string Search
		{
			get { return _search; }
			set {
				if (_search != value)
					getDestinations(value);
				SetValue(ref _search, value); }
		}

		private AirportInfo _selectedDestination;

		public AirportInfo SelectedDestination
		{
			get { return _selectedDestination; }
			set {
				SetValue(ref _selectedDestination, value);
				if (_type == "departure" && (_multiCitySegment == null || _multiCitySegment == 1) && value != null && value.Code != null)
				{
					_request.From1Iata = value.Code;
					_request.From1City = value.City;
				}

				if (_type == "arrival" && (_multiCitySegment == null || _multiCitySegment == 1) && value != null && value.Code != null)
				{
					_request.To1Iata = value.Code;
					_request.To1City = value.City;
				}

				if (_type == "departure" && _multiCitySegment == 2 && value != null && value.Code != null)
				{
					_request.From2Iata = value.Code;
					_request.From2City = value.City;
				}

				if (_type == "arrival" && _multiCitySegment == 2 && value != null && value.Code != null)
				{
					_request.To2Iata = value.Code;
					_request.To2City = value.City;
				}

				if (_type == "departure" && _multiCitySegment == 3 && value != null && value.Code != null)
				{
					_request.From3Iata = value.Code;
					_request.From3City = value.City;
				}

				if (_type == "arrival" && _multiCitySegment == 3 && value != null && value.Code != null)
				{
					_request.To3Iata = value.Code;
					_request.To3City = value.City;
				}
				var navigation = Application.Current.MainPage as Shell;
				navigation.Navigation.PopAsync(true);

			}
		}

		private async void getDestinations(string search)
		{
			try
			{
				var response = await desinationService.GetDestinationAutocompleteResults(search);
				if (response != null && response.Airports != null && response.Airports.Count > 0)
				{
					Results = new ObservableCollection<AirportInfo>(response.Airports as List<AirportInfo>);
				}
				else
				{
					Results = null;
				}
				
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error",e.Message,"Ok");
			}
		}
	}
}
