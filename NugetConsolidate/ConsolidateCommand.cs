using System;
using System.ComponentModel.Design;
using Consolidate;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;

namespace NugetConsolidate
{
    /// <summary>
    /// Command handlers
    /// </summary>
    internal sealed class ConsolidateCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("35fd4e54-69f9-4f4a-869e-5ac1d5eb38c6");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package _package;

        private IVsStatusbar _statusBarSvc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidateCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private ConsolidateCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            _package = package;

            var commandService = ServiceProvider.GetService(typeof (IMenuCommandService)) as OleMenuCommandService;
            if (commandService == null) return;
            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(MenuItemCallback, menuCommandId);
            commandService.AddCommand(menuItem);

            Consolidator.Instance.NeedsConsolidating += OnNeedsConsolidating;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ConsolidateCommand Instance { get; private set; }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => _package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new ConsolidateCommand(package);
        }

        public void RegisterVisualStudioServices(IVsStatusbar statusBarSvc)
        {
            _statusBarSvc = statusBarSvc;
        }

        public void RegisterNugetServices(IVsPackageInstallerServices installerServices, IVsPackageInstaller packageInstaller, IVsPackageUninstaller packageUninstaller)
        {
            Consolidator.Instance.RegisterNugetServices(installerServices, packageInstaller, packageUninstaller);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            Consolidator.Instance.Execute();
        }

        private void OnNeedsConsolidating(object sender, EventArgs eventArgs)
        {
            _statusBarSvc.SetColorText("Consolidate nuget packages", 123, 98);
        }
    }
}