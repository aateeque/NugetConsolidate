using System.Collections.Generic;
using NuGet.VisualStudio;

namespace Consolidate
{
    public interface IConsolidatingService
    {
        void Consolidate(IEnumerable<IVsPackageMetadata> installedPackages); 
    }
}