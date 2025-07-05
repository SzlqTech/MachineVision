using AutoMapper;
using MachineVision.Core.Models;
using MachineVision.Core.Services.DataBase;
using MachineVision.Core.ViewModels;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;


namespace MachineVision.View.ViewModels
{
    public class ProductViewModel: NavigationViewMdoel
    {
        private readonly ProductService productService;
     

        public ProductViewModel(ProductService productService)
        {
            InitCommand();
            this.productService = productService;
        }


        public ProductModel SelectedProduct { get; set; }


        public DelegateCommand AddCommand { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        public DelegateCommand<ProductModel> ImportCommand { get; set; }

        public ObservableCollection<ProductModel> Products { get; set; }

        public void InitCommand()
        {
            Products = new ObservableCollection<ProductModel>();
            AddCommand = new DelegateCommand(() =>
            {
                Products.Add(new ProductModel() { });
            });
            SaveCommand = new DelegateCommand(async () =>
            {
                await Save();
            });
            ImportCommand = new DelegateCommand<ProductModel>(product =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "VM Sol File|*.sol*";
                if ((bool)openFileDialog.ShowDialog())
                {
                    product.Path=openFileDialog.FileName;
                }
            });
        }

        public async Task Save()
        {
           await productService.InsertOrUpdateBatchAsync(Products.ToList());        
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
           
        }
    }
}
