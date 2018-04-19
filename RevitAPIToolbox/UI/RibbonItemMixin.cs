using System;
using Autodesk.Windows;

namespace Techyard.Revit.UI
{
    internal static class RibbonItemMixin
    {
        public static string GetCommandId(this RibbonItem item)
        {
            return item.Id;
        }

        public static void SetCommandId(this RibbonItem item, string id)
        {
            RibbonTab parentTab;
            RibbonPanel parentPanel;
            item.Id = id;
            ComponentManager.Ribbon.FindItem(item.Id, false, out parentPanel, out parentTab, true);

            if (parentTab == null || parentPanel == null)
                return;

            if (string.IsNullOrEmpty(parentTab.Id))
                parentTab.Id = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(parentPanel.Source.Id))
                parentPanel.Source.Id = Guid.NewGuid().ToString();
            item.GenerateId(parentPanel.Source.Id, id);
        }

        public static void GenerateId(this RibbonItem item, string parentId, string name)
        {
            item.Id = $"CustomCtrl_%{parentId}%{name}";
        }
    }
}