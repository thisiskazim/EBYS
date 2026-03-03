using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public int BaseKurumId { get; set; }
        public DateTime creat_time { get; set; } = DateTime.Now;
        public bool isDelete { get; set; } = false;
    }
}
