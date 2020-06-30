using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ResvoyageMobileApp.ViewModels.Hotel
{
    public class HotelCalendarViewModel : BaseViewModel
    {
        public HotelCalendarViewModel(HotelRequestViewModel request)
        {
            _request = request;
            var startDate = _request.CheckInDate;
            var endDate = _request.CheckOutDate;
            DateTime dateStart;
            DateTime dateEnd;
            _selectedRange = new SelectionRange();

            if (DateTime.TryParse(startDate, out dateStart))
            {
                _selectedRange.StartDate = dateStart;
            }

            if (DateTime.TryParse(endDate, out dateEnd))
            {
                _selectedRange.EndDate = dateEnd;
            }
            else
            {
                _selectedRange.EndDate = _selectedRange.StartDate;
            }

        }
        private HotelRequestViewModel _request;

        public HotelRequestViewModel Request
        {
            get { return _request; }
            set { SetValue(ref _request, value); }
        }
        private SelectionRange _selectedRange;

        public SelectionRange SelectedRange
        {
            get { return _selectedRange; }
            set { _selectedRange = value; }
        }
        public ICommand CalendarCellTapped => new Command(SetDateValues);

        private void SetDateValues()
        {
            if (SelectedRange.EndDate != SelectedRange.StartDate)
            {
                _request.CheckInDate = SelectedRange.StartDate.ToString("yyyy-MM-dd");
                _request.CheckInDateString = SelectedRange.StartDate.ToString("dd MMM");
                _request.CheckInDateDayString = SelectedRange.StartDate.ToString("ddd");
                _request.CheckOutDate = SelectedRange.EndDate.ToString("yyyy-MM-dd");
                _request.CheckOutDateString = SelectedRange.EndDate.ToString("dd MMM");
                _request.CheckOutDateDayString = SelectedRange.EndDate.ToString("ddd");

                _request.NumNights = (SelectedRange.EndDate - SelectedRange.StartDate).Days;
                var navigation = Application.Current.MainPage as Shell;
                navigation.Navigation.PopAsync(true);
            }
            else
            {
                _request.CheckInDate = SelectedRange.StartDate.ToString("yyyy-MM-dd");
                _request.CheckInDateString = SelectedRange.StartDate.ToString("dd MMM");
                _request.CheckInDateDayString = SelectedRange.StartDate.ToString("ddd");
                _request.CheckOutDate = null;
                _request.CheckOutDateString = null;
                _request.CheckOutDateDayString = null;
            }
        }
    }
}
