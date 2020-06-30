﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ResvoyageMobileApp.Models.Flight
{
    public class FlightResponse
    {
        public ObservableCollection<PreparedFlightResults> FlightList { get; set; }
        public ErrorResult Error { get; set; }
    }
}
