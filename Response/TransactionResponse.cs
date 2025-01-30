using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VirtualAccountSystemBackend.Response
{
    public class TransactionResponse
    {
      
        public string? SessionId { get; set; }

        
        public string? NameEnquiryRef { get; set; }

        public string? DestinationInstitutionCode { get; set; }

        public string? ChannelCode { get; set; }

        public string? BeneficiaryAccountName { get; set; }

        public string? BeneficiaryAccountNumber { get; set; }

       
        public string? BeneficiaryBankVerificationNumber { get; set; }

        public string? BeneficiaryKyclevel { get; set; }

        public string? OriginatorAccountName { get; set; }

        public string? OriginatorAccountNumber { get; set; }

        public string? OriginatorBankVerificationNumber { get; set; }

        public string? OriginatorKyclevel { get; set; }

        public string? TransactionLocation { get; set; }

        public string? Narration { get; set; }

        public string? PaymentReference { get; set; }
        public string? Status { get; set; }

        public string? Amount { get; set; }

        public string? CollectionAccountNumber { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
