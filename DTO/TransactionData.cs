namespace VirtualAccountSystemBackend.DTO
{
    public class TransactionData
    {
        public string ? VirtualAccountNo { get; set; }
        public string ? FirstName { get; set; }
        public string ? LastName { get; set; }
        public string ? MiddleName { get; set; }
        public string ? Amount { get; set; }
        public string ? Narration { get; set; }
        public string ? PaymentReference { get; set; }
        public string ? SessionID { get; set; }
        public string? Status { get; set; }

        public DateTime ? DateCreated { get; set; }

    }
}
