using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualAccountSystemBackend.Service;

namespace VirtualAccountSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualAccountController : ControllerBase
    {
		VirtualAccountService virtualAccountService = new VirtualAccountService();

		[HttpGet("test")]
		public Task getSHArecord()
		{

			return virtualAccountService.GenerateSHAEncription();

		}
	}
}
