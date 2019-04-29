﻿using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;

namespace Nethereum.CQS.SimpleTokenTransfer
{
    public class Program
    {
        
        static void Main(string[] args)
        {

            Console.WriteLine("Deploying the contract");
            DeployStandardTokenAsync().Wait();
            Console.ReadLine();
            Console.WriteLine("Checking the balance");
            BalanceAsync().Wait();
            Console.ReadLine();
            Console.WriteLine("Transfering...");
            TransferAsync().Wait();
            Console.ReadLine();
        }

        //This is the contract address of an already deployed smartcontract in the Rinkey Test Chain
        private static string ContractAddress { get; set; } = "0xe757820308b5701302341cecd8a62c8d425c782b";

        //The smart contract deployment message (command) including the Byte code (compiled solidity smart contract) and the Constructor parameters.
        public class StandardTokenDeployment : ContractDeploymentMessage
        {
            public static string BYTECODE = "0x60606040526040516020806106f5833981016040528080519060200190919050505b80600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005081905550806000600050819055505b506106868061006f6000396000f360606040523615610074576000357c010000000000000000000000000000000000000000000000000000000090048063095ea7b31461008157806318160ddd146100b657806323b872dd146100d957806370a0823114610117578063a9059cbb14610143578063dd62ed3e1461017857610074565b61007f5b610002565b565b005b6100a060048080359060200190919080359060200190919050506101ad565b6040518082815260200191505060405180910390f35b6100c36004805050610674565b6040518082815260200191505060405180910390f35b6101016004808035906020019091908035906020019091908035906020019091905050610281565b6040518082815260200191505060405180910390f35b61012d600480803590602001909190505061048d565b6040518082815260200191505060405180910390f35b61016260048080359060200190919080359060200190919050506104cb565b6040518082815260200191505060405180910390f35b610197600480803590602001909190803590602001909190505061060b565b6040518082815260200191505060405180910390f35b600081600260005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008573ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925846040518082815260200191505060405180910390a36001905061027b565b92915050565b600081600160005060008673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561031b575081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505410155b80156103275750600082115b1561047c5781600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a381600160005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505403925050819055506001905061048656610485565b60009050610486565b5b9392505050565b6000600160005060008373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505490506104c6565b919050565b600081600160005060003373ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561050c5750600082115b156105fb5781600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a36001905061060556610604565b60009050610605565b5b92915050565b6000600260005060008473ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005054905061066e565b92915050565b60006000600050549050610683565b9056";

            public StandardTokenDeployment() : base(BYTECODE)
            {

            }

            [Parameter("uint256", "totalSupply")]
            public BigInteger TotalSupply { get; set; }
        }

        //The Deployment of the Standard Token smart contract
        public static async Task DeployStandardTokenAsync()
        {
            
            //Your account address
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";

            //The private key corresponding to your address used to sign the transactions.
            var privatekey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";

            //The url to the Rinkeby testchain in infura
            var url = "https://ropsten.infura.io/";

            //The smart contract deployment message, as described above, including the "Total Supply" of the Standard Token
            var deploymentMessage = new StandardTokenDeployment
            {
                TotalSupply = 100000,
                FromAddress = senderAddress
            };
           
            //Creating a new instance of Web3 to connect to Ethereum, including a new account configured with our private key to sign transactions
            var web3 = new Web3.Web3(new Account(privatekey), url);

            //Deploying using a new handler
            var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardTokenDeployment>();
            var transactionReceipt = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage);

            ContractAddress = transactionReceipt.ContractAddress;

            Console.WriteLine("Contract deployed to address: " + ContractAddress);
        }

      


        public static async Task BalanceAsync()
        {
            //Replace with your own
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var contractAddress = ContractAddress;
            var url = "https://ropsten.infura.io/";

            //no private key we are not signing anything (read only mode)
            var web3 = new Web3.Web3(url);

            var balanceOfFunctionMessage = new BalanceOfFunction()
            {
                Owner = senderAddress,
            };

            var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            var balance = await balanceHandler.QueryAsync<BigInteger>(contractAddress, balanceOfFunctionMessage);


            Console.WriteLine("Balance of token: " + balance);

        }

        [Function("balanceOf", "uint256")]
        public class BalanceOfFunction : FunctionMessage
        {

            [Parameter("address", "_owner", 1)]
            public string Owner { get; set; }

        }

        public static async Task TransferAsync()
        {
            //Replace with your own
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var receiverAddress = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
            var privatekey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var url = "https://ropsten.infura.io/";


            var web3 =  new Web3.Web3(new Account(privatekey), url);

            var transactionMessage = new TransferFunction()
            {
                FromAddress = senderAddress,
                To = receiverAddress,
                TokenAmount = 100,
                //Set our own price
                GasPrice =  Web3.Web3.Convert.ToWei(25, UnitConversion.EthUnit.Gwei)
                
            };

            var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();

            /// this is done automatically so is not needed.
            var estimate = await transferHandler.EstimateGasAsync(ContractAddress, transactionMessage);
            transactionMessage.Gas = estimate.Value;


            var transactionHash = await transferHandler.SendRequestAsync(ContractAddress, transactionMessage);
            Console.WriteLine(transactionHash);

        }


        [Function("transfer", "bool")]
        public class TransferFunction : FunctionMessage
        {
            [Parameter("address", "_to", 1)]
            public string To { get; set; }

            [Parameter("uint256", "_value", 2)]
            public BigInteger TokenAmount { get; set; }
        }
    }
}
