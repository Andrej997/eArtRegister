using eArtRegister.API.Application.NFTs.Commands.AddNFT;
using eArtRegister.API.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class NFTController : ApiControllerBase
    {
        [ApiExplorerSettings(GroupName = "asset")]
        [HttpPost("add")]
        public async Task<ActionResult<Guid>> AddNFT(IFormFile file, AddNFTCommand command)
        {
            if (file == null)
            {
                return BadRequest("Missing NFT file");
            }

            try
            {
                command.File = file.GetUploadFileModel();
                return await Mediator.Send(command);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }
    }
}
