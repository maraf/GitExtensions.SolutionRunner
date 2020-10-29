using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitExtensions.SolutionRunner.Services
{
    public interface ISolutionFileProvider
    {
        Task<IReadOnlyCollection<string>> GetListAsync(bool isTopLevelSearchOnly, bool includeWorkspaces);
    }
}
