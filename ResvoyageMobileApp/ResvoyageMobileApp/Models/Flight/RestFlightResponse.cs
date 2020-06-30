using System;
using System.Collections.Generic;
using System.Text;

namespace ResvoyageMobileApp.Models.Flight
{
    public class RestFlightResponse
    {
        public List<PricedItinerary> PricedItineraries { get; set; }
        public Guid SessionId { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MinBookedCabinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public AirMatrix Matrix { get; set; }
        public List<FlexibleFlight> FlexibleOutboundFlights { get; set; }
        public bool DisplayPriceForOnePassenger { get; set; }
        public ErrorResult Error { get; set; }
    }

    public class FlexibleFlight
    {
        public DateTime DepartureDate { get; set; }
        public List<AirlineMatrix> FlexibleInboundFlights { get; set; }
    }

    public class PricedItinerary
    {
        public AirItinerary_REST AirItinerary { get; set; }
        public AirItineraryPricingInfo AirItineraryPricingInfo { get; set; }
        public Guid Id { get; set; }
        public string PolicyType { get; set; }
        public string DeepLinkUrl { get; set; }
        public string Provider { get; set; }
        public string OfficeId { get; set; }
    }

    public class AirItineraryPricingInfo
    {
        public decimal TotalPrice { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Tax { get; set; }
        public decimal Markup { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Discount { get; set; }
        public decimal PromotionalDiscount { get; set; }
        public string CurrencyCode { get; set; }
        public List<PriceFeeDetail> PTC_FareBreakdowns { get; set; }
        public string PricingSource { get; set; }
        public bool IsNegotiatedPrice { get; set; }
        public string FareType { get; set; }
        public string FareFamily { get; set; }
        public string ValidatingAirlineCode { get; set; }
    }

    public class PriceFeeDetail
    {
        public string PTCIdentifier { get; set; }
        public List<BaggageInfo> Baggages { get; set; }
        public decimal BasePriceFromItinerary { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Markup { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal DiscountAmountFromContract { get; set; }
        public decimal PromotionalDiscount { get; set; }
        public decimal TotalPrice
        {
            get { return (BasePrice + Markup + Discount + Tax + PromotionalDiscount + DiscountAmountFromContract); }
        }
        public int PassengerCount { get; set; }
        public string PassengerType { get; set; }
        public string CodeContext { get; set; }
        public List<MarkupBreakdown> MarkupBreakdown { get; set; }
        public List<MarkupBreakdown> DiscountBreakdown { get; set; }
        public List<MarkupBreakdown> PromotionalDiscountBreakdown { get; set; }
        public MarkupBreakdown ContractManagerDiscountBreakdown { get; set; }
        public List<string> FareBasisCodes { get; set; }
    }
    public class BaggageInfo
    {
        public int SequenceNumber { get; set; }
        public string FlightNumber { get; set; }
        public decimal FreeQuantity { get; set; }
        public string Id { get; set; }
        public string PassengerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public BagAllowanceType BagAllowanceType { get; set; }
        public BagAllowanceUnit BagAllowanceUnit { get; set; }


    }
    public class MarkupBreakdown
    {
        public decimal MarkupAmount { get; set; }
        public string MarkupName { get; set; }
        public bool Display { get; set; }
        public int Id { get; set; }
    }

    public class AirItinerary_REST
    {
        public string DirectionInd { get; set; }
        public List<OriginDestinationOption> OriginDestinationOptions { get; set; }
    }

    public class OriginDestinationOption
    {
        public int SectorSequence { get; set; }
        public List<FlightSegment> FlightSegments { get; set; }
        public string Cabin { get; set; }
        public TimeSpan JourneyTotalDuration { get; set; }
    }

    public class FlightSegment
    {
        public string RouteNumber { get; set; }
        public string FlightNumber { get; set; }
        public string MarketingAirlineCode { get; set; }
        public string MarketingAirlineName { get; set; }
        public string OperatingAirlineName { get; set; }
        public string OperatingAirlineCode { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string DepartureAirport { get; set; }
        public string DepartureCountryCode { get; set; }
        public string ArrivalAirport { get; set; }
        public string ArrivalCountryCode { get; set; }
        public string Aircraft { get; set; }
        public string BookingClass { get; set; }
        public string ArrivalAirportName { get; set; }
        public string DepartureAirportName { get; set; }
        public string BaggageFeeUrl { get; set; }
        public List<Baggage> FreeBaggages { get; set; }
        public string Cabin { get; set; }
        public int? SeatsLeft { get; set; }
    }

    public class Baggage
    {
        public decimal FreeQuantity { get; set; }
        public BagAllowanceType BagAllowanceType { get; set; }
        public BagAllowanceUnit BagAllowanceUnit { get; set; }
    }
    public enum BagAllowanceType
    {
        Piece,
        Weight
    }

    public enum BagAllowanceUnit
    {
        K,
        L
    }


    public class AirMatrix
    {
        private List<AirlineMatrix> _directAirlines = new List<AirlineMatrix>();
        private List<AirlineMatrix> _oneStopAirlines = new List<AirlineMatrix>();
        private List<AirlineMatrix> _twoStopsAirlines = new List<AirlineMatrix>();

        public List<AirlineMatrix> DirectAirlines
        {
            get { return _directAirlines; }
            set { _directAirlines = value; }
        }

        public List<AirlineMatrix> OneStopAirlines
        {
            get { return _oneStopAirlines; }
            set { _oneStopAirlines = value; }
        }

        public List<AirlineMatrix> TwoStopsAirlines
        {
            get { return _twoStopsAirlines; }
            set { _twoStopsAirlines = value; }
        }
    }

    public class AirlineMatrix
    {
        public string AirlineCode { get; set; }
        public string Airline { get; set; }
        public DateTime Departure { get; set; }
        public bool HasMultipleAirlines { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
