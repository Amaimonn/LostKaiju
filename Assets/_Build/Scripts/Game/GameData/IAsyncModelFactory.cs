using System.Threading.Tasks;
using R3;

namespace LostKaiju.Game.GameData
{
    public interface IAsyncModelFactory<T> where T : IModel
    {
        public Observable<T> OnProduced { get; }

        public abstract Task<T> GetModelAsync();
    }
}