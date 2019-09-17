using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSurfing.Data
{
    public abstract partial class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
