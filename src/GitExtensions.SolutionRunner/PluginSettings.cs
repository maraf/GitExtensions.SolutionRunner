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
        
        private readonly ISettingsSource source;

        /// <summary>
        /// Gets current value of <see cref="ExecutablePathProperty"/>.
        /// </summary>
        public string ExecutablePath => source.GetValue(ExecutablePathProperty.Name, ExecutablePathProperty.DefaultValue, t => t);

        /// <summary>
        /// Gets current value of <see cref="ExecutableArgumentsProperty"/>.
        /// </summary>
        public string ExecutableArguments => source.GetValue(ExecutableArgumentsProperty.Name, ExecutableArgumentsProperty.DefaultValue, t => t);
        
        public PluginSettings(ISettingsSource source)
        {
            this.source = source;
        }

        #region IEnumerable<ISetting>

        private static readonly List<ISetting> properties;

        static PluginSettings()
        {
            properties = new List<ISetting>(2)
            {
                ExecutablePathProperty,
                ExecutableArgumentsProperty,
            };
        }

        public IEnumerator<ISetting> GetEnumerator()
            => properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion
    }
}
