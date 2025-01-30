using Newtonsoft.Json;

namespace VirtualAccountSystemBackend.DTO
{


	public class Merchant
	{
		public string Code { get; set; }
	}

	public class AccountOpeningRequest
	{

		public string? bvn { get; set; }
		public string? firstName { get; set; }
		public string? lastName { get; set; }

		public string? middleName { get; set; }

		public string? accountName { get; set; }

		public string? email { get; set; }

		public string? phone { get; set; }

		public string? productType { get; set; }

		public string? customerReference { get; set; }
	
		public string? expireAt { get; set; }
	
		public string? singleDepositLimit { get; set; }

		public Merchant merchant { get; set; }
	}

}

