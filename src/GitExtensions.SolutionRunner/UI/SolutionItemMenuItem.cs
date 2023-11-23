using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GitExtensions.SolutionRunner.UI
{
    public class SolutionItemMenuItem : ToolStripMenuItem
    {
        private const string RunAsAdminConfiguration = "runas";
        private const string VsCodeWorkspace = ".code-workspace";
        private const string VsCodeExe = "code";

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
            string arguments = settings.ExecutableArguments
                ?.Replace(PluginSettings.SolutionFileToken, filePath)
                ?.Replace(PluginSettings.SolutionDirectoryToken, Path.GetDirectoryName(filePath));

            var process = new Process();
            process.StartInfo.FileName = !string.IsNullOrWhiteSpace(settings.ExecutablePath)
                ? settings.ExecutablePath
                : filePath;
            process.StartInfo.Arguments = arguments ?? filePath;
            process.StartInfo.UseShellExecute = true;

            if (settings.ShouldRunAsAdmin)
                StartAdminProcess(process, e);
            else
                process.Start();

            base.OnClick(e);
        }

        private void StartAdminProcess(Process process, EventArgs e)
        {
            if (IsVisualStudioCodeWorkspace())
                process.StartInfo.FileName = VsCodeExe;

            process.StartInfo.Verb = RunAsAdminConfiguration;

            try
            {
                process.Start();
            }
            catch (Win32Exception ex)
            {
            }
        }

        private bool IsVisualStudioCodeWorkspace()
        {
            return filePath.EndsWith(VsCodeWorkspace, StringComparison.OrdinalIgnoreCase);
        }
    }
}