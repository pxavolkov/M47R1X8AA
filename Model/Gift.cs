using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Gift
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public string UsedUserID { get; set; }
        public int CreditsBonus { get; set; }
        public DateTime? UsedDate { get; set; }
    }
}
