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
        //    [Route("api/[controller]")]
        //    [ApiController]
        //    public class UsersController : ApiControllerBase
        //    {
        //        [HttpPost]
        //        [Route("search")]
        //        [ApiExplorerSettings(GroupName = "v1")]
        //        public async Task<ActionResult<List<UserDto>>> Search(SearchQuerry querry)
        //        {
        //            try
        //            {
        //                return await Mediator.Send(querry);
        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest(ex.Message);
        //            }
        //        }

        //        [HttpPost]
        //        [Route("register")]
        //        [ApiExplorerSettings(GroupName = "v1")]
        //        public async Task<ActionResult<UserDto>> Register(RegisterCommand command)
        //        {
        //            try
        //            {
        //                return await Mediator.Send(command);
        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest(ex.Message);
        //            }
        //        }

        //        [HttpPost]
        //        [Route("login")]
        //        [ApiExplorerSettings(GroupName = "v1")]
        //        public async Task<ActionResult<UserDto>> Login(LoginCommand command)
        //        {
        //            try
        //            {
        //                return await Mediator.Send(command);
        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest(ex.Message);
        //            }
        //        }
    }
}