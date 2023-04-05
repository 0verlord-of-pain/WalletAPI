using Wallet.Core.Enums;

namespace Wallet.API.Controllers.In;
    public class CreateTransactionModel
    {
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public TransactionStatus Status { get; set; }
        public string ImageUrl { get; set; }
}
