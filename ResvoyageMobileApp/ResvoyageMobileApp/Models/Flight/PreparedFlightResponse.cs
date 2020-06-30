using System;
using System.Collections.Generic;
using System.Text;

namespace ResvoyageMobileApp.Models.Flight
{
    public class PreparedFlightResults
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public decimal Total { get; set; }
        public string PriceWithCurrency { get; set; }
        public List<FlightSegmentOrganized> ListSegments { get; set; }
        public string AirlineImage { get; set; }
        public string AirlaneName { get; set; }
        public PricedItinerary FlightInfo { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public DateTime? FirstDateFrom { get; set; }

    }

    public class FlightSegmentOrganized
    {
        public int Index { get; set; }
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        public string AirlineOpCode { get; set; }
        public string AirlineOpName { get; set; }
        public string BookingClass { get; set; }
        public string Cabin { get; set; }
        public string DurationTotal { get; set; }
        public string FlightNumber { get; set; }
        public string PlaceFrom { get; set; }
        public string PlaceTo { get; set; }
        public string PlaceFromIATA { get; set; }
        public string PlaceToIATA { get; set; }
        public string IATAInfo { get; set; }
        public string RouteNumber { get; set; }
        public int SectrorSequence { get; set; }
        public string Transfer { get; set; }
        public int Stops { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string TimeFrom
        {
            get
            {
                return DateFrom?.ToString("HH:mm") ?? "";
            }
        }
        public string TimeTo
        {
            get
            {
                return DateTo?.ToString("HH:mm") ?? "";
            }
        }
        public string TimeInfo
        {
            get
            {
                return string.Format("{0} - {1}", TimeFrom, TimeTo);
            }
        }
    }
}
