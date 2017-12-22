using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Web3.Accounts;

namespace Nethereum.CQS.SimpleTokenTransfer
{
    public class Program
    {
        static void Main(string[] args)
        {
            BalanceAsync().Wait();
            Console.ReadLine();
            //TransferAsync().Wait();
            Console.ReadLine();
        }


        public static async Task BalanceAsync()
        {
            var senderAddress = "0x7c31560552170ce96c4a7b018e93cddc19dc61b6";
            var contractAddress = "0x0d8775f648430679a709e98d2b0cb6250d2887ef";

            //no private key we are not signing anything
            var web3 = new Web3.Web3("https://mainnet.infura.io");

            var balanceOfFunctionMessage = new BalanceOfFunction()
            {
                Owner = senderAddress,
            };

            var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            var balance = await balanceHandler.QueryAsync<BigInteger>(balanceOfFunctionMessage, contractAddress);
            Console.WriteLine("Balance of token: " + Web3.Web3.Convert.FromWei(balance));

    }

        [Function("balanceOf", "uint256")]
        public class BalanceOfFunction : ContractMessage
        {

            [Parameter("address", "_owner", 1)]
            public string Owner { get; set; }

        }

        public static async Task TransferAsync()
        {
            var senderAddress = "";
            var contractAddress= "";
            var receiverAddress = "";
            var privatekey = "";

            var web3 =  new Web3.Web3(new Account(privatekey), "https://mainnet.infura.io");

            var transactionMessage = new TransferFunction()
            {
                FromAddress = senderAddress,
                To = receiverAddress,
                TokenAmount = Web3.Web3.Convert.ToWei(1000)
            };

            var transferHandler = web3.Eth.GetContractTrasactionHandler<TransferFunction>();
            var transactionHash = await transferHandler.SendRequestAsync(transactionMessage, contractAddress);
            Console.WriteLine(transactionHash);

        }


        [Function("transfer", "bool")]
        public class TransferFunction : ContractMessage
        {
            [Parameter("address", "_to", 1)]
            public string To { get; set; }

            [Parameter("uint256", "_value", 2)]
            public BigInteger TokenAmount { get; set; }
        }
    }
}
