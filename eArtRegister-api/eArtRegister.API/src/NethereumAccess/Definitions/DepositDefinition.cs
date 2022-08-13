using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace NethereumAccess.Definitions
{
    public class DepositDefinition
    {
        public partial class DepositDeployment : DepositDeploymentBase
        {
            public DepositDeployment() : base(BYTECODE) { }
            public DepositDeployment(string byteCode) : base(byteCode) { }
        }

        public class DepositDeploymentBase : ContractDeploymentMessage
        {
            public static string BYTECODE = "608060405234801561001057600080fd5b506103a6806100206000396000f3fe6080604052600436106100345760003560e01c806327e235e31461003957806351cff8d914610076578063d0e30db014610092575b600080fd5b34801561004557600080fd5b50610060600480360381019061005b919061020d565b61009c565b60405161006d9190610253565b60405180910390f35b610090600480360381019061008b91906102ac565b6100b4565b005b61009a610153565b005b60006020528060005260406000206000915090505481565b8073ffffffffffffffffffffffffffffffffffffffff166108fc349081150290604051600060405180830381858888f193505050501580156100fa573d6000803e3d6000fd5b50346000803373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282546101499190610308565b9250508190555050565b346000803373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282546101a1919061033c565b92505081905550565b600080fd5b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b60006101da826101af565b9050919050565b6101ea816101cf565b81146101f557600080fd5b50565b600081359050610207816101e1565b92915050565b600060208284031215610223576102226101aa565b5b6000610231848285016101f8565b91505092915050565b6000819050919050565b61024d8161023a565b82525050565b60006020820190506102686000830184610244565b92915050565b6000610279826101af565b9050919050565b6102898161026e565b811461029457600080fd5b50565b6000813590506102a681610280565b92915050565b6000602082840312156102c2576102c16101aa565b5b60006102d084828501610297565b91505092915050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052601160045260246000fd5b60006103138261023a565b915061031e8361023a565b9250828203905081811115610336576103356102d9565b5b92915050565b60006103478261023a565b91506103528361023a565b925082820190508082111561036a576103696102d9565b5b9291505056fea2646970667358221220e50664cf21893d663b202f39cc23f87480778debc02309d26dcbb4ca0c4ed24464736f6c63430008100033";
            public DepositDeploymentBase() : base(BYTECODE) { }
            public DepositDeploymentBase(string byteCode) : base(byteCode) { }

        }

        public partial class BalancesFunction : BalancesFunctionBase { }

        [Function("balances", "uint256")]
        public class BalancesFunctionBase : FunctionMessage
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class DepositFunction : DepositFunctionBase { }

        [Function("deposit")]
        public class DepositFunctionBase : FunctionMessage
        {

        }

        public partial class WithdrawFunction : WithdrawFunctionBase { }

        [Function("withdraw")]
        public class WithdrawFunctionBase : FunctionMessage
        {
            [Parameter("address", "destAddr", 1)]
            public virtual string DestAddr { get; set; }
        }

        public partial class BalancesOutputDTO : BalancesOutputDTOBase { }

        [FunctionOutput]
        public class BalancesOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }
    }
}
