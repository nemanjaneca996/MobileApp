using ResvoyageMobileApp.Models.Flight;
using ResvoyageMobileApp.Resources;
using ResvoyageMobileApp.Views.Flight;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ResvoyageMobileApp.ViewModels.Flight
{
    public class FlightResultViewModel :BaseViewModel
    {
		public FlightResultViewModel(ObservableCollection<PreparedFlightResults> preparedFlightResults, FlightRequestViewModel flightResultViewModel)
		{
			_results = preparedFlightResults;
			_request = flightResultViewModel;
			_filterdResults = preparedFlightResults;
			_filters = new FiltersViewModel(preparedFlightResults);
		}
		private ObservableCollection<PreparedFlightResults> _results;

		public ObservableCollection<PreparedFlightResults>  Results
		{
			get { return _results; }
			set { SetValue(ref _results, value); }
		}
		private ObservableCollection<PreparedFlightResults> _filterdResults;

		public ObservableCollection<PreparedFlightResults> FilterdResults
		{
			get { return _filterdResults; }
			set { SetValue(ref _filterdResults, value); }
		}
		private FlightRequestViewModel _request;

		public FlightRequestViewModel Request
		{
			get { return _request; }
			set { SetValue(ref _request, value); }
		}
		private PreparedFlightResults _selectedFlight;

		public PreparedFlightResults SelectedFlight
		{
			get { return _selectedFlight; }
			set { 
				SetValue(ref _selectedFlight, value);
				
				if (value != null)
				{
					var page = new FlightDetailsPage(value, _request);
					var navigation = Application.Current.MainPage as Shell;
					navigation.Navigation.PushAsync(page, true);
					SelectedFlight = null;

				}

			}
		}
		private FiltersViewModel _filters;

		public FiltersViewModel Filters
		{
			get { return _filters; }
			set { SetValue(ref _filters, value); }
		}
		public ICommand ShowFilters => new Command(DisplayFilters);
		public ICommand ChangeSorting => new Command<SortFlightViewModel>(ChangeFlightSorting);

		private void ChangeFlightSorting(SortFlightViewModel obj)
		{
			Filters.Sort.ForEach(x => x.IsSelected = false);
			obj.IsSelected = true;

			if (obj.Title == AppResources.FF_CHEAPEST)
			{
				FilterdResults = new ObservableCollection<PreparedFlightResults>(FilterdResults.OrderBy(x => x.Total));
			}
			else if (obj.Title == AppResources.FF_QUICKEST)
			{
				FilterdResults = new ObservableCollection<PreparedFlightResults>(FilterdResults.OrderBy(x => x.TotalDuration));
			}
			else if (obj.Title == AppResources.FF_EARLIEST)
			{
				FilterdResults = new ObservableCollection<PreparedFlightResults>(FilterdResults.OrderBy(x => x.FirstDateFrom));
			}
		}

		private void DisplayFilters()
		{
			var page = new FlightFiltersPage(this);
			var navigation = Application.Current.MainPage as Shell;
			navigation.Navigation.PushAsync(page,true);
		}

		public string RequestTextInfo
		{
			get
			{
				var passengers = _request.Adults + _request.Children + _request.Infants;
				return string.Format("{0} {1}, {2} {3}", _request.Cabin.ToString(), AppResources.FR_CABIN, passengers, passengers == 1 ? AppResources.FR_PASSENGER : AppResources.FR_PASSENGERS);
			}
		}

	}
}
