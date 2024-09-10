using GitExtensions.SolutionRunner.Properties;
using GitExtensions.SolutionRunner.Services;
using GitExtensions.SolutionRunner.UI;
using GitUI;
using GitUI.CommandsDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using GitExtensions.Extensibility.Git;
using GitExtensions.Extensibility.Plugins;
using GitExtensions.Extensibility.Settings;

namespace GitExtensions.SolutionRunner
{
    [Export(typeof(IGitPlugin))]
    public class Plugin : GitPluginBase
    {
        internal PluginSettings Configuration { get; private set; }

        public Plugin()
            : base(PluginSettings.HasProperties)
        {
            Id = new Guid("92ff3885-c08a-477c-a04a-84fb8ec47d5c");
            Name = "SolutionRunner";
            Description = "Solution Runner";
            Icon = Resources.Icon;
        }

        public override bool Execute(GitUIEventArgs e)
        {
            e.GitUICommands.StartSettingsDialog(this);
            return false;
        }

        public override IEnumerable<ISetting> GetSettings()
            => Configuration;

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

        private SolutionListMenuItem FindMainMenuItem(IGitUICommands commands, MenuStripEx mainMenu = null)
        {
            if (mainMenu == null)
                mainMenu = FindMainMenu(commands);

            if (mainMenu == null)
                return null;

            return mainMenu.Items.OfType<SolutionListMenuItem>().FirstOrDefault();
        }

        public override void Register(IGitUICommands commands)
        {
            base.Register(commands);

            Configuration = new PluginSettings(Settings);
            if (commands.Module.IsValidGitWorkingDir())
            {
                MenuStripEx mainMenu = FindMainMenu(commands);
                if (mainMenu != null && FindMainMenuItem(commands, mainMenu) == null)
                {
                    var provider = new GitSolutionFileProvider(commands.Module.WorkingDir, commands.Module.GitExecutable);

                    mainMenu.Items.Add(new SolutionListMenuItem(provider, Configuration));
                }
            }
        }

        public override void Unregister(IGitUICommands commands)
        {
            base.Unregister(commands);

            MenuStripEx mainMenu = FindMainMenu(commands);
            if (mainMenu != null)
            {
                SolutionListMenuItem mainMenuItem = FindMainMenuItem(commands, mainMenu);
                if (mainMenuItem != null)
                {
                    mainMenu.Items.Remove(mainMenuItem);
                    mainMenuItem.Dispose();
                }
            }
        }
    }
}
