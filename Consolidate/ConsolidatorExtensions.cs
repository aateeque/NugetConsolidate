using System.Collections.Generic;
using NuGet.VisualStudio;

namespace Consolidate
{
    public static class ConsolidatorExtensions
    {
        public static IEnumerable<IVsPackageMetadata> BuildCache(this IEnumerable<IVsPackageMetadata> This, Dictionary<string, List<string>> cache)
        {

            return This;
        }

        public static void Consolidate(this IEnumerable<IVsPackageMetadata> This)
        {
            
        }
    }
}