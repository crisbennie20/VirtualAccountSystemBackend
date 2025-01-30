namespace VirtualAccountSystemBackend.Response
{
    public class DecryptApiResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public string ProviderResponseCode { get; set; }
        public object Provider { get; set; }
        public object ProviderResponse { get; set; }
        public Error Error { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

}


