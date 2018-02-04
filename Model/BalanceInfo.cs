using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class BalanceInfo
    {
        private int _current;

        [Key]
        public int ID { get; set; }

        public int Current
        {
            get
            {
                if (MiningTime != null && MiningTime < DateTime.Now) //mining
                {
                    _current += 100; //from web.config
                    MiningTime = null;
                }
                return _current;
            }
            set { _current = value; }
        }

        public DateTime? MiningTime { get; set; }
    }
}
