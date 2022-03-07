using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Contract.Queries.Test
{
    public class TestQuery : IRequest
    {
    }

    public class TestQueryHandler : IRequestHandler<TestQuery, Unit>
    {
        private readonly INethereumBC _nethereum;

        public TestQueryHandler(INethereumBC nethereum)
        {
            _nethereum = nethereum;
        }

        public async Task<Unit> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                await _nethereum.TestContract();

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
