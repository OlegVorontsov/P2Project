using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P2Project.Core.Extensions
{
    public static class EfCorePropertyExtensions
    {
        public static PropertyBuilder<DateTime> SetLocalDateTime(
            this PropertyBuilder<DateTime> builder,
            DateTimeKind kind)
        {
            return builder.HasConversion(
                        d => d.ToUniversalTime(),
                        d => DateTime.SpecifyKind(d, kind));
        }
    }
}
