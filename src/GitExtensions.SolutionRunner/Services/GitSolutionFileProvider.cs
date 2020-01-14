using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GitUIPluginInterfaces;
using System.Linq;

namespace GitExtensions.SolutionRunner.Services
{
    public class GitSolutionFileProvider : ISolutionFileProvider
    {
        private readonly string rootPath;
        private readonly IExecutable executor;

        public GitSolutionFileProvider(string rootPath, IExecutable executor)
        {
            this.rootPath = rootPath;
            this.executor = executor;
        }

        public async Task<IReadOnlyCollection<string>> GetListAsync(bool isTopLevelSearchOnly)
        {
            IProcess process = executor.Start("ls-files -coz -- *.sln", redirectOutput: true);
            string output = await process.StandardOutput.ReadToEndAsync();

            var result = output.Split(new[] { '\0' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !isTopLevelSearchOnly || !x.Contains('/'))
                .Select(x => Path.Combine(rootPath, x))
                .Where(File.Exists)
                .ToArray();

            return result;
        }
    }
}
