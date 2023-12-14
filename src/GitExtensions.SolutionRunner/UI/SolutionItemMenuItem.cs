using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GitExtensions.SolutionRunner.UI
{
    public class SolutionItemMenuItem : ToolStripMenuItem
    {
        private const string RunAsAdminConfiguration = "runas";
        private const string ExtractExecutablePattern = @"^""([^""""]*)""";

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

            process.StartInfo.FileName = string.IsNullOrWhiteSpace(settings.ExecutablePath)
                ? GetPreferredExecutablePathOrFilePath()
                : settings.ExecutablePath;

            process.StartInfo.Arguments = string.IsNullOrWhiteSpace(arguments) ? filePath : arguments;
            process.StartInfo.UseShellExecute = true;

            if (settings.ShouldRunAsAdmin)
                process.StartInfo.Verb = RunAsAdminConfiguration;

            process.Start();

            base.OnClick(e);
        }

        private string GetPreferredExecutablePathOrFilePath()
        {
            var programId = TryFindPreferredExecutableProgram();
            var executablePath = TryFindPreferredExecutableProgramPath(programId);

            return executablePath ?? filePath;
        }

        private object TryFindPreferredExecutableProgram()
        {
            var fileExtension = Path.GetExtension(filePath)!;
            var extensionKey = Registry.ClassesRoot.OpenSubKey(fileExtension);
            var programId = GetAssociatedProgramId(extensionKey);

            var hasFileExtensionNotAssociatedProgramId = programId == null;
            if (hasFileExtensionNotAssociatedProgramId)
            {
                extensionKey = extensionKey?.OpenSubKey("OpenWithProgids");
                programId = extensionKey?.GetValueNames().FirstOrDefault();
            }

            extensionKey?.Close();

            return programId;
        }

        private static string TryFindPreferredExecutableProgramPath(object programId)
        {
            if(programId == null)
                return null;

            var programKey = Registry.ClassesRoot.OpenSubKey(programId + @"\shell\open\command");
            var runTemplate = GetAssociatedProgramId(programKey);

            programKey?.Close();

            var executableApplication = Regex.Match(runTemplate.ToString(), ExtractExecutablePattern).Value;

            return executableApplication;
        }

        private static object GetAssociatedProgramId(RegistryKey extensionKey)
        {
            return extensionKey.GetValue(string.Empty);
        }
    }
}