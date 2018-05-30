using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class BalanceInfo
    {
        [Key]
        public int ID { get; set; }

        public int Current { get; set; }

        public DateTime? MiningTime { get; set; }
    }
}
