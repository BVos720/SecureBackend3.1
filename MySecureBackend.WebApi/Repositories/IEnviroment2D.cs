using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IEnviroment2D
    {

        Task InsertAsync(Models.Enviroment2D enviroment2D);
        Task deleteAsync(Guid id);
        Task<IEnumerable<Enviroment2D>> SelectAsync();
        Task<IEnumerable<Enviroment2D>> SelectByUserAsync(string userId);
        Task<Enviroment2D?> SelectAsync(Guid id);
        Task UpdateAsync(Enviroment2D enviroment2D);

    }
}
