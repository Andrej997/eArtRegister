using eArtRegister.API.Application.Bundle.Commands.CreateBundle;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class BundleController : ApiControllerBase
    {
        [HttpPost]
        [Route("create")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<Guid>> CreateBundle(CreateBundleCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
