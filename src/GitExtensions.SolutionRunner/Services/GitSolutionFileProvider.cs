using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

using GitExtensions.Extensibility;

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

        public async Task<IReadOnlyCollection<string>> GetListAsync(bool isTopLevelSearchOnly, bool includeWorkspaces)
        {
            string command = "ls-files -cz *.sln" + (includeWorkspaces ? " *.code-workspace" : "");
            IProcess process = executor.Start(command, redirectOutput: true, outputEncoding: Encoding.Default);
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
