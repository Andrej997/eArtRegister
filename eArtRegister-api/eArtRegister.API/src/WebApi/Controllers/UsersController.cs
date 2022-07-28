using eArtRegister.API.Application.Users.Commands.RequestRolePermission;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpPost]
        [Route("requestRolePermission")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> RequestRolePermission(RequestRolePermissionCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}