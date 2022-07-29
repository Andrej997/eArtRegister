using eArtRegister.API.Application.Users.Commands.RequestRolePermission;
using eArtRegister.API.Application.Users.Queries.GetUserIds;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpGet("ids")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<Guid>>> GetUserIds()
        {
            try
            {
                return await Mediator.Send(new GetUserIdsQuery());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("requestRolePermission")]
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