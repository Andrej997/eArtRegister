using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using eArtRegister.API.Application.JobSpecification.Queries.GetJobSpecification;


namespace eArtRegister.API.WebApi.Controllers
{
    //[Authorize]
    public class JobSpecificationController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<JobSpecificationVm>> Get()
        {
            return await Mediator.Send(new GetJobSpecificationQuery());
        }

        
      
    }
}
