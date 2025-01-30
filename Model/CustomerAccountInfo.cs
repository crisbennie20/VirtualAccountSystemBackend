using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VirtualAccountSystemBackend.Model;

[Table("CustomerAccountInfo")]
public partial class CustomerAccountInfo
{
    [Key]
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? AccountName { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? AccountType { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? ProductType { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Unicode(false)]
    public string? Meta { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? CustomerReference { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? VirtualAccountNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateCreated { get; set; }
}
