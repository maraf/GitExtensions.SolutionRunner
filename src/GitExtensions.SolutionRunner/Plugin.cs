using GitExtensions.SolutionRunner.Properties;
using GitUIPluginInterfaces;
using ResourceManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitExtensions.SolutionRunner
{
    [Export(typeof(IGitPlugin))]
    public class Plugin : GitPluginBase
    {
        public Plugin()
        {
            Name = "SolutionRunner";
            Description = "Solution Runner";
            Icon = Resources.Icon;
        }

        public override bool Execute(GitUIEventArgs e)
            => true;
    }
}
