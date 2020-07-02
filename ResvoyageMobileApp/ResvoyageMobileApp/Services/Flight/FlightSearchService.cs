﻿using Newtonsoft.Json;
using RestSharp;
using ResvoyageMobileApp.Models;
using ResvoyageMobileApp.Models.Flight;
using ResvoyageMobileApp.ViewModels.Flight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ResvoyageMobileApp.Services.Flight
{
    public class FlightSearchService : BaseService
    {
        public async Task<FlightResponse> GetFlightsResponseAsync(FlightRequestViewModel flightRequestViewModel)
        {
            var token = GetToken();
            var restRequest = new RestRequest("api/v1/air/search?" + PrepareFlightRequestForURL(flightRequestViewModel));
            restRequest.AddHeader("Content-Type", "application/json-patch+json");
            restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "application/json");

            var restResut = await _client.ExecuteAsync(restRequest);
            var response = new FlightResponse();
            if (restResut.IsSuccessful && restResut.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var flights = JsonConvert.DeserializeObject<RestFlightResponse>(restResut.Content);
                var organizedFlights = OrganizeFlightResponse(flights);
                response.FlightList = organizedFlights;
            }
            else
            {
                var error = JsonConvert.DeserializeObject<ErrorResult>(restResut.Content);
                response.Error = error;
            }
            return response;
        }

        public string PrepareFlightRequestForURL(FlightRequestViewModel flightRequestViewModel)
        {
            var newRequest = new FlightRequest
            {
                Adults = flightRequestViewModel.Adults,
                Children = flightRequestViewModel.Children,
                Infants = flightRequestViewModel.Infants,
                FlightClass = flightRequestViewModel.Cabin,
                From1 = flightRequestViewModel.From1Iata,
                To1 = flightRequestViewModel.To1Iata,
                DepartureDate1 = flightRequestViewModel.DepartureDate1                
            };

            if (flightRequestViewModel.SearchType == Models.SearchType.RoundTrip)
            {
                newRequest.From2 = newRequest.To1;
                newRequest.To2 = newRequest.From1;
                newRequest.DepartureDate2 = flightRequestViewModel.DepartureDate2;
            }

            if (flightRequestViewModel.SearchType == Models.SearchType.MultiCity && flightRequestViewModel.MultiCityActieSegments > 1)
            {
                newRequest.From2 = flightRequestViewModel.From2Iata;
                newRequest.To2 = flightRequestViewModel.To2Iata;
                newRequest.DepartureDate2 = flightRequestViewModel.DepartureDate2;

            }
            if (flightRequestViewModel.SearchType == Models.SearchType.MultiCity && flightRequestViewModel.MultiCityActieSegments > 2)
            {
                newRequest.From3 = flightRequestViewModel.From3Iata;
                newRequest.To3 = flightRequestViewModel.To3Iata;
                newRequest.DepartureDate3 = flightRequestViewModel.DepartureDate3;
            }

            return ObjToQueryString(newRequest);
        }

        public ObservableCollection<PreparedFlightResults> OrganizeFlightResponse(RestFlightResponse response)
        {
            var result = new ObservableCollection<PreparedFlightResults>();
            if (response != null && response.PricedItineraries.Count > 0)
            {
                foreach (var itinerary in response.PricedItineraries)
                {
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
                    result.Add(one);
                }
            }

            return result;
        }
    }
}
