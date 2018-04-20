#region References

using System;
using System.Linq;
using System.Windows.Media;
using Autodesk.Windows;

#endregion

namespace Techyard.Revit.UI
{
    public class UIFactory
    {
        private UIFactory()
        {
        }

        public static UIFactory Factory { get; } = new UIFactory();

        private RibbonTab CreateTab(string tabName)
        {
            var tab = new RibbonTab
            {
                Name = tabName,
                Title = tabName,
                Id = tabName
            };
            return tab;
        }

        public RibbonTab FindTab(string tabName, bool createIfAbsent = false)
        {
            var tab = ComponentManager.Ribbon.Tabs.FirstOrDefault(t => t.Title.Equals(tabName));
            if (null == tab && createIfAbsent)
                tab = CreateTab(tabName);
            return tab;
        }

        public void SetTabColor(string tabName, Color color)
        {
            var tab = FindTab(tabName, true);
            tab.Theme.TabHeaderBackground = new SolidColorBrush(color);
        }

        public RibbonPanel CreatePanel(string tabName, string panelName)
        {
            var tab = FindTab(tabName, true);
            var panel = new RibbonPanel
            {
                Source = new RibbonPanelSource
                {
                    Title = panelName,
                    Name = panelName,
                    Id = ControlUtility.GenerateId(tab.Id, panelName)
                }
            };
            tab.Panels.Add(panel);
            return panel;
        }

        public RibbonPanel FindPanel(string tabName, string panelName, bool createIfAbsent = false)
        {
            var tab = FindTab(tabName, createIfAbsent);
            var panel = tab?.Panels.FirstOrDefault(p => p.Source.Name.Equals(panelName));
            if (null == panel && createIfAbsent)
                panel = CreatePanel(tabName, panelName);
            return panel;
        }

        public T CreateItem<T>(string tabName, string panelName, string itemName, Action<T> callback = null)
            where T : RibbonItem
        {
            var panel = FindPanel(tabName, panelName, true);
            var item = Activator.CreateInstance<T>();
            item.GenerateId(panel.Source.Id, itemName);
            item.Name = itemName;
            panel.Source.Items.Add(item);
            callback?.Invoke(item);
            return item;
        }

        public T CreateItem<T>(string itemName) where T : RibbonItem
        {
            var item = Activator.CreateInstance<T>();
            item.Name = itemName;
            return item;
        }

        public T FindItem<T>(string tabName, string panelName, string itemName, bool createIfAbsent = false)
            where T : RibbonItem
        {
            var panel = FindPanel(tabName, panelName, true);
            var item = panel?.Source.Items.FirstOrDefault(b => b.Name.Equals(itemName)) as T;
            if (null == item && createIfAbsent)
                item = CreateItem<T>(tabName, panelName, itemName);
            return item;
        }
    }
}