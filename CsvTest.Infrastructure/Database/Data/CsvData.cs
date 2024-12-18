namespace CsvTest.Infrastructure.Database.Data
{
    public class CsvData
    {
        public int CsvDataID { get; set; }
        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }
        public int PassengerCount { get; set; }

        public decimal TipAmount { get; set; }
        public decimal FareAmount { get; set; }
        public decimal TripDistance { get; set; }

        public string? StoreFwdFlag { get; set; }

        public DateTime TpepPickupDatetime { get; set; }
        public DateTime TpepDropOffDatetime { get; set; }
    }
}
