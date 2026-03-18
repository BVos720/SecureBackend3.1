using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class MemoryEnviromentRepository : ILevelProgress
    {
        private static List<LevelProgress> LevelProgresses = [];

        public Task deleteAsync(Guid id)
        {
            LevelProgresses.Remove(LevelProgresses.Single(x => x.Id == id));
            return Task.CompletedTask;
        }

        public Task InsertAsync(LevelProgress levelProgress)
        {
            LevelProgresses.Add(levelProgress);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<LevelProgress>> SelectAsync()
        {
            return Task.FromResult(LevelProgresses.AsEnumerable());
        }

        public Task<LevelProgress?> SelectAsync(Guid id)
        {
            return Task.FromResult(LevelProgresses.SingleOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<LevelProgress>> SelectByUserAsync(string userId)
        {
            return Task.FromResult(LevelProgresses.Where(x => x.UserID == userId).AsEnumerable());
        }

        public Task DeleteByEnvironmentAsync(Guid environmentId)
        {
            LevelProgresses.RemoveAll(x => x.Id == environmentId);
            return Task.CompletedTask;
        }

        public async Task UpdateAsync(LevelProgress levelProgress)
        {
            await deleteAsync(levelProgress.Id);
            await InsertAsync(levelProgress);
        }
    }
}
