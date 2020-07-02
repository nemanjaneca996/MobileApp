using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using ResvoyageMobileApp.Models;
using ResvoyageMobileApp.Models.Flight;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ResvoyageMobileApp.Services.Flight
{
    public class AddToCartService : BaseService
    {
        public async Task<ShoppingCartModel> AddToCartAsync(PreparedFlightDetails flightDetails)
        {
            var token = GetToken();
            var restRequest = new RestRequest("/api/v1/cart/air/add", Method.POST);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddJsonBody(GenerateRequest(flightDetails));
            IRestResponse restResut = await _client.ExecuteAsync(restRequest);
            var response = new ShoppingCartModel();
            if (restResut.IsSuccessful && restResut.StatusCode == System.Net.HttpStatusCode.OK)
            {
                response = JsonConvert.DeserializeObject<ShoppingCartModel>(restResut.Content);

                if (response != null && response.ShoppingCart != null && response.ShoppingCart.Air != null)
                    PrepareAir(response);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<ErrorResult>(restResut.Content);
                response.Error = error;
            }
                
            return response;
        }

        private AddToCartModel GenerateRequest(PreparedFlightDetails flightDetails)
        {
            return new AddToCartModel
            {
                SessionId = flightDetails.SessionId,
                ItemId = flightDetails.Id
            };
        }

        private void PrepareAir(ShoppingCartModel response)
        {
            var itinerary = response.ShoppingCart.Air;
            var one = new PreparedFlightResults()
            {
                SessionId = response.SessionId,
                Id = itinerary.Id,
                Total = itinerary.AirItineraryPricingInfo.TotalPrice,
                PriceWithCurrency = itinerary.AirItineraryPricingInfo.CurrencyCode + " " + itinerary.AirItineraryPricingInfo.DisplayTotalPrice,
                ListSegments = new List<FlightSegmentOrganized>(),
                FlightInfo = itinerary
            };
            int i = 0;
            foreach (var sequence in itinerary.AirItinerary.OriginDestinationOptions)
            {
                var flight = new FlightSegmentOrganized()
                {
                    SectrorSequence = sequence.SectorSequence,
                    Cabin = sequence.Cabin,
                    DurationTotal = string.Format("{0}h {1}m", Math.Floor(sequence.JourneyTotalDuration.TotalHours), sequence.JourneyTotalDuration.Minutes)
                };
                one.TotalDuration += sequence.JourneyTotalDuration;
                TimeSpan longest = new TimeSpan();

                foreach (var fly in sequence.FlightSegments)
                {
                    if (fly.Duration > longest)
                    {
                        longest = fly.Duration;
                        flight.AirlineName = fly.MarketingAirlineName;
                        flight.AirlineCode = fly.MarketingAirlineCode;
                        flight.AirlineOpCode = fly.OperatingAirlineCode;
                        flight.AirlineOpName = fly.OperatingAirlineName;
                        flight.RouteNumber = fly.RouteNumber;
                        flight.BookingClass = fly.BookingClass;
                        one.AirlineImage = one.AirlineImage == null ? string.Format("https://engines.resvoyage.com/airline-logos/{0}.gif", flight.AirlineCode) : one.AirlineImage;
                        one.AirlaneName = one.AirlaneName == null ? flight.AirlineName : one.AirlaneName;
                    }

                    if (one.FirstDateFrom == null)
                    {
                        one.FirstDateFrom = fly.DepartureDate;
                    }

                    flight.PlaceFrom = flight.PlaceFrom != null ? flight.PlaceFrom : fly.DepartureAirportName;
                    flight.PlaceTo = fly.ArrivalAirportName;
                    flight.PlaceFromIATA = flight.PlaceFromIATA != null ? flight.PlaceFromIATA : fly.DepartureAirport;
                    flight.PlaceToIATA = fly.ArrivalAirport;
                    flight.DateFrom = flight.DateFrom != null ? flight.DateFrom : fly.DepartureDate;
                    flight.DateTo = fly.ArrivalDate;
                    flight.IATAInfo = string.Format("{0} → {1}", flight.PlaceFromIATA, flight.PlaceToIATA);
                }

                var stops = sequence.FlightSegments.Count - 1;
                flight.Stops = stops;

                if (stops == 0)
                    flight.Transfer = "Non stop";
                else if (stops == 1)
                    flight.Transfer = "1 stop";
                else
                    flight.Transfer = stops + " stops";

                flight.Index = i;
                one.ListSegments.Add(flight);
                i++;
            }
            response.ShoppingCart.PreparedAir = one;
        }
    }
}
