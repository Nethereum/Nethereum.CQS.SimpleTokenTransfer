using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
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
            TransferAsync().Wait();
            Console.ReadLine();
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
                TokenAmount = 1000
            };

            var transferHandler = web3.Eth.GetContractTrasactionHandler<TransferFunction>();
            var transferReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(transactionMessage, contractAddress);

        }


        [Function("transfer", "bool")]
        public class TransferFunction : ContractMessage
        {
            [Parameter("address", "_to", 1)]
            public string To { get; set; }

            [Parameter("uint256", "_value", 2)]
            public int TokenAmount { get; set; }
        }
    }
}
