using System.Collections.Generic;
using Project;
using Project.Decks;
using Project.Items;
using Project.UI.MainUI;
using UnityEngine;

public class Scratch : MonoBehaviour
{
    [SerializeField] UIShaker Shaker;
    private bool firstUpdate;
    private bool lmbClicked;

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
            }
        }
        else
        {
            lmbClicked = false;
        }
    }

    private void FirstUpdate()
    {

    }
}