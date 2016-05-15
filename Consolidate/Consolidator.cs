using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;

namespace Consolidate
{
    public class Consolidator
    {
        private static readonly Lazy<Consolidator> Lazy = new Lazy<Consolidator>(() => new Consolidator());
        private static readonly Dictionary<string, List<string>> PackageVerCache = new Dictionary<string, List<string>>();

        private IVsPackageInstallerServices _installerServices;
        private readonly ConsolidatingService _consolidatingService;
        private IVsPackageInstaller _packageInstaller;
        private IVsPackageUninstaller _packageUninstaller;

        public static Consolidator Instance => Lazy.Value;

        public event EventHandler NeedsConsolidating;

        private Consolidator()
        {
            _consolidatingService = new ConsolidatingService();
        }

        public void RegisterNugetServices(IVsPackageInstallerServices installerServices, IVsPackageInstaller packageInstaller, IVsPackageUninstaller packageUninstaller)
        {
            _installerServices = installerServices;
            _packageInstaller = packageInstaller;
            _packageUninstaller = packageUninstaller;
        }

        public void Execute()
        {
            var installedPackages = _installerServices.GetInstalledPackages();

           var pkgsToConsolidate = installedPackages.GroupBy(pkg => pkg.Id, pkg => pkg.VersionString, StringComparer.OrdinalIgnoreCase)
                             .Where(g => g.Any())
                             .Select(p => Tuple.Create(p.Key, p.Max()));
            if (pkgsToConsolidate.Any())
            {
                OnNeedsConsolidating();
            }
        }

        protected virtual void OnNeedsConsolidating()
        {
            NeedsConsolidating?.Invoke(this, EventArgs.Empty);
        }
    }
}