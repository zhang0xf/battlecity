using System.Collections;
using System.Collections.Generic;

public class ResourceData
{
    private string name;
    private string path;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Path
    {
        get { return path; }
        set { path = value; }
    }

    public ResourceData(string name, string path)
    {
        Name = name;
        Path = path;
    }
}
