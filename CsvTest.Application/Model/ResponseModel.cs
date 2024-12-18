namespace CsvTest.Application.Model
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Result { get; set; }
    }

    public class ResponseModel
    {
        public bool Success { get; set; }

        public string? Message { get; set; }
    }
}
