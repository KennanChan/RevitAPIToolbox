using System;
using System.Collections.Generic;
using Autodesk.Windows;
using UIFramework;
using UIFrameworkServices;

namespace Techyard.Revit.UI
{
    public static class RibbonCommandItemMixin
    {
        public static bool SetShortCut(this RibbonCommandItem commandItem, string key)
        {
            try
            {
                if (commandItem == null || string.IsNullOrEmpty(key))
                    return false;

                RibbonTab parentTab;
                RibbonPanel parentPanel;
                var commandId = ControlHelper.GetCommandId(commandItem);

                if (string.IsNullOrEmpty(commandId))
                {
                    commandId = Guid.NewGuid().ToString();
                    ControlHelper.SetCommandId(commandItem, commandId);
                }

                ComponentManager.Ribbon.FindItem(commandItem.Id, false, out parentPanel, out parentTab, true);

                if (parentTab == null || parentPanel == null)
                    return false;

                var path = $"{parentTab.Id}>{parentPanel.Source.Id}";

                var shortcutItem = new ShortcutItem(commandItem.Text, commandId, key, path)
                {
                    ShortcutType = StType.RevitAPI
                };
                KeyboardShortcutService.applyShortcutChanges(
                    new Dictionary<string, ShortcutItem>
                    {
                        {
                            commandId, shortcutItem
                        }
                    });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
