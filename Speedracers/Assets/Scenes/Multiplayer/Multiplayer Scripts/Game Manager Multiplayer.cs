using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMultiplayer : Singleton<GameManagerMultiplayer>
{
    [SerializeField] private int NumberOfMenus;
    private List<IMenu> allMenus = new List<IMenu>();

    public void AddMenu(IMenu _menu)
    {
        allMenus.Add(_menu);
        Debug.Log(allMenus.Count);

        if (allMenus.Count >= NumberOfMenus)
        {
            Debug.Log("All menus are added");

            OpenMenu(EMenuName.Menu);
        }
    }

    public void OpenMenu(EMenuName eMenuName)
    {
        foreach (var i in allMenus)
        {
            if (i.MenuName == eMenuName)
                i.OpenMenu();
            else
                i.CloseMenu();
        }
    }
}

public interface IMenu
{
    public EMenuName MenuName { get; set; }
    public void OpenMenu();
    public void CloseMenu();
}
public enum EMenuName
{
    Menu,
    Multiplayer,
    RoomList,
    WaitingArea,
    ViewAllRooms,
    Disconnected,
}