using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShopNoUsers.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        //Possible user id here for the future
        public string CartUserId { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }
    }
}
