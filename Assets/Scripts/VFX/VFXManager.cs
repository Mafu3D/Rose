using System;
using Project;
using Project.Resources;
using Project.UI.BattleUI;
using UnityEngine;

namespace Project.VFX
{
    public class VFXManager : Singleton<VFXManager>
    {

        [Header("VFX")]
        [SerializeField] public ResourceKeysDefinition GlobalVFXResources;
        [SerializeField] public BattleUI battleUI;

        public event Action<GameObject> OnPlayStatusEffectVFX;

        public void PlayStatusEffectVFX(string key, Character target)
        {
            GameObject vfxPrefab;
            if (GlobalVFXResources.Resources.TryGetValue(key, out vfxPrefab))
            {
                if (vfxPrefab != null)
                {
                    battleUI.PlayEffectVFX(vfxPrefab, target);
                }
            }
        }
    }
}
