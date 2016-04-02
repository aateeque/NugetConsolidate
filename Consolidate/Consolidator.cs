using System;
using System.Collections.Generic;
using NuGet.VisualStudio;

namespace Consolidate
{
    public class Consolidator
    {
        private static readonly Lazy<Consolidator> Lazy = new Lazy<Consolidator>(() => new Consolidator());
        private static readonly Dictionary<string, List<string>> PackageVerCache = new Dictionary<string, List<string>>();
        private IVsPackageInstallerServices _installerServices;

        public static Consolidator Instance => Lazy.Value;

        private Consolidator()
        {
        }

        public void Initialize(IVsPackageInstallerServices installerServices)
        {
            if (installerServices != null)
            {
                _installerServices = installerServices;
            }
        }

        public void Execute()
        {
            var installedPackages = _installerServices.GetInstalledPackages();

            installedPackages.BuildCache(PackageVerCache)
                             .Consolidate();
        }
    }
}