using System.Collections.Generic;
using Project;
using Project.Decks;
using Project.Items;
using Project.PlayerSystem;
using Project.UI.MainUI;
using UnityEngine;

public class Scratch : MonoBehaviour
{
    [SerializeField] Player Player;
    private bool firstUpdate;
    private bool lmbClicked;
    private bool rmbClicked;

    void Start()
    {

    }

    void Update()
    {


        if (firstUpdate)
        {
            FirstUpdate();
            firstUpdate = false;
        }


        if (Input.GetMouseButton(0))
        {
            if (!lmbClicked)
            {
                lmbClicked = true;
                Player.GemTracker.AddGem(1);
            }
        }
        else
        {
            lmbClicked = false;
        }

        if (Input.GetMouseButton(1))
        {
            if (!rmbClicked)
            {
                rmbClicked = true;
                Player.GemTracker.RemoveGem(1);
            }
        }
        else
        {
            rmbClicked = false;
        }
    }

    private void FirstUpdate()
    {

    }
}