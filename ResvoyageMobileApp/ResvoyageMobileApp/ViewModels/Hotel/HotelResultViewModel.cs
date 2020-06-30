using ResvoyageMobileApp.Models.Hotel;
using ResvoyageMobileApp.Resources;
using ResvoyageMobileApp.Views.Hotel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ResvoyageMobileApp.ViewModels.Hotel
{
    public class HotelResultViewModel : BaseViewModel
    {
        public HotelResultViewModel(HotelRequestViewModel request, HotelSearchResponse response)
        {
			_request = request;
			if (response != null)
			{
				_sessionId = response.SessionId;
				_filters = new FiltersViewModel(response.Hotels);
				_results = new ObservableCollection<HotelInformation>(response.Hotels);
				_filterdResults = new ObservableCollection<HotelInformation>(response.Hotels);
				_minPrice = Math.Floor(response.MinPrice / request.NumNights);
				_maxPrice = Math.Ceiling(response.MaxPrice / request.NumNights);

				if (response.Hotels != null && response.Hotels.Count > 0)
					_currencyCode = response.Hotels[0].CurrencyCode;
			}			
		}
		private Guid _sessionId;

		public Guid SessionId
		{
			get { return _sessionId; }
			set { SetValue(ref _sessionId, value); }
		}

		private ObservableCollection<HotelInformation> _results;

		public ObservableCollection<HotelInformation> Results
		{
			get { return _results; }
			set { SetValue(ref _results, value); }
		}
		private ObservableCollection<HotelInformation> _filterdResults;

		public ObservableCollection<HotelInformation> FilterdResults
		{
			get { return _filterdResults; }
			set { SetValue(ref _filterdResults, value); }
		}
		private HotelInformation _selectedHotel;

		public HotelInformation SelectedHotel
		{
			get { return _selectedHotel; }
			set
			{
				SetValue(ref _selectedHotel, value);

				if (value != null)
				{
					var page = new HotelDetailsPage(value.Id, _request, _sessionId);
					_navigation.Navigation.PushAsync(page, true);
					SelectedHotel = null;
				}

			}
		}
		private HotelRequestViewModel _request;

		public HotelRequestViewModel Request
		{
			get { return _request; }
			set { SetValue(ref _request, value); }
		}
		public FiltersViewModel _filters;
		public FiltersViewModel Filters
		{
			get { return _filters; }
			set { SetValue(ref _filters, value); }
		}
		private decimal _minPrice;

		public decimal MinPrice
		{
			get { return _minPrice; }
			set { SetValue(ref _minPrice, value); }
		}
		private decimal _maxPrice;

		public decimal MaxPrice
		{
			get { return _maxPrice; }
			set { SetValue(ref _maxPrice, value); }
		}
		private string _currencyCode;

		public string CurrencyCode
		{
			get { return _currencyCode; }
			set { SetValue(ref _currencyCode, value); }
		}
		public string RequestTextInfo
		{
			get
			{
				return string.Format("{0} - {1}, {2}", _request.CheckInDateString, _request.CheckOutDateString, _request.CityName);
			}
		}
		public ICommand ChangeSorting => new Command<CheckboxViewModel>(ChangeHotelSorting); 
		public ICommand ShowFilters => new Command(DisplayFilters);

		private void ChangeHotelSorting(CheckboxViewModel obj)
		{
			Filters.Sort.ForEach(x => x.IsSelected = false);
			obj.IsSelected = true;

			if (obj.Title == AppResources.FF_CHEAPEST)
			{
				FilterdResults = new ObservableCollection<HotelInformation>(FilterdResults.OrderBy(x => x.DailyRatePerRoom));
			}
			else if (obj.Title == AppResources.HF_STAR_RATING)
			{
				FilterdResults = new ObservableCollection<HotelInformation>(FilterdResults.OrderByDescending(x => x.StarRatingDecimal));
			}
			else if (obj.Title == AppResources.HF_HOTEL_NAME)
			{
				FilterdResults = new ObservableCollection<HotelInformation>(FilterdResults.OrderBy(x => x.HotelName));
			}
		}
		private void DisplayFilters()
		{
			var page = new HotelFiltersPage(this);
			_navigation.Navigation.PushAsync(page, true);
		}
	}
}
