using System;
using System.Collections.Generic;
using System.ICore.Enum;
using System.ICore.Log;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace System.ICore.MenuItemSet
{
    public class MenuItemSeting
    {
        public static Dictionary<MenuStyle, List<MenuItem>> MenuItemUI = new Dictionary<MenuStyle, List<MenuItem>>();

        public static void AddMenuItem(MenuStyle style, MenuItem item)
        {
            try
            {
                if (MenuItemUI.Keys.Contains(style))
                {
                    if (MenuItemUI.TryGetValue(style, out List<MenuItem> Items))
                    {
                        Items.Add(item);
                    }
                }
                else
                {
                    MenuItemUI.Add(style, new List<MenuItem>() { item });
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }



        public static MenuItem FindMenuItem(MenuStyle style, string Name)
        {
            try
            {
                if (MenuItemUI.Keys.Contains(style))
                {
                    MenuItemUI.TryGetValue(style, out List<MenuItem> Items);
                    MenuItem menu = Items.FirstOrDefault(p => p.Header.ToString() == Name);
                    return menu;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
                return null;
            }
        }
    }
}
