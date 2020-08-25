using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogBook.Database
{
    public static class EntityTypeBuilderExtenstion
    {
        public static PropertyBuilder<DateTime> HasUTCConversion(this PropertyBuilder<DateTime> property)
        {
            return property.HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }

        public static PropertyBuilder<DateTime?> HasUTCConversion(this PropertyBuilder<DateTime?> property)
        {
            return property.HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
        }
    }
}
