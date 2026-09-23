// C# / SpeedHack.cs / Speed Hack System / Runtime: Unity 2021+
// CoreDev - Player Movement Speed Modifier

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cheat.Utils
{
    public class SpeedHack : MonoBehaviour
    {
        // Settings
        public static class Settings
        {
            public static bool Enabled = false;
            public static float SpeedMultiplier = 2f; // 2x speed
            public static bool AffectAllPlayers = false;
            public static bool OnlyLocalPlayer = true;
            public static KeyCode ToggleKey = KeyCode.F;
        }
        
        private List<MonoBehaviour> _hookedPlayers = new List<MonoBehaviour>();
        private Dictionary<MonoBehaviour, float> _originalSpeeds = new Dictionary<MonoBehaviour, float>();
        
        private void Update()
        {
            // Toggle with key
            if (Input.GetKeyDown(Settings.ToggleKey))
            {
                Settings.Enabled = !Settings.Enabled;
                Debug.Log("[CoreDev] SpeedHack: " + (Settings.Enabled ? "Enabled" : "Disabled"));
            }
            
            if (Settings.Enabled)
            {
                ApplySpeedHack();
            }
            else
            {
                RemoveSpeedHack();
            }
        }
        
        private void ApplySpeedHack()
        {
            // Clear previous hooks
            RemoveSpeedHack();
            
            if (!Settings.Enabled) return;
            
            // Find all players
            var allObjects = FindObjectsOfType<MonoBehaviour>();
            
            foreach (var obj in allObjects)
            {
                var typeName = obj.GetType().Name;
                
                // Check if it's a player
                if (typeName.Contains("Player") || typeName.Contains("Character"))
                {
                    // Check if we should affect this player
                    var isLocal = IsLocalPlayer(obj);
                    
                    if ((Settings.OnlyLocalPlayer && isLocal) || (!Settings.OnlyLocalPlayer && Settings.AffectAllPlayers))
                    {
                        // Hook the player
                        _hookedPlayers.Add(obj);
                        
                        // Store original speed
                        var originalSpeed = GetPlayerSpeed(obj);
                        _originalSpeeds[obj] = originalSpeed;
                        
                        // Apply speed multiplier
                        SetPlayerSpeed(obj, originalSpeed * Settings.SpeedMultiplier);
                    }
                }
            }
        }
        
        private void RemoveSpeedHack()
        {
            foreach (var player in _hookedPlayers)
            {
                if (player != null && _originalSpeeds.ContainsKey(player))
                {
                    SetPlayerSpeed(player, _originalSpeeds[player]);
                }
            }
            
            _hookedPlayers.Clear();
            _originalSpeeds.Clear();
        }
        
        private bool IsLocalPlayer(MonoBehaviour player)
        {
            // In a real implementation, we'd check if this is the local player
            // For now, assume the first player found is local
            var fpsPlayer = player.GetComponent<MonoBehaviour>();
            if (fpsPlayer != null && fpsPlayer.GetType().Name == "FPSPlayer")
            {
                return true;
            }
            return false;
        }
        
        private float GetPlayerSpeed(MonoBehaviour player)
        {
            // Try to get speed from various components
            
            // Check for CharacterController
            var controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Speed is usually controlled by a script, not the controller itself
            }
            
            // Check for Rigidbody
            var rigidbody = player.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                return rigidbody.velocity.magnitude;
            }
            
            // Try to get from PlayerController or similar
            var playerController = player.GetComponentInChildren<MonoBehaviour>();
            if (playerController != null)
            {
                var type = playerController.GetType();
                var speedField = type.GetField("moveSpeed");
                if (speedField != null)
                {
                    return (float)speedField.GetValue(playerController);
                }
                
                speedField = type.GetField("speed");
                if (speedField != null)
                {
                    return (float)speedField.GetValue(playerController);
                }
            }
            
            return 5f; // Default speed
        }
        
        private void SetPlayerSpeed(MonoBehaviour player, float speed)
        {
            // Try to set speed on various components
            
            // Try to set on PlayerController or similar
            var playerController = player.GetComponentInChildren<MonoBehaviour>();
            if (playerController != null)
            {
                var type = playerController.GetType();
                var speedField = type.GetField("moveSpeed");
                if (speedField != null)
                {
                    speedField.SetValue(playerController, speed);
                    return;
                }
                
                speedField = type.GetField("speed");
                if (speedField != null)
                {
                    speedField.SetValue(playerController, speed);
                    return;
                }
            }
            
            // Try to set on Rigidbody
            var rigidbody = player.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                // This won't work for continuous speed modification
                // but we can try to modify velocity
                var velocity = rigidbody.velocity;
                if (velocity.magnitude > 0.1f)
                {
                    velocity = velocity.normalized * speed;
                    rigidbody.velocity = velocity;
                }
            }
        }
        
        private void OnDisable()
        {
            RemoveSpeedHack();
        }
        
        private void OnDestroy()
        {
            RemoveSpeedHack();
        }
    }
}
