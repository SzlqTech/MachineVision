using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace MachineVision.Core.ViewModels
{
    public interface IHostDialogService : IDialogService
    {
        Task<IDialogResult> ShowDialogAsync(
            string name,
            IDialogParameters parameters = null,
            string IdentifierName = "Root");
    }
}
