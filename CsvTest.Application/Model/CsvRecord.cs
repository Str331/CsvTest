﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace CsvTest.Application.Model
{
    public class CsvRecord
    {
        [Ignore]
        public int? ID { get; set; }

        [Name("PULocationID")]
        public int PULocationID { get; set; }

        [Name("DOLocationID")]
        public int DOLocationID { get; set; }

        [Name("passenger_count")]
        public int? PassengerCount { get; set; }

        [Name("tip_amount")]
        public decimal TipAmount { get; set; }

        [Name("fare_amount")]
        public decimal FareAmount { get; set; }

        [Name("trip_distance")]
        public decimal TripDistance { get; set; }

        [Name("store_and_fwd_flag")]
        public string StoreFwdFlag { get; set; } = null!;

        [Name("tpep_pickup_datetime")]
        public DateTime TpepPickupDatetime { get; set; }

        [Name("tpep_dropoff_datetime")]
        public DateTime TpepDropOffDatetime { get; set; }
    }
}
