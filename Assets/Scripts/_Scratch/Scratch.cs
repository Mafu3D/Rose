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
    private bool firstUpdate = true;
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


        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))
        {
            Player.GoldTracker.AddGold(10);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.V))
        {
            Player.GoldTracker.RemoveGold(1);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.G))
        {
            Player.GemTracker.AddGem(1);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.H))
        {
            Player.GemTracker.RemoveGem(1);
        }
    }

    private void FirstUpdate()
    {
    }
}