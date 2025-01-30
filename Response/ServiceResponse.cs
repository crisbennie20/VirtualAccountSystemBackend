namespace VirtualAccountSystemBackend.Response
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool? ResponseStatus { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }

    }
}
