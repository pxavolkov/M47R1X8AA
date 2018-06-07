using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public string TargetUserId { get; set; }
        public int Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionType { get; set; }
        public string Comment { get; set; }

        public Transaction(string userId, string targetUserId, int amount, int transactionType, string comment = null)
        {
            UserId = userId;
            TargetUserId = targetUserId;
            Amount = amount;
            TransactionType = transactionType;
            Comment = comment;

            TransactionDate = DateTime.Now;
        }
    }
}
