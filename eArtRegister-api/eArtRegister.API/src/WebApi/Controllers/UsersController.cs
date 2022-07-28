//using eArtRegister.API.Application.Users.Commands.Login;
//using eArtRegister.API.Application.Users.Commands.Register;
//using eArtRegister.API.Application.Users.Queries.Search;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace eArtRegister.API.WebApi.Controllers
//{
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
//    }
//}
