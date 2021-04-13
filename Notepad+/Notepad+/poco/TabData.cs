using System;
using System.Collections.Generic;
using System.Text;

class TabData
{
   public TabData(string name)
    {
        this.Name = name;
    }

    public TabData(string name, string filePath) : this(name)
    {
        this.FilePath = filePath;
    }

    string name = "default";
    string filePath = null;

    public string Name { get => name; set => name = value; }
    public string FilePath { get => filePath; set => filePath = value; }
}