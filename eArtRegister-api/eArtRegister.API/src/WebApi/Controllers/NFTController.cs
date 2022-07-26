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
        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPost("add")]
        public async Task<ActionResult<Guid>> AddNFT(IFormFile file, string name, string description, Guid bundleId, double price, double royality)
        {
            if (file == null)
            {
                return BadRequest("Missing NFT file");
            }

            try
            {
                return await Mediator.Send(new AddNFTCommand
                {
                    Name = name,
                    Description = description,
                    BundleId = bundleId,
                    Price = price,
                    Royality = royality,
                    File = file.GetUploadFileModel()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
