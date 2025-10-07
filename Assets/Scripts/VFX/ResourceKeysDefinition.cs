using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Resources
{
    [CreateAssetMenu(fileName = "NewResourceKeysDefinition", menuName = "Resource Keys Definition", order = 0)]
    public class ResourceKeysDefinition : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Key", ValueLabel = "Effect")]
        public Dictionary<string, GameObject> Resources = new Dictionary<string, GameObject>();
    }
}
