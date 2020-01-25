using System.Threading.Tasks;

namespace Demo.Common.Command
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}