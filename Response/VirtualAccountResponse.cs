namespace VirtualAccountSystemBackend.Response
{

  public class VasAccountResponse
    {
        public string AccountName { get; set; }
        public string Type { get; set; }
        public string ProductType { get; set; }
        public int SingleDepositLimit { get; set; }
        public decimal Fee { get; set; }
        public string Status { get; set; }
        public object Meta { get; set; }
        public string CustomerReference { get; set; }
        public string ExpireAt { get; set; }
        public string CreatedAt { get; set; }
        public string VNUBAN { get; set; }
    }
}
