// C# / NoSpread.cs / No Spread System / Runtime: Unity 2021+
// CoreDev - Bullet Spread Removal

using System;
using UnityEngine;

namespace Cheat.Utils
{
    public class NoSpread : MonoBehaviour
    {
        // Settings
        public static class Settings
        {
            public static bool Enabled = false;
            public static float SpreadReduction = 100f; // 100% = full spread removal
            public static bool AffectAllWeapons = true;
        }
        
        private void Awake()
        {
            Debug.Log("[CoreDev] NoSpread: Initialized");
        }
        
        // Hook into weapon fire to remove spread
        public Vector3 OnBulletFire(Vector3 originalDirection, string weaponName)
        {
            if (!Settings.Enabled) return originalDirection;
            
            // Get weapon spread
            var spread = GetWeaponSpread(weaponName);
            
            // Reduce spread
            var reducedSpread = spread * (1f - Settings.SpreadReduction / 100f);
            
            // Apply reduced spread
            var newDirection = originalDirection;
            newDirection += UnityEngine.Random.insideUnitSphere * reducedSpread;
            newDirection.Normalize();
            
            return newDirection;
        }
        
        private float GetWeaponSpread(string weaponName)
        {
            // In a real implementation, we'd get the actual spread from the weapon
            // For now, return a default value
            
            // Common weapon spreads (in degrees)
            switch (weaponName.ToLower())
            {
                case "pistol": return 2f;
                case "rifle": return 0.5f;
                case "shotgun": return 10f;
                case "sniper": return 0.1f;
                default: return 1f;
            }
        }
        
        // Hook into weapon update to modify spread
        public void OnWeaponUpdate(ref float spread, string weaponName)
        {
            if (!Settings.Enabled) return;
            
            spread *= (1f - Settings.SpreadReduction / 100f);
        }
    }
}
