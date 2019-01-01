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

        public Task<IReadOnlyCollection<string>> GetListAsync()
        {
            string[] solutionFiles = Directory.GetFiles(rootPath, "*.sln", SearchOption.AllDirectories);
            return Task.FromResult<IReadOnlyCollection<string>>(solutionFiles);
        }
    }
}
