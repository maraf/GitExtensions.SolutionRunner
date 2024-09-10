using System.Collections;
using System.Collections.Generic;

using GitExtensions.Extensibility.Settings;

namespace GitExtensions.SolutionRunner
{
    internal class PluginSettings : IEnumerable<ISetting>
    {
        public const string DefaultExecutableArguments = SolutionFileToken;
        public const string SolutionFileToken = "{SolutionPath}";
        public const string SolutionDirectoryToken = "{SolutionDirectory}";

        /// <summary>
        /// Gets a property holding path to executable to start.
        /// </summary>
        public static StringSetting ExecutablePathProperty { get; } = new StringSetting("Executable Path", "Executable path to start by clicking on solution file", null);

        /// <summary>
        /// Gets a property holding arguments for executa ble to start.
        /// </summary>
        public static StringSetting ExecutableArgumentsProperty { get; } = new StringSetting("Executable Arguments", $"Optional arguments for executable (with {SolutionFileToken} and {SolutionDirectoryToken} variables)", DefaultExecutableArguments);

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

        private readonly SettingsSource source;

        /// <summary>
        /// Gets current value of <see cref="ExecutablePathProperty"/>.
        /// </summary>
        public string ExecutablePath => source.GetString(ExecutablePathProperty.Name, ExecutablePathProperty.DefaultValue);

        /// <summary>
        /// Gets current value of <see cref="ExecutableArgumentsProperty"/>.
        /// </summary>
        public string ExecutableArguments => source.GetString(ExecutableArgumentsProperty.Name, ExecutablePathProperty.DefaultValue);

        /// <summary>
        /// Gets current value of <see cref="IsTopLevelSearchedOnlyProperty"/>.
        /// </summary>
        public bool IsTopLevelSearchedOnly => source.GetBool(IsTopLevelSearchedOnlyProperty.Name, IsTopLevelSearchedOnlyProperty.DefaultValue);


        /// <summary>
        /// Gets current value of <see cref="EnableVSCodeWorkspacesProperty"/>.
        /// </summary>
        public bool EnableVSCodeWorkspaces => source.GetBool(EnableVSCodeWorkspacesProperty.Name, EnableVSCodeWorkspacesProperty.DefaultValue);
        
        /// <summary>
        /// Gets current value of <see cref="ShouldRunAsAdminProperty"/>.
        /// </summary>
        public bool ShouldRunAsAdmin =>  source.GetBool(ShouldRunAsAdminProperty.Name, ShouldRunAsAdminProperty.DefaultValue);

        public PluginSettings(SettingsSource source)
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
