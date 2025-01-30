using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using RestSharp;
using Serilog.Core;
using System.Security.Cryptography.X509Certificates;

using VirtualAccountSystemBackend.DTO;
using VirtualAccountSystemBackend.Model;
using VirtualAccountSystemBackend.Response;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace VirtualAccountSystemBackend.Service
{
    public class VirtualAccountService
    {
        VASContext vasContext = new VASContext();
        RSAService rsaService = new RSAService();
        AESService aesService = new AESService();

        private readonly ILogger<VirtualAccountService> logger;

        public VirtualAccountService(ILogger<VirtualAccountService> logger)
        {
            this.logger = logger;


        }
        public async Task<ServiceResponse<VasAccountResponse>> createVirtualAccount(CustomerProfileDTO request)
        {



            var serviceResponse = new ServiceResponse<VasAccountResponse>();

            try
            {
                var getMerchantDetails = vasContext.ApiAccessInfos.FromSqlInterpolated<ApiAccessInfo>($"GetMerchantDetails").AsEnumerable().ToList();

                var merchantDetail = getMerchantDetails.Select(x => new ApiAccessInfo
                {
                    Id = x.Id,
                    MerchantCode = x.MerchantCode,
                    MerchantPrivateKey = x.MerchantPrivateKey,
                    MerchantPublicKey = x.MerchantPublicKey,
                    PublicKey = x.PublicKey,
                    SymmetricKey = x.SymmetricKey
                });

                var customerProfile = new CustomerProfile
                {
                    AccountName = request.AccountName,
                    Bvn = request.Bvn,
                    Channel = request.Channel,
                    CustomerReference = request.TransactionReference,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    IsTempAccount = request.IsTempAccount,
                    MiddleName = request.MiddleName,
                    PolicyNo = request.PolicyNo,
                    ProductType = request.ProductType,
                    SingleDepositLimit = request.SingleDepositLimit,

                    DateCreated = DateTime.Now

                };

                var addCustomerInfo = vasContext.CustomerProfiles.Add(customerProfile);
                var saveRecord = vasContext.SaveChanges();

                if (saveRecord > 0)
                {

                    var requestPayload = new
                    {

                        bvn = request.Bvn,
                        firstName = request.FirstName,
                        lastName = request.LastName,
                        middleName = request.MiddleName,
                        accountName = request.AccountName,
                        email = request.Email,
                        phone = request.Phone,
                        productType = request.ProductType,
                        customerReference = request.TransactionReference,
                        expireAt = request.ExpireAt,
                        singleDepositLimit = request.SingleDepositLimit,
                        merchant = new
                        {
                            code = getMerchantDetails.FirstOrDefault().MerchantCode
                        }
                    };
                    string jsonString = JsonConvert.SerializeObject(requestPayload);



                    RestResponse restResponse = new RestResponse();

                    var payloadHash = HashUtil.Hash(jsonString);

                    var authToken = HashUtil.Hash(getMerchantDetails.FirstOrDefault().MerchantPublicKey);

                    var secureToken = rsaService.EncryptWithPublicKey(getMerchantDetails.FirstOrDefault().SymmetricKey, getMerchantDetails.FirstOrDefault().PublicKey);

                    string data_AESencrypted = AESService.Encrypt(jsonString, getMerchantDetails.FirstOrDefault().SymmetricKey);

                    string signature = rsaService.SignWithPrivateKey(payloadHash, getMerchantDetails.FirstOrDefault().MerchantPrivateKey);

                    var restClient = new RestClient("https://virtualaccountstest.ubagroup.com/api/v1/virtual");

                    var getClientRequest = new RestRequest("/accounts", Method.Post);

                    getClientRequest.AddHeader("Authorization", $"Bearer {authToken}");
                    getClientRequest.AddHeader("Signature", signature);
                    getClientRequest.AddHeader("SecureToken", secureToken);



                    var requestBody = new


                    {
                        data = data_AESencrypted
                    };




                    getClientRequest.AddJsonBody(requestBody);

                    restResponse = restClient.Execute(getClientRequest);



                    if (restResponse.IsSuccessful == true)
                    {
                        var resData = JsonConvert.DeserializeObject<ApiRes>(restResponse.Content);
                        var check = resData.Data;
                        //var check4 = check;

                        var check2 = getMerchantDetails.FirstOrDefault().SymmetricKey;

                        var decryptResponse = AESService.Decrypt(check.ToString(), getMerchantDetails.FirstOrDefault().SymmetricKey);
                        var virtualAccountDecrypt = JsonConvert.DeserializeObject<VirtualAccountApiResponse>(decryptResponse);

                        var accountResponse = new VasAccountResponse
                        {
                            AccountName = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.AccountName,
                            VNUBAN = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.VNUBAN,
                            Status = virtualAccountDecrypt.Status,
                            CreatedAt = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.CreatedAt,
                            CustomerReference = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.CustomerReference,
                            ExpireAt = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.ExpireAt,
                            Fee = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.Fee,
                            Meta = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.Meta,
                            ProductType = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.ProductType,
                            SingleDepositLimit = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.SingleDepositLimit,
                            Type = virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.Type
                        };

                        var customerInfo = new CustomerAccountInfo
                        {
                            CustomerId = customerProfile.Id,
                            AccountName = accountResponse.AccountName,
                            VirtualAccountNo = accountResponse.VNUBAN,
                            AccountType = accountResponse.Type,
                            CustomerReference = accountResponse.CustomerReference,
                            Status = accountResponse.Status,
                            ProductType = accountResponse.ProductType,
                            Meta = accountResponse.Meta.ToString(),
                            DateCreated = DateTime.Now

                        };
                        vasContext.CustomerAccountInfos.Add(customerInfo);

                        var saveInfo = vasContext.SaveChanges();

                        if (saveInfo > 0)
                        {
                            var getCustomerDetails = vasContext.CustomerProfiles.Where(p => p.Id == customerProfile.Id).FirstOrDefault();
                            // DateTime dateTime ;
                            //var date =  DateTime.TryParse(virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.ExpireAt, out dateTime);
                            // getCustomerDetails.ExpireAt = dateTime;
                            DateTime? dateTime = null;  // Use nullable DateTime to allow null
                            if (!string.IsNullOrEmpty(virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.ExpireAt))
                            {
                                bool dateParsed = DateTime.TryParse(virtualAccountDecrypt.Data.ProviderResponse.VirtualAccount.ExpireAt, out DateTime parsedDate);
                                if (dateParsed)
                                {
                                    dateTime = parsedDate;
                                }
                            }

                            getCustomerDetails.ExpireAt = dateTime;
                            vasContext.CustomerProfiles.Update(getCustomerDetails);
                            vasContext.SaveChanges();

                            serviceResponse.Data = accountResponse;
                            serviceResponse.ResponseCode = "00";
                            serviceResponse.ResponseStatus = true;
                            serviceResponse.ResponseMessage = virtualAccountDecrypt.Message;
                            return serviceResponse;
                        }
                        else
                        {
                            logger.LogInformation("Failed to save record");
                            logger.LogInformation(customerProfile.Id.ToString());
                            serviceResponse.Data = null;
                            serviceResponse.ResponseCode = "01";
                            serviceResponse.ResponseStatus = false;
                            serviceResponse.ResponseMessage = "Failed to save record";
                            return serviceResponse;
                        }

                    }
                    else
                    {
                        logger.LogInformation("Response from api call");
                        logger.LogInformation(restResponse.Content);

                        var resData = JsonConvert.DeserializeObject<ApiRes>(restResponse.Content);
                        var check = resData.Data;
                        //var check4 = check;

                        var check2 = getMerchantDetails.FirstOrDefault().SymmetricKey;


                        string decryptResponse = AESService.Decrypt(check.ToString(), getMerchantDetails.FirstOrDefault().SymmetricKey);
                        var decryptResponse1 = JsonConvert.DeserializeObject<DecryptApiResponse>(decryptResponse);

                        serviceResponse.Data = null;
                        serviceResponse.ResponseCode = "01";
                        serviceResponse.ResponseStatus = false;
                        serviceResponse.ResponseMessage = decryptResponse1.Message;
                        return serviceResponse;

                    }



                }
                else
                {
                    logger.LogInformation("Database to save record not available");
                    serviceResponse.Data = null;
                    serviceResponse.ResponseCode = "01";
                    serviceResponse.ResponseStatus = false;
                    serviceResponse.ResponseMessage = "Database to save record not available";
                    return serviceResponse;
                }

            }
            catch (Exception ex)
            {

                logger.LogInformation("Exception error");
                logger.LogInformation(ex.ToString());
                serviceResponse.Data = null;
                serviceResponse.ResponseCode = "05";
                serviceResponse.ResponseStatus = false;
                serviceResponse.ResponseMessage = "Failed to connect to the server";
                return serviceResponse;
            }





        }

        public async Task<ServiceResponse<List<AllVirtualAccountDTO>>> getAllVirtualAccount()
        {
            var serviceResponse = new ServiceResponse<List<AllVirtualAccountDTO>>();
            try
            {
                var getVirtualAccounts = vasContext.AllVirtualAccountDTOs.FromSqlInterpolated<AllVirtualAccountDTO>($"GetAllVirtualAccount").AsEnumerable().ToList();

                if (getVirtualAccounts.Any())
                {

                    serviceResponse.Data = getVirtualAccounts;
                    serviceResponse.ResponseCode = "00";
                    serviceResponse.ResponseStatus = true;
                    serviceResponse.ResponseMessage = "Successfully retirved";
                    return serviceResponse;
                }
                else
                {
                    logger.LogInformation("No record available");
                    serviceResponse.Data = null;
                    serviceResponse.ResponseCode = "01";
                    serviceResponse.ResponseStatus = false;
                    serviceResponse.ResponseMessage = "Database to save record not available";
                    return serviceResponse;
                }
            }
            catch (Exception ex)
            {

                logger.LogInformation("Exception error");
                logger.LogInformation(ex.ToString());
                serviceResponse.Data = null;
                serviceResponse.ResponseCode = "05";
                serviceResponse.ResponseStatus = false;
                serviceResponse.ResponseMessage = "Failed to connect to the server";
                return serviceResponse;
            }



        }

        public async Task<ServiceResponse<List<AllVirtualAccountDTO>>> getAllVirtualAccountByAccountNo(string accountNo)
        {
            var serviceResponse = new ServiceResponse<List<AllVirtualAccountDTO>>();
            try
            {
                var getVirtualAccounts = vasContext.AllVirtualAccountDTOs.FromSqlInterpolated<AllVirtualAccountDTO>($"GetVirtualAccountByAccountNo {accountNo}").AsEnumerable().ToList();

                if (getVirtualAccounts.Any())
                {

                    serviceResponse.Data = getVirtualAccounts;
                    serviceResponse.ResponseCode = "00";
                    serviceResponse.ResponseStatus = true;
                    serviceResponse.ResponseMessage = "Successfully retirved";
                    return serviceResponse;
                }
                else
                {
                    logger.LogInformation("No record available");
                    serviceResponse.Data = null;
                    serviceResponse.ResponseCode = "01";
                    serviceResponse.ResponseStatus = false;
                    serviceResponse.ResponseMessage = "Database to save record not available";
                    return serviceResponse;
                }
            }
            catch (Exception ex)
            {

                logger.LogInformation("Exception error");
                logger.LogInformation(ex.ToString());
                serviceResponse.Data = null;
                serviceResponse.ResponseCode = "05";
                serviceResponse.ResponseStatus = false;
                serviceResponse.ResponseMessage = "Failed to connect to the server";
                return serviceResponse;
            }



        }

        public async Task<ServiceResponse<List<AllVirtualAccountDTO>>> getAllVirtualAccountByPolicyNo(string policyNo)
        {
            var serviceResponse = new ServiceResponse<List<AllVirtualAccountDTO>>();
            try
            {
                var getVirtualAccounts = vasContext.AllVirtualAccountDTOs.FromSqlInterpolated<AllVirtualAccountDTO>($"GetVirtualAccountByPolicyNo {policyNo}").AsEnumerable().ToList();

                if (getVirtualAccounts.Any())
                {

                    serviceResponse.Data = getVirtualAccounts;
                    serviceResponse.ResponseCode = "00";
                    serviceResponse.ResponseStatus = true;
                    serviceResponse.ResponseMessage = "Successfully retirved";
                    return serviceResponse;
                }
                else
                {
                    logger.LogInformation("No record available");
                    serviceResponse.Data = null;
                    serviceResponse.ResponseCode = "01";
                    serviceResponse.ResponseStatus = false;
                    serviceResponse.ResponseMessage = "Database to save record not available";
                    return serviceResponse;
                }
            }
            catch (Exception ex)
            {

                logger.LogInformation("Exception error");
                logger.LogInformation(ex.ToString());
                serviceResponse.Data = null;
                serviceResponse.ResponseCode = "05";
                serviceResponse.ResponseStatus = false;
                serviceResponse.ResponseMessage = "Failed to connect to the server";
                return serviceResponse;
            }



        }


    }
}
