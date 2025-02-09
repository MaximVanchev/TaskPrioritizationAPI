using TaskPrioritizationAPI.Infrastructure.Data;
using TaskPrioritizationAPI.Infrastructure.Data.Common;

namespace TaskPrioritizationAPI.Infrastructure.Data.Repositories
{
    public class ApplicationDbRepository : Repository, IApplicationDbRepository
    {
        public ApplicationDbRepository(Context context)
        {
            this.Context = context;
        }
    }
}