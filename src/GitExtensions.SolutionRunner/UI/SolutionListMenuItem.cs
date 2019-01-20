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

            Text = "&Solution Runner";
            DropDownOpening += OnDropDownOpening;
            DropDown.ShowItemToolTips = true;
        }

        private async void OnDropDownOpening(object sender, EventArgs e)
        {
            DropDown.Items.Clear();

            var loading = new LoadingMenuItem();
            DropDown.Items.Add(loading);

            HashSet<string> solutions = new HashSet<string>();
            DropDown.Items.AddRange(await CreateBundleItemsAsync(settings.IsTopLevelSearchedOnly, solutions));
            DropDown.Items.Remove(loading);

            if (settings.IsTopLevelSearchedOnly)
            {
                var separator = new ToolStripSeparator();
                DropDown.Items.Add(separator);
                DropDown.Items.Add(loading);

                ToolStripItem[] items = await CreateBundleItemsAsync(false, solutions);
                if (items.Length > 0)
                    DropDown.Items.AddRange(items);
                else
                    DropDown.Items.Remove(separator);

                DropDown.Items.Remove(loading);
            }
        }

        private async Task<ToolStripItem[]> CreateBundleItemsAsync(bool isTopLevelSearchedOnly, HashSet<string> solutions)
        {
            return await Task.Run(async () =>
            {
                List<ToolStripItem> newItems = new List<ToolStripItem>();

                IEnumerable<string> solutionFiles = await provider.GetListAsync(isTopLevelSearchedOnly);
                foreach (string filePath in solutionFiles)
                {
                    if (solutions.Add(filePath))
                        newItems.Add(new SolutionItemMenuItem(filePath, settings));
                }

                newItems.Sort((x, y) => x.Text.CompareTo(y.Text));

                return newItems.ToArray();
            });
        }
    }
}
