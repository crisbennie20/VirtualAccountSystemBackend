using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualAccountSystemBackend.DTO;
using VirtualAccountSystemBackend.Response;
using VirtualAccountSystemBackend.Service;

namespace VirtualAccountSystemBackend.Controllers
{
    [Route("api/vas")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly VirtualAccountTransactionService virtualAccountTransactionService;

        public TransactionsController(VirtualAccountTransactionService virtualAccountTransactionService)
        {
            this.virtualAccountTransactionService = virtualAccountTransactionService;
        }

        [HttpGet("getAllTransactions")]
        public async Task<ActionResult<ServiceResponse<List<TransactionData>>>> getAllVirtualAccounts()
        {
            var service = await virtualAccountTransactionService.getAllVirtualAccountTransaction();
            return service;

        }

        [HttpGet("getTransactionsByAccount")]
        public async Task<ActionResult<ServiceResponse<List<TransactionData>>>> getVirtualAccountByAccountNo(string accountNo)
        {
            var service = await virtualAccountTransactionService.getAllVirtualAccountTransactionByAccountNo(accountNo);
            return service;

        }
    }
}
