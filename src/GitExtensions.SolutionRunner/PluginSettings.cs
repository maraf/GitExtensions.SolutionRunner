using GitUIPluginInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitExtensions.SolutionRunner
{
    internal class PluginSettings : IEnumerable<ISetting>
    {
        public const string DefaultExecutableArguments = "{SolutionPath}";

        /// <summary>
        /// Gets a property holding path to executable to start.
        /// </summary>
        public static StringSetting ExecutablePathProperty { get; } = new StringSetting("Executable Path", "Executable path to start by clicking on solution file", null);

        /// <summary>
        /// Gets a property holding arguments for executa ble to start.
        /// </summary>
        public static StringSetting ExecutableArgumentsProperty { get; } = new StringSetting("Executable Arguments", $"Optional arguments for executable ({DefaultExecutableArguments} will be replaced with selected solution file)", DefaultExecutableArguments);

        /// <summary>
        /// Gets a property holding arguments for executa ble to start.
        /// </summary>
        public static BoolSetting IsTopLevelSearchedOnlyProperty { get; } = new BoolSetting("Top Level Search", "Search only top level directory structure", false);

        /// <summary>
        /// Includes VSCode workspace files (.code-workspace)
        /// </summary>
        public static BoolSetting EnableVSCodeWorkspacesProperty { get; } = new BoolSetting("Include VSCode workspaces", "Include VSCode workspace files (.code-workspace)", false);
        
        /// <summary>
        /// Start executable as Administrator
        /// </summary>
        public static BoolSetting ShouldRunAsAdminProperty { get; } = new BoolSetting("Run as Administrator", "Run as Administrator", false);

        private readonly ISettingsSource source;

        /// <summary>
        /// Gets current value of <see cref="ExecutablePathProperty"/>.
        /// </summary>
        public string ExecutablePath => source.GetValue(ExecutablePathProperty.Name, ExecutablePathProperty.DefaultValue, t => t);

        /// <summary>
        /// Gets current value of <see cref="ExecutableArgumentsProperty"/>.
        /// </summary>
        public string ExecutableArguments => source.GetValue(ExecutableArgumentsProperty.Name, ExecutableArgumentsProperty.DefaultValue, t => t);

        /// <summary>
        /// Gets current value of <see cref="IsTopLevelSearchedOnlyProperty"/>.
        /// </summary>
        public bool IsTopLevelSearchedOnly => source.GetValue(IsTopLevelSearchedOnlyProperty.Name, IsTopLevelSearchedOnlyProperty.DefaultValue, t => Boolean.Parse(t));

        /// <summary>
        /// Gets current value of <see cref="EnableVSCodeWorkspacesProperty"/>.
        /// </summary>
        public bool EnableVSCodeWorkspaces => source.GetValue(EnableVSCodeWorkspacesProperty.Name, EnableVSCodeWorkspacesProperty.DefaultValue, t => Boolean.Parse(t));
        
        /// <summary>
        /// Gets current value of <see cref="ShouldRunAsAdminProperty"/>.
        /// </summary>
        public bool ShouldRunAsAdmin =>  source.GetValue(ShouldRunAsAdminProperty.Name, ShouldRunAsAdminProperty.DefaultValue, t => Boolean.Parse(t));

        public PluginSettings(ISettingsSource source)
        {
            this.source = source;
        }

        #region IEnumerable<ISetting>

        private static readonly List<ISetting> properties;

        public static bool HasProperties => properties.Count > 0;

        static PluginSettings()
        {
            properties = new List<ISetting>(4)
            {
                ExecutablePathProperty,
                ExecutableArgumentsProperty,
                IsTopLevelSearchedOnlyProperty,
                EnableVSCodeWorkspacesProperty,
                ShouldRunAsAdminProperty
            };
        }

        public IEnumerator<ISetting> GetEnumerator()
            => properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion
    }
}
