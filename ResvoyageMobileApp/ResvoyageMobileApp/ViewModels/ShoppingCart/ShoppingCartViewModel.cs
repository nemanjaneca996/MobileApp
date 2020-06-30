using CreditCardValidator;
using ResvoyageMobileApp.Models;
using ResvoyageMobileApp.Models.ShoppingCartModels;
using ResvoyageMobileApp.Services.ShoppingCart;
using ResvoyageMobileApp.ViewModels.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ResvoyageMobileApp.ViewModels.ShoppingCart
{
    public class ShoppingCartViewModel : BaseViewModel
    {
        private BookingCompleteService bookingCompleteService;
        public ShoppingCartViewModel(ShoppingCartModel shoppingCart)
        {
            _shoppingCart = shoppingCart.ShoppingCart;
            _sessionId = shoppingCart.SessionId;
            _passengerInfo = new PassengerInfoViewModel();
            _paymentDetails = new PaymentDetailsViewModel();
            bookingCompleteService = new BookingCompleteService();
        }
        private Guid _sessionId;

        public Guid SessionId
        {
            get { return _sessionId; }
            set { SetValue(ref _sessionId, value); }
        }
        private ShoppingCartResponse _shoppingCart;

        public ShoppingCartResponse ShoppingCart
        {
            get { return _shoppingCart; }
            set { SetValue(ref _shoppingCart, value); }
        }
        private PassengerInfoViewModel _passengerInfo;

        public PassengerInfoViewModel PassengerInfo
        {
            get { return _passengerInfo; }
            set { SetValue(ref _passengerInfo, value); }
        }
        private PaymentDetailsViewModel _paymentDetails;

        public PaymentDetailsViewModel PaymentDetails
        {
            get { return _paymentDetails; }
            set { SetValue(ref _paymentDetails, value); }
        }
        private bool _showWaitScreen;

        public bool ShowWaitScreen
        {
            get { return _showWaitScreen; }
            set { SetValue(ref _showWaitScreen, value); }
        }
        private bool _showPassengerInfo;
        public List<string> PassengerTitle
        {
            get
            {
                return new List<string> { "Mr", "Ms", "Mrs", "Miss", "Mx" };
            }
        }
        public List<PassengerGendreViewModel> PassengerGender
        {
            get
            {
                return new List<PassengerGendreViewModel> { new PassengerGendreViewModel(){ Name = "Male"}, new PassengerGendreViewModel() { Name = "Female" }};
            }
        }
        public List<int> ExpirationMonths
        {
            get 
            {
                var list = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    list.Add(i);
                }
                return list;
            }
        }
        public List<int> ExpirationYears
        {
            get
            {
                var list = new List<int>();
                var year = DateTime.Now.Year;
                for (int i = 0; i <= 14; i++)
                {
                    list.Add(year+i);
                }
                return list;
            }
        }
        public List<int> Years
        {
            get
            {
                var list = new List<int>();
                var year = DateTime.Now.Year;
                for (int i = year; i >= 1910; i--)
                {
                    list.Add(i);
                }
                return list;
            }
        }
        public List<int> Months
        {
            get
            {
                var list = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    list.Add(i);
                }
                return list;
            }
        }
        public List<int> Days
        {
            get
            {
                var list = new List<int>();
                for (int i = 1; i <= 31; i++)
                {
                    list.Add(i);
                }
                return list;
            }
        }
        public  string PayText
        {
            get
            {
                if (ShoppingCart != null && ShoppingCart.TotalPrice != 0)
                    return string.Format("Pay {0}", ShoppingCart.TotalPrice);
                else
                    return null;
            }
        }
        public ICommand Pay => new Command(BookingComplete); 
        public ICommand ChangedGender => new Command<string>(changedGender);

        private void changedGender(string selectedGender)
        {
            PassengerInfo.Gender = selectedGender;
        }

        private async void BookingComplete()
        {
            try
            {
                ShowWaitScreen = true;
                var response = await bookingCompleteService.BookAsync(GenerateRequest());
                ShowWaitScreen = false;
                if (response != null && response.ErrorResult != null && response.ErrorResult.ErrorMessage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error ", response.ErrorResult.ErrorMessage, "Ok");
                }
                else if (response != null && response.IsSuccessful)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Successfully booked. Your PNR:" + response.ReferenceNumber, "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error ", "Some error occurred", "Ok");
                }

            }
            catch (Exception e)
            {
                ShowWaitScreen = false;
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
            }
        }

        private BookingCompleteRequest GenerateRequest()
        {
            var request = new BookingCompleteRequest();
            request.Travellers = new List<PassengerInfo>();
            request.SessionId = _sessionId.ToString();
            var traveller = new PassengerInfo
            {
                Email = _passengerInfo.Email,
                DateOfBirthString = string.Format("{0}-{1}-{2}", _passengerInfo.Year, _passengerInfo.Month, _passengerInfo.Day),
                FirstName = _passengerInfo.FirstName,
                MiddleName = _passengerInfo.MiddleName,
                LastName = _passengerInfo.LastName,
                Phone = _passengerInfo.Phone,
                Title = _passengerInfo.Title,
                Gender = _passengerInfo.Gender == "Male",
                TypeCode = "ADT"
            };
            request.Travellers.Add(traveller);
            request.PaymentDetails = new List<PaymentInfoPerProductWise>();

            var payment = new PaymentInfoPerProductWise { 
                TravelType="AIR",
                PaymentOption = "CreditCard",
                Address = new AddressInfo { 
                    CityName = _paymentDetails.AddressInfo.CityName,
                    CountryCode = _paymentDetails.AddressInfo.CountryName,
                    CountryName = _paymentDetails.AddressInfo.CountryName,
                    RegionName = _paymentDetails.AddressInfo.RegionName,
                    StreetAddress = _paymentDetails.AddressInfo.StreetAddress,
                    ZIP = _paymentDetails.AddressInfo.ZIP
                },
                CardInfo = new PaymentCardInfo { 
                    CardholderName = _paymentDetails.CardInfo.CardholderName,
                    CardType = GetCardType(),
                    CardTypeCode = GetCardType(),
                    ExpirationMonth = _paymentDetails.CardInfo.ExpirationMonth,
                    ExpirationYear = _paymentDetails.CardInfo.ExpirationYear,
                    CVV = _paymentDetails.CardInfo.CVV,
                    CardNumber = _paymentDetails.CardInfo.CardNumber                    
                }
            };
            request.PaymentDetails.Add(payment);

            return request;
        }

        private string GetCardType()
        {
            if (PaymentDetails != null && PaymentDetails.CardInfo != null && PaymentDetails.CardInfo.CardNumber != null)
            {
                CreditCardDetector detector = new CreditCardDetector(PaymentDetails.CardInfo.CardNumber);
                if (detector.IsValid())
                {
                    switch (detector.Brand)
                    {
                        case CardIssuer.AmericanExpress:
                            return "AX";
                        case CardIssuer.MasterCard:
                            return "MC";
                        case CardIssuer.Visa:
                            return "VI";
                        case CardIssuer.Discover:
                            return "DS";
                        case CardIssuer.DinersClub:
                            return "DN";
                    }
                }
            }
            return null;
        }
    }
}
