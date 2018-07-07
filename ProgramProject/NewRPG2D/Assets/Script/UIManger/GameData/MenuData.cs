using UnityEngine;
using System.Collections;

[System.Serializable]
public class MenuData
{
    [SerializeField]
    private int menuNumber;

    [SerializeField]
    private int id;

    [SerializeField]
    private string name;

    [SerializeField]
    private int parentNumber;

    [SerializeField]
    private string use;

    [SerializeField]
    private MenuType menuType;

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
            return menuNumber;
        }
    }
    public string Name
    {
        get
        {
            return name;
        }
    }

    public int ParentNumber
    {
        get
        {
            return parentNumber;
        }
    }

    public string Use
    {
        get
        {
            return use;
        }
    }

    public MenuType MenuType
    {
        get
        {
            return menuType;
        }
    }

    public MenuData() { }
    public MenuData(string name)
    {
        this.name = name;
    }
    public MenuData(int parentNumber, int id)
    {
        this.parentNumber = parentNumber;
        this.id = id;
    }
    public MenuData(int menuNumb, int id, string name)
    {
        this.menuNumber = menuNumb;
        this.id = id;
        this.name = name;
    }

}

