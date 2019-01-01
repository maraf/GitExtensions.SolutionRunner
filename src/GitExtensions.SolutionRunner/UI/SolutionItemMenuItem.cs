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

        public SolutionItemMenuItem(string filePath)
        {
            this.filePath = filePath;

            Text = Path.GetFileName(filePath);
            ToolTipText = filePath;
        }

        protected override void OnClick(EventArgs e)
        {
            Process.Start(filePath);

            base.OnClick(e);
        }
    }
}
