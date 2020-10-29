using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitExtensions.SolutionRunner.Services
{
    public class DirectorySolutionFileProvider : ISolutionFileProvider
    {
        private readonly string rootPath;

        public DirectorySolutionFileProvider(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public Task<IReadOnlyCollection<string>> GetListAsync(bool isTopLevelSearchOnly, bool includeWorkspaces)
        {
            SearchOption searchOption = isTopLevelSearchOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            string[] solutionFiles = Directory.GetFiles(rootPath, "*.sln", searchOption);

            if (includeWorkspaces) 
            {
                solutionFiles = solutionFiles
                    .Concat(Directory.GetFiles(this.rootPath, "*.code-workspace", searchOption))
                    .ToArray();
            }
            return Task.FromResult<IReadOnlyCollection<string>>(solutionFiles);
        }
    }
}
