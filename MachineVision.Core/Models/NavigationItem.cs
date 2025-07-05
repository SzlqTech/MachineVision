using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace MachineVision.Core.Models
{
    public class NavigationItem : BindableBase
    {
        public NavigationItem(string icon, string key, string name, string pageName, ObservableCollection<NavigationItem> items = null)
        {
            this.Icon = icon;
            this.Name = name;
            this.PageName = pageName;
            this.Items = items;
            this.Key = key;
        }

       
        private string name;
      
        private string icon;
    
        private ObservableCollection<NavigationItem> items;
       
        private string pageName;
       
        private string key;

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 菜单图标
        /// </summary>

        public string Icon
        {
            get { return icon; }
            set { icon = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 菜单子项
        /// </summary>
        public ObservableCollection<NavigationItem> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 菜单导航页面名称
        /// </summary>
        public string PageName
        {
            get { return pageName; }
            set { pageName = value; RaisePropertyChanged(); }
        }

        public string Key
        {
            get { return key; }
            set { key = value; RaisePropertyChanged(); }
        }
    }
}
