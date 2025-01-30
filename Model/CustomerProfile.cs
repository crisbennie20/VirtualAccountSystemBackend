using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VirtualAccountSystemBackend.Model;

[Table("CustomerProfile")]
public partial class CustomerProfile
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? PolicyNo { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Channel { get; set; }

    [StringLength(11)]
    [Unicode(false)]
    public string? Bvn { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LastName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MiddleName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? AccountName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(13)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? ProductType { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? CustomerReference { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpireAt { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? SingleDepositLimit { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateCreated { get; set; }

    public bool? IsTempAccount { get; set; }
}
