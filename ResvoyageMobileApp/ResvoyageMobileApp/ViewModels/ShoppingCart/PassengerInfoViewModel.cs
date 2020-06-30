using System;
using System.Collections.Generic;
using System.Text;

namespace ResvoyageMobileApp.ViewModels.ShoppingCart
{
    public class PassengerInfoViewModel : BaseViewModel
    {
		private string _title;

		public string Title
		{
			get { return _title; }
			set { SetValue(ref _title, value); }
		}
		private string _firstName;

		public string FirstName
		{
			get { return _firstName; }
			set { SetValue(ref _firstName, value); }
		}
		private string _middleName;

		public string MiddleName
		{
			get { return _middleName; }
			set { SetValue(ref _middleName, value); }
		}
		private string _lastName;

		public string LastName
		{
			get { return _lastName; }
			set { SetValue(ref _lastName, value); }
		}
		private string _email;

		public string Email
		{
			get { return _email; }
			set { SetValue(ref _email, value); }
		}
		private string _phone;

		public string Phone
		{
			get { return _phone; }
			set { SetValue(ref _phone, value); }
		}
		private string _dateOfBirthString;

		public string DateOfBirthString
		{
			get { return _dateOfBirthString; }
			set { SetValue(ref _dateOfBirthString, value); }
		}
		private string _day;

		public string Day
		{
			get { return _day; }
			set { SetValue(ref _day, value); }
		}
		private string _month;

		public string Month
		{
			get { return _month; }
			set { SetValue(ref _month, value); }
		}
		private string _year;

		public string Year
		{
			get { return _year; }
			set { SetValue(ref _year, value); }
		}
		private string _gender;

		public string Gender
		{
			get { return _gender; }
			set { SetValue(ref _gender, value); }
		}

	}
}
