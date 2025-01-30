using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtualAccountSystemBackend.DTO;
using VirtualAccountSystemBackend.Model;
using VirtualAccountSystemBackend.Request;
using VirtualAccountSystemBackend.Response;

[ApiController]
[Route("api/webhook")]
public class WebhookController : ControllerBase
{
    private readonly ILogger<WebhookController> _logger;
    private readonly VASContext _vasContext;
    private readonly RSAService _rsaService = new RSAService();
    private readonly AESService _aesService = new AESService();

    public WebhookController(
        ILogger<WebhookController> logger,
        VASContext vasContext // Injected from DI
       )
    {
        _logger = logger;
        _vasContext = vasContext;
    
    }

    [HttpPost]
    public ActionResult ReceiveWebhook([FromBody] WebhookRequest webhookRequest, [FromHeader(Name = "SecureToken")] string SecureToken)
    {
        _logger.LogInformation("Webhook received 1...");

        try
        {
            if (webhookRequest == null || string.IsNullOrEmpty(webhookRequest.Data))
                return BadRequest("Invalid webhook request. Missing 'data' field.");

            _logger.LogInformation("Webhook received.");
            _logger.LogInformation($"SecureToken: {SecureToken}");
            _logger.LogInformation($"Raw Encrypted Data: {webhookRequest.Data}");

            // Step 1: Retrieve Merchant Private Key
            //var merchantDetail = _vasContext.ApiAccessInfos.FirstOrDefault();
            var getMerchantDetails = _vasContext.ApiAccessInfos.FromSqlInterpolated<ApiAccessInfo>($"GetMerchantDetails").AsEnumerable().ToList();

            var merchantDetail = getMerchantDetails.Select(x => new ApiAccessInfo
            {
                Id = x.Id,
                MerchantCode = x.MerchantCode,
                MerchantPrivateKey = x.MerchantPrivateKey,
                MerchantPublicKey = x.MerchantPublicKey,
                PublicKey = x.PublicKey,
                SymmetricKey = x.SymmetricKey
            });

            if (merchantDetail == null)
                return BadRequest("Merchant details not found.");

            // Step 2: Decrypt SecureToken using RSA to retrieve AES Key
            string aesKey = _rsaService.DecryptWithPrivateKey(SecureToken, merchantDetail.FirstOrDefault().MerchantPrivateKey);

            if (string.IsNullOrEmpty(aesKey))
                return BadRequest("Failed to decrypt SecureToken.");

            _logger.LogInformation($"Decrypted AES Key: {aesKey}");

            // Step 3: Decrypt Encrypted Data using AES Key
            string jsonData = AESService.Decrypt(webhookRequest.Data, aesKey);
            if (string.IsNullOrEmpty(jsonData))
                return BadRequest("Failed to decrypt data.");

            _logger.LogInformation($"Decrypted JSON Data: {jsonData}");

            // Step 4: Deserialize JSON to Transaction Object
            var transaction = JsonConvert.DeserializeObject<TransactionResponse>(jsonData);
            if (transaction == null)
                return BadRequest("Invalid transaction data.");

            // Step 5: Save Transaction to Database



            var customerTransaction = new CustomerTransaction
            {
                Amount = transaction.Amount,
                BeneficiaryAccountName = transaction.BeneficiaryAccountName,
                BeneficiaryAccountNumber = transaction.BeneficiaryAccountNumber,
                BeneficiaryBankVerificationNumber = transaction.BeneficiaryBankVerificationNumber,
                BeneficiaryKyclevel = transaction.BeneficiaryKyclevel,
                ChannelCode = transaction.ChannelCode,
                CollectionAccountNumber = transaction.CollectionAccountNumber,
                DestinationInstitutionCode = transaction.DestinationInstitutionCode,
                DateCreated = DateTime.Now,
                NameEnquiryRef = transaction.NameEnquiryRef,
                Narration = transaction.Narration,
                OriginatorAccountName = transaction.OriginatorAccountName,
                OriginatorAccountNumber = transaction.OriginatorAccountNumber,
                OriginatorBankVerificationNumber = transaction.OriginatorBankVerificationNumber,
                OriginatorKyclevel = transaction.OriginatorKyclevel,
                PaymentReference = transaction.PaymentReference,
                SessionId = transaction.SessionId,
                TransactionLocation = transaction.TransactionLocation,
                Status = transaction.Status

            };

            _vasContext.CustomerTransactions1.Add(customerTransaction);
            _vasContext.SaveChanges();

            _logger.LogInformation($"Received Transaction: {transaction.PaymentReference}, Amount: {transaction.Amount}");

            //vasContext.CustomerTransactions.Add(transaction);
            //vasContext.SaveChanges();

            _logger.LogInformation($"Transaction Received: {transaction.PaymentReference}, Amount: {transaction.Amount}");

            return Ok(new { status = "success" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Webhook processing failed: {ex.Message}");
            return StatusCode(500, new { status = "error", message = ex.Message });
        }
    }
}
