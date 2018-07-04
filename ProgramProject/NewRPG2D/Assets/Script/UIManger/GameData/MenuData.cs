using UnityEngine;
using System.Collections;

[System.Serializable]
public class MenuData
{
    [SerializeField]
    private int menuNumb;

    [SerializeField]
    private int id;

    [SerializeField]
    private string name;

    public int Id
    {
        get
        {
            return id;
        }
    }
    public int MenuNumb
    {
        get
        {
            return menuNumb;
        }
    }
    public string Name
    {
        get
        {
            return name;
        }
    }

    public MenuData() { }
    public MenuData(int menuNumb)
    {
        this.menuNumb = menuNumb;
    }
    public MenuData(int menuNumb, int id)
    {
        this.menuNumb = menuNumb;
        this.id = id;
    }
    public MenuData(int menuNumb, int id, string name)
    {
        this.menuNumb = menuNumb;
        this.id = id;
        this.name = name;
    }

}

