using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VirtualAccountSystemBackend.Model;

[Table("ApiAccessInfo")]
public partial class ApiAccessInfo
{
    [Key]
    public int Id { get; set; }

    [Unicode(false)]
    public string? PublicKey { get; set; }

    [Unicode(false)]
    public string? MerchantPublicKey { get; set; }

    [Unicode(false)]
    public string? MerchantPrivateKey { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SymmetricKey { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MerchantCode { get; set; }
}
