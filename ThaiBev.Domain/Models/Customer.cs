using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiBev.Domain.Models
{
    public partial class Customers
    {
        public int Id { get; set; }
        public string CusName { get; set; }
        public string CusAddress { get; set; }
    }
}
