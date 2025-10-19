using System;
using Project;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] Image image;

    void Awake()
    {
        // GameManager.Instance.OnGameStartEvent += SetBossImage;
    }

    void Start()
    {
        GameManager.Instance.OnGameStartEvent += SetBossImage;
    }

    private void SetBossImage()
    {
        image.sprite = GameManager.Instance.Boss.CombatSprite;
    }
}
