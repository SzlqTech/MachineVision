using MachineVision.Core.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace MachineVision.Core.Services
{
    public class NavigationMenuService : BindableBase, INavigationMenuService
    {
        public NavigationMenuService()
        {
            Items = new ObservableCollection<NavigationItem>();
        }

        private ObservableCollection<NavigationItem> items;

        public ObservableCollection<NavigationItem> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }

        public void InitMenus()
        {
            Items.Clear();
            Items.Add(new NavigationItem("", "All", "全部", "", new ObservableCollection<NavigationItem>()
            {
                 new NavigationItem("","Firework","流程管理","",new ObservableCollection<NavigationItem>()
                 {
                      new NavigationItem("home","MainTab", "主页面","MainTabView"),

                 }),

                 new NavigationItem("","Config","配置管理","",new ObservableCollection<NavigationItem>()
                 {              
                      new NavigationItem("product","Product", "产品管理","ProductView"),
                      new NavigationItem("camera","Camera", "相机管理","CarmerView"),

                 }),
                
            }));

            Items.Add(new NavigationItem("", "MainTab", "主页面", "MainTabView"));
            Items.Add(new NavigationItem("", "Product", "产品管理", "ProductView"));
         
        }

        public void RefreshMenus()
        {
            //foreach (var item in Items)
            //{
            //   // item.Name = LanguageHelper.KeyValues[item.Key];
            //    if (item.Items != null && item.Items.Count > 0)
            //    {
            //        foreach (var subItem in item.Items)
            //        {
            //           /// subItem.Name = LanguageHelper.KeyValues[subItem.Key];
            //            if (subItem.Items != null && subItem.Items.Count > 0)
            //            {
            //                foreach (var other in subItem.Items)
            //                {
            //                   // if (other != null) other.Name = LanguageHelper.KeyValues[other.Key];
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}
