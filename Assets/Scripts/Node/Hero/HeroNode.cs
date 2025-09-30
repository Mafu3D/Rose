using UnityEngine;
using Project.PlayerSystem;
using System;
using Project.Attributes;

namespace Project.GameNode.Hero
{
    public class HeroNode : Node
    {
        [SerializeField] public Player Player;
    }
}