using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VirtualAccountSystemBackend.DTO
{
    public class AllVirtualAccountDTO
    {
        public int Id { get; set; }

        public string? PolicyNo { get; set; }

        public string? Channel { get; set; }
        public bool? IsTempAccount { get; set; }

        public string? Bvn { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        public string? AccountName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ProductType { get; set; }

        public string? CustomerReference { get; set; }

        public DateTime? ExpireAt { get; set; }

        public string? SingleDepositLimit { get; set; }

        public DateTime? DateCreated { get; set; }

        public string? VirtualAccountNo { get; set; }
        public string? AccountType { get; set; }


    }
}
