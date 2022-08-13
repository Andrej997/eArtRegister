using eArtRegister.API.Application.Users.Commands.CheckWallet;
using eArtRegister.API.Application.Users.Commands.CreateDeposit;
using eArtRegister.API.Application.Users.Commands.Deposit;
using eArtRegister.API.Application.Users.Commands.DepositServer;
using eArtRegister.API.Application.Users.Commands.RequestRolePermission;
using eArtRegister.API.Application.Users.Queries.GetRoles;
using eArtRegister.API.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpGet("getUser/{wallet}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<UserDto>> GetRoles(string wallet)
        {
            try
            {
                return await Mediator.Send(new GetRolesQuery(wallet));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("createDeposit")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateDeposit(CreateDepositCommand command)
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

        [HttpPost("deposit")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> Deposit(DepositCommand command)
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

        [HttpPost("deposit/server")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> DepositServer(DepositServerCommand command)
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