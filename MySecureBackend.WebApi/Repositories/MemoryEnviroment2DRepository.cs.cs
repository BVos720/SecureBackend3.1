using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class MemoryEnviromentRepository : IEnviroment2D
    {
        private static List<Level2D> Enviroments = [];
        public Task deleteAsync(Guid id)
        {
            Enviroments.Remove(Enviroments.Single(x => x.Id == id));
            return Task.CompletedTask;
        }

        public Task InsertAsync(Level2D enviroment2D)
        {
            Enviroments.Add(enviroment2D);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Level2D>> SelectAsync()
        {
            return Task.FromResult(Enviroments.AsEnumerable());
        }

        public Task<Level2D?> SelectAsync(Guid id)
        {
            return Task.FromResult(Enviroments.SingleOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<Level2D>> SelectByUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Level2D enviroment2D)
        {
            await deleteAsync(enviroment2D.Id);
            await InsertAsync(enviroment2D);
        }
    }
}
