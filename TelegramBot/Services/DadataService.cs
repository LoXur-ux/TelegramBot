using Dadata;
using Dadata.Model;

namespace TelegramBot.Services
{
    public class DadataService : ICleanClientAsync
    {
        public Task<T> Clean<T>(string source, CancellationToken cancellationToken = default) where T : IDadataEntity
        {
            throw new NotImplementedException();
        }

        public Task<IList<IDadataEntity>> Clean(IEnumerable<StructureType> structure, IEnumerable<string> data, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
