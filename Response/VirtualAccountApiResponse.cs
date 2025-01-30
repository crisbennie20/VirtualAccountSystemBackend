using Newtonsoft.Json;

public class VirtualAccountApiResponse
{
    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    public ResponseData Data { get; set; }
}

public class ResponseData
{
    [JsonProperty("provider_response_code")]
    public string ProviderResponseCode { get; set; }

    [JsonProperty("provider")]
    public object Provider { get; set; }

    [JsonProperty("provider_response")]
    public ProviderResponse ProviderResponse { get; set; }

    [JsonProperty("error")]
    public object Error { get; set; }

    [JsonProperty("errors")]
    public object Errors { get; set; }
}

public class ProviderResponse
{
    [JsonProperty("virtualAccount")]
    public VirtualAccount VirtualAccount { get; set; }
}

public class VirtualAccount
{
    [JsonProperty("accountName")]
    public string AccountName { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("productType")]
    public string ProductType { get; set; }

    [JsonProperty("singleDepositLimit")]
    public int SingleDepositLimit { get; set; }

    [JsonProperty("fee")]
    public decimal Fee { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("meta")]
    public object Meta { get; set; }

    [JsonProperty("customerReference")]
    public string CustomerReference { get; set; }

    [JsonProperty("expireAt")]
    public string? ExpireAt { get; set; }

    [JsonProperty("createdAt")]
    public string CreatedAt { get; set; }

    [JsonProperty("vNUBAN")]
    public string VNUBAN { get; set; }
}
