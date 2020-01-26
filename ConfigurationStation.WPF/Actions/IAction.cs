using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationStation.WPF.Actions
{
    public interface IAction
    {
        Task Execute(ActionContext context, CancellationToken cancellationToken);
    }
}
