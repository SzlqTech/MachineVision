using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace MachineVision.Core.ViewModels
{
    public class DialogViewModel :ViewModelBase, IHostDialogAware
    {
        public DialogViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            SaveCommand = new DelegateCommand(() =>
            {
                Save();
            });
        }

        public DelegateCommand CancelCommand { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        public string IdentifierName { get; set; }

        public virtual void Cancel()
        {
            if (DialogHost.IsDialogOpen(IdentifierName))
                DialogHost.Close(IdentifierName);
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public virtual void Save()
        {
            Save(new DialogResult(ButtonResult.OK));
        }

        public virtual void Save(DialogResult result)
        {
            if (DialogHost.IsDialogOpen(IdentifierName))
                DialogHost.Close(IdentifierName, result);
        }
    }
}
