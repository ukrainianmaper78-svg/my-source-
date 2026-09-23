// C# / PlayerESP.cs / Individual Player ESP / Runtime: Unity 2021+
// CoreDev - Per-Player ESP Component

using UnityEngine;

namespace Cheat.ESP
{
    public class PlayerESP : MonoBehaviour
    {
        public string PlayerName = "Player";
        public bool IsTeam = false;
        public float Health = 100f;
        public float MaxHealth = 100f;
        public string WeaponName = "Unknown";
        
        private ESPManager _espManager;
        private Transform _headBone;
        private Transform _rootBone;
        
        private void Awake()
        {
            _espManager = FindObjectOfType<ESPManager>();
            if (_espManager == null)
            {
                var espObj = new GameObject("ESPManager");
                _espManager = espObj.AddComponent<ESPManager>();
            }
            
            // Find bones
            _headBone = FindDeepChild(transform, "Head");
            _rootBone = FindDeepChild(transform, "Hips");
        }
        
        private void Update()
        {
            // Update player data in ESP manager
            if (_espManager != null)
            {
                // This will be handled by ESPManager's cache system
            }
        }
        
        private Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Contains(name, System.StringComparison.OrdinalIgnoreCase))
                {
                    return child;
                }
            }
            return null;
        }
    }
}
