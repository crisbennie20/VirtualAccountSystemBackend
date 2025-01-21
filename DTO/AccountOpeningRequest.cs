using Newtonsoft.Json;

namespace VirtualAccountSystemBackend.DTO
{


	public class Merchant
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}

	public class AccountOpeningRequest
	{
		[JsonProperty("bvn")]
		public string? bvn { get; set; }

		[JsonProperty("firstName")]
		public string? firstName { get; set; }

		[JsonProperty("lastName")]
		public string? lastName { get; set; }

		[JsonProperty("middleName")]
		public string? middleName { get; set; }

		[JsonProperty("accountName")]
		public string? accountName { get; set; }

		[JsonProperty("email")]
		public string? email { get; set; }

		[JsonProperty("phone")]
		public string? phone { get; set; }

		[JsonProperty("productType")]
		public string? productType { get; set; }

		[JsonProperty("customerReference")]
		public string? customerReference { get; set; }

		[JsonProperty("expireAt")]
		public string? expireAt { get; set; }

		[JsonProperty("singleDepositLimit")]
		public string? singleDepositLimit { get; set; }

		[JsonProperty("merchant")]
		public Merchant merchant { get; set; }
	}

}

