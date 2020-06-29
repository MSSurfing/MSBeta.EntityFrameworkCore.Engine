using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Engine.QueryValues
{
    public class StringValue
    {
        public string Value { get; set; }
    }

    public partial class StringValueMap : IQueryTypeConfiguration<StringValue>
    {
        public void Configure(QueryTypeBuilder<StringValue> builder) { }
    }
}
