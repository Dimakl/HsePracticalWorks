using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

static class StaticNotepadData
{

    public static string currentTheme = "dark";
    public static int autosave = 0;

    /// <summary>
    /// Получение бэкграунда темы.
    /// </summary>
    /// <returns></returns>
    public static Color GetRTBBackground() => currentTheme == "light" ? Color.White : Color.Gray;

    /// <summary>
    /// Получение цвета текста темы.
    /// </summary>
    /// <returns></returns>
    public static Color GetRTBTextColor() => currentTheme == "light" ? Color.Black : Color.White;

    public static TabData[][] tabData = new TabData[][] { new TabData[] { } };

    /// <summary>
    /// Удаление вкладки.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    public static void RemoveTab(int i, int j)
    {
        List<TabData> td = new List<TabData>(tabData[i]);
        td.RemoveAt(j);
        if (td.Count == 0)
            td.Add(new TabData("New file"));
        tabData[i] = td.ToArray();
    }
}
