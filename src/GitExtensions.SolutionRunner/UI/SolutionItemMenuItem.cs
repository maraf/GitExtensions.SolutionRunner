using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitExtensions.SolutionRunner.UI
{
    public class SolutionItemMenuItem : ToolStripMenuItem
    {
        private const string RunAsAdminConfiguration = "runas";

        private readonly string filePath;
        private readonly PluginSettings settings;

        internal SolutionItemMenuItem(string filePath, PluginSettings settings)
        {
            this.filePath = filePath;
            this.settings = settings;

            Text = "&" + Path.GetFileName(filePath);
            ToolTipText = filePath;
        }

        protected override void OnClick(EventArgs e)
        {
            if (string.IsNullOrEmpty(settings.ExecutablePath))
            {
                Process.Start(filePath);
            }
            else
            {
                string arguments = settings.ExecutableArguments
                    ?.Replace(PluginSettings.SolutionFileToken, filePath)
                    ?.Replace(PluginSettings.SolutionDirectoryToken, Path.GetDirectoryName(filePath));

                var process = new Process();
                process.StartInfo.FileName = settings.ExecutablePath; 
                process.StartInfo.Arguments = arguments ?? filePath;

                if (settings.ShouldRunAsAdmin)
                    process.StartInfo.Verb = RunAsAdminConfiguration;

                process.Start();
            }

            base.OnClick(e);
        }
    }
}