using GitExtensions.SolutionRunner.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitExtensions.SolutionRunner.UI
{
    /// <summary>
    /// Main menu item.
    /// </summary>
    public class SolutionListMenuItem : ToolStripMenuItem
    {
        private readonly ISolutionFileProvider provider;
        private readonly PluginSettings settings;

        internal SolutionListMenuItem(ISolutionFileProvider provider, PluginSettings settings)
        {
            this.provider = provider;
            this.settings = settings;

            Text = "Solution Runner";
            DropDownOpening += OnDropDownOpening;
            DropDown.ShowItemToolTips = true;
        }

        private async void OnDropDownOpening(object sender, EventArgs e)
        {
            foreach (var item in DropDown.Items.OfType<ToolStripMenuItem>().ToList())
                DropDown.Items.Remove(item);

            DropDown.Items.Add(new ToolStripMenuItem("Loading..."));

            DropDown.Items.AddRange(await CreateBundleItemsAsync());
            DropDown.Items.RemoveAt(0);
        }

        private async Task<ToolStripItem[]> CreateBundleItemsAsync()
        {
            return await Task.Run(async () =>
            {
                List<ToolStripItem> newItems = new List<ToolStripItem>();

                IEnumerable<string> solutionFiles = await provider.GetListAsync();
                foreach (string filePath in solutionFiles)
                    newItems.Add(new SolutionItemMenuItem(filePath, settings));

                newItems.Sort((x, y) => x.Text.CompareTo(y.Text));
                return newItems.ToArray();
            });
        }
    }
}
