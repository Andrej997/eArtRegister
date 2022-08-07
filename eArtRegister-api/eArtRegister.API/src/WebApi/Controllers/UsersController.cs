using eArtRegister.API.Application.Users.Commands.CheckWallet;
using eArtRegister.API.Application.Users.Commands.RequestRolePermission;
using eArtRegister.API.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
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

        [HttpPost("checkWallet")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CheckWallet(CheckWalletCommand command)
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