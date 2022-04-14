using System.Threading.Tasks;

namespace RichbetsResurrected.Interfaces.Interfaces.Games;

public interface IStartableGame
{
    public Task StartAsync();
}