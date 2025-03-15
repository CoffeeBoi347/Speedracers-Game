using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsManager : MonoBehaviour
{
    public static PlayerActionsManager instance;
    public PlayerActions action;
    public PlaneMovementPlayer1 player;
    private Dictionary<string, System.Action> actionMap;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        actionMap = new Dictionary<string, System.Action>
        {
            {
                "STOP", () => player.FreezePlayer()
            }
            ,
            {
                "SRT", () => player.UnfreezePlayer()
            }
            ,
            {
                "DODGE", () => player.DodgePlayer()
            }
            ,
            {
                "FLIP", () => player.FlipPlayer()
            }
            ,
            {
                "SHOT", () => player.BulletPlayer()
            }
            ,
            {
                "FRE", () => player.FirePlayer()
            }
            ,
            {
                "MOVE", () => player.MovePlayer()
            }
            ,
            {
                "RUSH", () => player.RushPlayer()
            }
            ,
            {
                "GRAPPLE", () => player.GrapplePlayer()
            }
        };
    }

    private void Start()
    {
        action = PlayerActions.None;
    }

    public void ExecuteAction(string typedWord)
    {
        string upperCaseWord = typedWord.ToUpper();
        if (actionMap.ContainsKey(upperCaseWord))
        {
            actionMap[upperCaseWord]?.Invoke(); 
        }
    }
}


public enum PlayerActions
{
    None,
    Freeze,
    Unfreeze,
    Dodge,
    Flip,
    Bullet,
    Strike,
    Fire,
    Rush,
    Move,
    Grapple
}