using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.VisualStudio;

namespace Consolidate
{
    public class ConsolidatingService : IConsolidatingService
    {
        public void Consolidate(IEnumerable<IVsPackageMetadata> installedPackages)
        {
        }
    }
}
