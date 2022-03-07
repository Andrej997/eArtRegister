using eArtRegister.API.Application.Contract.Queries.Test;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using NetTopologySuite.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class ContractController : ApiControllerBase
    {
        [HttpGet]
        [Route("test")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> Test()
        {
            try
            {
                await Mediator.Send(new TestQuery());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
