using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eArtRegister.API.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eArtRegister.API.Application.JobSpecification.Queries.GetJobSpecification
{

    public class GetJobSpecificationQuery : IRequest<JobSpecificationVm>
    {
    }

    public class GetJobSpecificationQueryHandler : IRequestHandler<GetJobSpecificationQuery, JobSpecificationVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobSpecificationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobSpecificationVm> Handle(GetJobSpecificationQuery request, CancellationToken cancellationToken)
        {
            return new JobSpecificationVm
            {

                Lists = await _context.JobSpecifications
                    .AsNoTracking()
                    .ProjectTo<JobSpecificationDto>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.Description)
                    .ToListAsync(cancellationToken)

            };
        }
    }
}
