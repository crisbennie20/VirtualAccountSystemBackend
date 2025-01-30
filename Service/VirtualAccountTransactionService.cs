using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using VirtualAccountSystemBackend.DTO;
using VirtualAccountSystemBackend.Model;
using VirtualAccountSystemBackend.Response;

namespace VirtualAccountSystemBackend.Service
{
    public class VirtualAccountTransactionService
    {
        VASContext vasContext = new VASContext();
        RSAService rsaService = new RSAService();
        AESService aesService = new AESService();

        private readonly ILogger<VirtualAccountTransactionService> logger;

        public VirtualAccountTransactionService(ILogger<VirtualAccountTransactionService> logger)
        {
            this.logger = logger;


        }

        public async Task<ServiceResponse<List<TransactionData>>> getAllVirtualAccountTransaction()
        {
            var serviceResponse = new ServiceResponse<List<TransactionData>>();
            try
            {
                var getVirtualAccounts = vasContext.TransactionDatas.FromSqlInterpolated<TransactionData>($"GetAllVirtualAccountTransaction").AsEnumerable().ToList();

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
                    serviceResponse.ResponseMessage = "No record available";
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

        public async Task<ServiceResponse<List<TransactionData>>> getAllVirtualAccountTransactionByAccountNo(string accountNo)
        {
            var serviceResponse = new ServiceResponse<List<TransactionData>>();
            try
            {
                var getVirtualAccounts = vasContext.TransactionDatas.FromSqlInterpolated<TransactionData>($"GetVirtualAccountTransactionByAccountNo {accountNo}").AsEnumerable().ToList();

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
                    serviceResponse.ResponseMessage = "No record available";
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
