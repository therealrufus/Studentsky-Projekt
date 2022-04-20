using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    string s = "";
    public Text text;

    public Transform player;
    private void Start()
    {
        player.GetComponent<MoveCrouch>().OnEnd.AddListener(OnCrouch);
        player.GetComponent<MoveWallride>().OnEnd.AddListener(OnWallrun);
        player.GetComponent<MoveGrapple>().OnEnd.AddListener(OnGrapple);
        player.GetComponent<MoveDoubleJump>().OnJump.AddListener(OnDoubleJump);
        player.GetComponent<MoveKick>().OnJump.AddListener(OnKick);
        ObjectiveManager.instance.onNewObjective.AddListener(OnObjective);
        player.GetComponent<ThrowPizza>().OnThrow.AddListener(OnThrow);
    }

    private void Update()
    {
        Change();
    }

    void Change()
    {
        switch (s)
        {
            case "":
                text.text = "press [Control] to crouch, you can use this to slide of slopes and change direction quickly";
                break;
            case "crouch":
                text.text = "you can wallrun! Just try running into a slope";
                break;
            case "wallrun":
                text.text = "press [Q] to activate your grappling hook";
                break;
            case "hook":
                text.text = "you have a double jump! Press [Space] in mid-air";
                break;
            case "jump":
                text.text = @"hardest movement technique: the wallKick
press [F] to kick a nearby wall
this will allow you to preserve momentum while changing the direction you were flying";
                    break;
            case "kick":
                text.text = @"Goal: run, fly, grapple to the marked objectives as fast as possible!
run into the marked objective to continue";
                break;
            case "objectives":
                text.text = @"you can also throw projectiles to complete objectives for you!
Throw them by pressing [LeftMouseButton]";
                break;
            case "throw":
                text.text = @"Thats it! Have fun!
[R] to restart and [esc] to go to the main menu";
                break;
            default:
                text.text = @"Thats it! Have fun!
[R] to restart and [Esc] to go to the main menu";
                break;
        }
    }

    void OnCrouch()
    {
        if (s == "")
            s = "crouch";
    }

    void OnWallrun()
    {
        if(s == "crouch")
        s = "wallrun";
    }

    void OnGrapple()
    {
        if (s == "wallrun")
            s = "hook";
    }

    void OnDoubleJump()
    {
        if (s == "hook")
            s = "jump";
    }

    void OnKick()
    {
        if (s == "jump")
            s = "kick";
    }

    void OnObjective()
    {
        if (s == "kick")
            s = "objectives";
    }


    void OnThrow()
    {
        if (s == "objectives")
            s = "throw";
    }
}
