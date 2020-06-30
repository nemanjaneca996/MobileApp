﻿using ResvoyageMobileApp.Models;
using ResvoyageMobileApp.Views.Flight;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ResvoyageMobileApp.ViewModels.Flight
{
    public class OtherInfoViewModel :BaseViewModel
    {
		public OtherInfoViewModel(FlightRequestViewModel flightRequestViewModel)
		{
			_request = flightRequestViewModel;

			_unclickablePlusInflant = _request?.Adults + _request?.Children + _request?.Inflants >= 9 || _request?.Adults == _request?.Inflants;
			_clickablePlusInflant = !_unclickablePlusInflant;
			_unclickableMinusInflant = _request?.Inflants == 0;
			_clickableMinusInflant = !_unclickableMinusInflant;

			_unclickablePlusChildren = _request?.Adults + _request?.Children + _request?.Inflants >= 9;
			_clickablePlusChildren = !_unclickablePlusChildren;
			_unclickableMinusChildren = _request?.Children == 0;
			_clickableMinusChildren = !_unclickableMinusChildren;

			_unclickablePlusAdult = _request?.Adults + _request?.Children + _request?.Inflants >= 9;
			_clickablePlusAdult = !_unclickablePlusAdult;
			_unclickableMinusAdult = _request?.Adults == 1;
			_clickableMinusAdult = !_unclickableMinusAdult;

			_cabin = _request.Cabin.ToString();
			_cabins = GetAvailableCabins(_cabin);

		}
		private FlightRequestViewModel _request;

		public FlightRequestViewModel Request
		{
			get { return _request; }
			set { SetValue(ref _request, value); }
		}
		private List<CabinViewModel> _cabins;

		public List<CabinViewModel> Cabins
		{
			get { return _cabins; }
			set { SetValue(ref _cabins, value); }
		}


		//adult
		private bool _unclickablePlusAdult;
		public bool UnclickablePlusAdult
		{
			get { return _unclickablePlusAdult; }
			set { SetValue(ref _unclickablePlusAdult, value); }
		}
		private bool _clickablePlusAdult;
		public bool ClickablePlusAdult
		{
			get { return _clickablePlusAdult; }
			set { SetValue(ref _clickablePlusAdult, value); }
		}
		private bool _unclickableMinusAdult;
		public bool UnclickableMinusAdult
		{
			get { return _unclickableMinusAdult; }
			set { SetValue(ref _unclickableMinusAdult, value); }
		}
		private bool _clickableMinusAdult;
		public bool ClickableMinusAdult
		{
			get { return _clickableMinusAdult; }
			set { SetValue(ref _clickableMinusAdult, value); }
		}

		//children
		private bool _unclickablePlusChildren;
		public bool UnclickablePlusChildren
		{
			get { return _unclickablePlusChildren; }
			set { SetValue(ref _unclickablePlusChildren, value); }
		}
		private bool _clickablePlusChildren;
		public bool ClickablePlusChildren
		{
			get { return _clickablePlusChildren; }
			set { SetValue(ref _clickablePlusChildren, value); }
		}
		private bool _unclickableMinusChildren;
		public bool UnclickableMinusChildren
		{
			get { return _unclickableMinusChildren; }
			set { SetValue(ref _unclickableMinusChildren, value); }
		}
		private bool _clickableMinusChildren;
		public bool ClickableMinusChildren
		{
			get { return _clickableMinusChildren; }
			set { SetValue(ref _clickableMinusChildren, value); }
		}

		//inflant
		private bool _unclickablePlusInflant;
		public bool UnclickablePlusInflant
		{
			get { return _unclickablePlusInflant; }
			set { SetValue(ref _unclickablePlusInflant, value); }
		}
		private bool _clickablePlusInflant;
		public bool ClickablePlusInflant
		{
			get { return _clickablePlusInflant; }
			set { SetValue(ref _clickablePlusInflant, value); }
		}
		private bool _unclickableMinusInflant;
		public bool UnclickableMinusInflant
		{
			get { return _unclickableMinusInflant; }
			set { SetValue(ref _unclickableMinusInflant, value); }
		}
		private bool _clickableMinusInflant;
		public bool ClickableMinusInflant
		{
			get { return _clickableMinusInflant; }
			set { SetValue(ref _clickableMinusInflant, value); }
		}

		private string _cabin;

		public string Cabin
		{
			get { return _cabin; }
			set { SetValue(ref _cabin, value); }
		}



		#region Commands
		public ICommand ReducePassengerValue => new Command<string>(reducePassengerValue);
		public ICommand AddPassengerValue => new Command<string>(addPassengerValue);
		public ICommand ChangedCabin => new Command<string>(changedCabin); 
		public ICommand ApplyValues => new Command(applyValues);
		#endregion

		private void reducePassengerValue(string passender)
		{
			if (passender == "adult")
			{
				_request.Adults--;
				ReloadButtons();
			}
			else if (passender == "children")
			{
				_request.Children--;
				ReloadButtons();
			}
			else if (passender == "inflant")
			{
				_request.Inflants--;
				ReloadButtons();
			}
		}
		private void addPassengerValue(string passender)
		{
			if (passender == "adult") 
			{
				_request.Adults++;
				ReloadButtons();
			}
			else if (passender == "children")
			{
				_request.Children++;
				ReloadButtons();
			}
			else if (passender == "inflant")
			{
				_request.Inflants++;
				ReloadButtons();
			}
		}
		private void ReloadButtons()
		{
			if (_request?.Adults + _request?.Children + _request?.Inflants >= 9)
			{
				UnclickablePlusAdult = true;
				ClickablePlusAdult = false;
				UnclickablePlusChildren = true;
				ClickablePlusChildren = false;
			}
			else
			{
				UnclickablePlusAdult = false;
				ClickablePlusAdult = true;
				UnclickablePlusChildren = false;
				ClickablePlusChildren = true;
			}

			if (_request?.Adults + _request?.Children + _request?.Inflants >= 9 || _request?.Adults == _request?.Inflants)
			{
				UnclickablePlusInflant = true;
				ClickablePlusInflant = false;
			}
			else
			{
				UnclickablePlusInflant = false;
				ClickablePlusInflant = true;
			}
			if (_request.Adults == 1)
			{
				UnclickableMinusAdult = true;
				ClickableMinusAdult = false;
			}
			else
			{
				UnclickableMinusAdult = false;
				ClickableMinusAdult = true;
			}

			if (_request.Children == 0)
			{
				UnclickableMinusChildren = true;
				ClickableMinusChildren = false;
			}
			else
			{
				UnclickableMinusChildren = false;
				ClickableMinusChildren = true;
			}


			if (_request.Inflants == 0)
			{
				UnclickableMinusInflant = true;
				ClickableMinusInflant = false;
			}
			else
			{
				UnclickableMinusInflant = false;
				ClickableMinusInflant = true;
			}
		}
		private List<CabinViewModel> GetAvailableCabins(string selectedCabin) {
			return new List<CabinViewModel> {
				new CabinViewModel{ IsSelected = selectedCabin == "Economy",Name = "Economy", Type = "Economy" },
				new CabinViewModel{ IsSelected = selectedCabin == "Business",Name = "Business", Type = "Business" },
				new CabinViewModel{ IsSelected = selectedCabin == "First",Name = "First", Type = "First" },
				new CabinViewModel{ IsSelected = selectedCabin == "Premium",Name = "Premium", Type = "Premium" }
			};

		}
		private void changedCabin(string cabin)
		{
			switch (cabin) {
				case "Business":
					_request.Cabin = CabinType.Business;
					break;
				case "Premium":
					_request.Cabin = CabinType.Premium;
					break;
				case "First":
					_request.Cabin = CabinType.First;
					break;
				case "Economy":
					_request.Cabin = CabinType.Economy;
					break;
				default:
					_request.Cabin = CabinType.Economy;
					break;
			}

		}
		private void applyValues() {

			if (!_request.IsFromPopularPlaces)
			{
				_navigation.Navigation.PopAsync(true);
			}
			else
			{
				var page = new FlightResultPage(_request);
				var navigation = Application.Current.MainPage as Shell;
				navigation.Navigation.PushAsync(page);
			}
		}
	}
}
