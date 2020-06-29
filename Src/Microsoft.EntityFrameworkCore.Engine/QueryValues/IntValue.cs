using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Engine.QueryValues
{
    public partial class IntValue
    {
        public int? Value { get; set; }
    }

    public partial class IntValueMap : IQueryTypeConfiguration<IntValue>
    {
        public void Configure(QueryTypeBuilder<IntValue> builder) { }
    }
}
