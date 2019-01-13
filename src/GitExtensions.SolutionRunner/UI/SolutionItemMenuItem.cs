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
        private readonly string filePath;
        private readonly PluginSettings settings;

        internal SolutionItemMenuItem(string filePath, PluginSettings settings)
        {
            this.filePath = filePath;
            this.settings = settings;

            Text = Path.GetFileName(filePath);
            ToolTipText = filePath;
        }

        protected override void OnClick(EventArgs e)
        {
            if (String.IsNullOrEmpty(settings.ExecutablePath))
                Process.Start(filePath);
            else
                Process.Start(settings.ExecutablePath, settings.ExecutableArguments?.Replace(PluginSettings.DefaultExecutableArguments, filePath) ?? filePath);

            base.OnClick(e);
        }
    }
}
