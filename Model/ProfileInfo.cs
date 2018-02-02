using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProfileInfo
    {
        [Key]
        public int ID { get; set; }
        public string Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsMale { get; set; }
        public int Age { get; set; }
        public Role Role { get; set; }
        public string PhotoPath { get; set; }
        public bool IsCitizen { get; set; }

        public BalanceInfo Balance { get; set; }

        public ProfileInfo()
        {
            Balance = new BalanceInfo();
        }
    }
}
