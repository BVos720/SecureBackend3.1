using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IEnviroment2D
    {

        Task InsertAsync(Models.Level2D enviroment2D);
        Task deleteAsync(Guid id);
        Task<IEnumerable<Level2D>> SelectAsync();
        Task<IEnumerable<Level2D>> SelectByUserAsync(string userId);
        Task<Level2D?> SelectAsync(Guid id);
        Task UpdateAsync(Level2D enviroment2D);

    }
}
