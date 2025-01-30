using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualAccountSystemBackend.DTO;
using VirtualAccountSystemBackend.Response;
using VirtualAccountSystemBackend.Service;

namespace VirtualAccountSystemBackend.Controllers
{
    [Route("api/vas")]
    [ApiController]
    public class VirtualAccountController : ControllerBase
    {
		
        private readonly VirtualAccountService virtualAccountService;

        public VirtualAccountController(VirtualAccountService virtualAccountService)
        {
            this.virtualAccountService = virtualAccountService;
        }

        [HttpPost("createAccount")]
		public async Task<ActionResult<ServiceResponse<VasAccountResponse>>> CreateVirtualAccount(CustomerProfileDTO request)
		{
			var service = await virtualAccountService.createVirtualAccount(request);
            return  service;

		}

        [HttpGet("getAllAccount")]
        public async Task<ActionResult<ServiceResponse<List<AllVirtualAccountDTO>>>> getAllVirtualAccounts()
        {
            var service = await virtualAccountService.getAllVirtualAccount();
            return service;

        }

        [HttpGet("getAccountByAccount")]
        public async Task<ActionResult<ServiceResponse<List<AllVirtualAccountDTO>>>> getVirtualAccountByAccountNo(string accountNo)
        {
            var service = await virtualAccountService.getAllVirtualAccountByAccountNo(accountNo);
            return service;

        }

        [HttpGet("getAccountByPolicy")]
        public async Task<ActionResult<ServiceResponse<List<AllVirtualAccountDTO>>>> getVirtualAccountByPolicyNo(string policyNo)
        {
            var service = await virtualAccountService.getAllVirtualAccountByPolicyNo(policyNo);
            return service;

        }
    }
}
