using GitExtensions.SolutionRunner.Properties;
using GitExtensions.SolutionRunner.Services;
using GitExtensions.SolutionRunner.UI;
using GitUI;
using GitUI.CommandsDialogs;
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

        private MenuStripEx FindMainMenu(IGitUICommands commands)
        {
            FormBrowse form = (FormBrowse)((GitUICommands)commands).BrowseRepo;
            if (form != null)
            {
                MenuStripEx mainMenu = form.Controls.OfType<MenuStripEx>().FirstOrDefault();
                return mainMenu;
            }

            return null;
        }

        private MainMenuItem FindMainMenuItem(IGitUICommands commands, MenuStripEx mainMenu = null)
        {
            if (mainMenu == null)
                mainMenu = FindMainMenu(commands);

            if (mainMenu == null)
                return null;

            return mainMenu.Items.OfType<MainMenuItem>().FirstOrDefault();
        }

        public override void Register(IGitUICommands commands)
        {
            base.Register(commands);

            if (commands.GitModule.IsValidGitWorkingDir())
            {
                MenuStripEx mainMenu = FindMainMenu(commands);
                if (mainMenu != null && FindMainMenuItem(commands, mainMenu) == null)
                {
                    var provider = new DirectorySolutionFileProvider(commands.GitModule.WorkingDir);

                    mainMenu.Items.Add(new MainMenuItem(provider));
                }
            }
        }

        public override void Unregister(IGitUICommands commands)
        {
            base.Unregister(commands);

            MenuStripEx mainMenu = FindMainMenu(commands);
            if (mainMenu != null)
            {
                MainMenuItem mainMenuItem = FindMainMenuItem(commands, mainMenu);
                if (mainMenuItem != null)
                {
                    mainMenu.Items.Remove(mainMenuItem);
                    mainMenuItem.Dispose();
                }
            }
        }
    }
}
