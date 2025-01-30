using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VirtualAccountSystemBackend.Model;

[Table("CustomerTransactions")]
public partial class CustomerTransaction1
{
    [Key]
    public int Id { get; set; }

    [Column("SessionID")]
    [StringLength(50)]
    [Unicode(false)]
    public string? SessionId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? NameEnquiryRef { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? DestinationInstitutionCode { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? ChannelCode { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? BeneficiaryAccountName { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? BeneficiaryAccountNumber { get; set; }

    [StringLength(11)]
    [Unicode(false)]
    public string? BeneficiaryBankVerificationNumber { get; set; }

    [Column("BeneficiaryKYCLevel")]
    [StringLength(2)]
    [Unicode(false)]
    public string? BeneficiaryKyclevel { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? OriginatorAccountName { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? OriginatorAccountNumber { get; set; }

    [StringLength(11)]
    [Unicode(false)]
    public string? OriginatorBankVerificationNumber { get; set; }

    [Column("OriginatorKYCLevel")]
    [StringLength(2)]
    [Unicode(false)]
    public string? OriginatorKyclevel { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TransactionLocation { get; set; }

    [Unicode(false)]
    public string? Narration { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PaymentReference { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Amount { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? CollectionAccountNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateCreated { get; set; }
}
