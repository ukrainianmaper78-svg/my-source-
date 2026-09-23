// C# / NoRecoil.cs / No Recoil System / Runtime: Unity 2021+
// CoreDev - Weapon Recoil Cancellation

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cheat.Utils
{
    public class NoRecoil : MonoBehaviour
    {
        // Settings
        public static class Settings
        {
            public static bool Enabled = false;
            public static float RecoilReduction = 100f; // 100% = full recoil removal
            public static bool AffectVertical = true;
            public static bool AffectHorizontal = true;
            public static bool AffectAllWeapons = true;
            public static List<string> WeaponBlacklist = new List<string>();
        }
        
        private Camera _mainCamera;
        private Vector3 _originalRotation;
        private Vector3 _recoilOffset = Vector3.zero;
        private float _nextUpdateTime = 0f;
        private float _updateInterval = 0.01f; // 10ms
        
        // Weapon data
        private class WeaponData
        {
            public string Name;
            public float VerticalRecoil;
            public float HorizontalRecoil;
            public float CurrentRecoil;
        }
        
        private Dictionary<string, WeaponData> _weaponData = new Dictionary<string, WeaponData>();
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
            {
                Debug.LogError("[CoreDev] NoRecoil: No main camera found!");
                enabled = false;
            }
            
            _originalRotation = _mainCamera.transform.localEulerAngles;
        }
        
        private void Update()
        {
            if (!Settings.Enabled || _mainCamera == null) return;
            
            // Update recoil compensation
            if (Time.time >= _nextUpdateTime)
            {
                CompensateRecoil();
                _nextUpdateTime = Time.time + _updateInterval;
            }
        }
        
        private void CompensateRecoil()
        {
            if (_mainCamera == null) return;
            
            // Get current rotation
            var currentRotation = _mainCamera.transform.localEulerAngles;
            
            // Calculate recoil offset
            var verticalOffset = currentRotation.x - _originalRotation.x;
            var horizontalOffset = currentRotation.y - _originalRotation.y;
            
            // Apply compensation
            var compensationX = 0f;
            var compensationY = 0f;
            
            if (Settings.AffectVertical)
            {
                compensationX = -verticalOffset * (Settings.RecoilReduction / 100f);
            }
            
            if (Settings.AffectHorizontal)
            {
                compensationY = -horizontalOffset * (Settings.RecoilReduction / 100f);
            }
            
            // Apply compensation
            _mainCamera.transform.localEulerAngles = new Vector3(
                currentRotation.x + compensationX,
                currentRotation.y + compensationY,
                currentRotation.z);
        }
        
        // Hook into weapon fire to capture recoil
        public void OnWeaponFire(string weaponName)
        {
            if (!Settings.Enabled) return;
            
            // Get or create weapon data
            if (!_weaponData.ContainsKey(weaponName))
            {
                _weaponData[weaponName] = new WeaponData
                {
                    Name = weaponName,
                    VerticalRecoil = 1f,
                    HorizontalRecoil = 1f
                };
            }
            
            var weapon = _weaponData[weaponName];
            
            // In a real implementation, we'd capture the actual recoil values
            // from the weapon's fire method
        }
        
        // Hook into weapon update to track recoil
        public void OnWeaponUpdate(string weaponName, Vector3 recoilAmount)
        {
            if (!_weaponData.ContainsKey(weaponName)) return;
            
            var weapon = _weaponData[weaponName];
            weapon.CurrentRecoil = recoilAmount.magnitude;
        }
    }
}
