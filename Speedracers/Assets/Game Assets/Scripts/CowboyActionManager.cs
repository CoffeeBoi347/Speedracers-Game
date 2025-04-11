using System.Collections.Generic;
using UnityEngine;

public class CowboyActionManager : MonoBehaviour
{
    public static CowboyActionManager instance;
    public CowboyFightWordManager fightWordManager;
    private Dictionary<string, System.Action> actionMap;
    public CowboyPlayer player;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        actionMap = new Dictionary<string, System.Action>()
        {
            {
                "RUN", () => player.RunPlayer()
            } 
            ,
            {
                "FLIP", () => player.FlipPlayer()
            } 
            ,
            {
                "JUMP", () => player.JumpPlayer()
            }
            ,
            {
                "ATTACK", () => player.PlayerCombat()
            }   
            ,
            {
                "WALK", () => player.WalkPlayer()
            }
            ,
            {
                "STOP", () => player.StopPlayer()
            }
        };
    }

    private void Start()
    {
        fightWordManager = FindObjectOfType<CowboyFightWordManager>();
    }
    public void ExecuteAction(string targettedWord)
    {
        if(actionMap.ContainsKey(targettedWord))
        {
            actionMap[targettedWord]?.Invoke();
        }
    }
}