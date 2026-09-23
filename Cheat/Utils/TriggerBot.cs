// C# / TriggerBot.cs / Trigger Bot System / Runtime: Unity 2021+
// CoreDev - Automatic Trigger Bot with Delay and Randomization

using System;
using UnityEngine;

namespace Cheat.Utils
{
    public class TriggerBot : MonoBehaviour
    {
        // Settings
        public static class Settings
        {
            public static bool Enabled = false;
            public static KeyCode TriggerKey = KeyCode.Mouse0;
            public static float ActivationDelay = 0.05f; // 50ms delay
            public static float Randomization = 0.02f; // 20ms randomization
            public static bool OnlyHeadshots = true;
            public static bool OnlyVisible = true;
            public static float MaxDistance = 500f;
        }
        
        private Camera _mainCamera;
        private bool _shouldFire = false;
        private float _fireTime = 0f;
        private float _lastFireTime = 0f;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
            {
                Debug.LogError("[CoreDev] TriggerBot: No main camera found!");
                enabled = false;
            }
        }
        
        private void Update()
        {
            if (!Settings.Enabled) return;
            
            // Check if trigger key is pressed
            if (Input.GetKey(Settings.TriggerKey))
            {
                CheckForTarget();
            }
            
            // Fire if needed
            if (_shouldFire && Time.time >= _fireTime)
            {
                SimulateFire();
                _shouldFire = false;
                _lastFireTime = Time.time;
            }
        }
        
        private void CheckForTarget()
        {
            if (_mainCamera == null) return;
            
            // Raycast from camera
            var ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            
            if (Physics.Raycast(ray, out var hit, Settings.MaxDistance))
            {
                var target = hit.collider.gameObject;
                
                // Check if it's a valid target
                if (IsValidTarget(target))
                {
                    // Check if it's a headshot (if enabled)
                    if (Settings.OnlyHeadshots && !IsHeadshot(hit))
                    {
                        return;
                    }
                    
                    // Check visibility (if enabled)
                    if (Settings.OnlyVisible && !IsVisible(target))
                    {
                        return;
                    }
                    
                    // Calculate randomized delay
                    var delay = Settings.ActivationDelay + UnityEngine.Random.Range(-Settings.Randomization, Settings.Randomization);
                    _fireTime = Time.time + delay;
                    _shouldFire = true;
                }
            }
        }
        
        private bool IsValidTarget(GameObject target)
        {
            if (target == null) return false;
            
            // Check if it's a player
            var player = target.GetComponentInParent<MonoBehaviour>();
            if (player != null)
            {
                var typeName = player.GetType().Name;
                if (typeName.Contains("Player") || typeName.Contains("Character"))
                {
                    return true;
                }
            }
            
            // Check if it's an enemy
            var enemy = target.GetComponentInParent<MonoBehaviour>();
            if (enemy != null)
            {
                var typeName = enemy.GetType().Name;
                if (typeName.Contains("Enemy") || typeName.Contains("Zombie"))
                {
                    return true;
                }
            }
            
            return false;
        }
        
        private bool IsHeadshot(RaycastHit hit)
        {
            var target = hit.collider.gameObject;
            var head = FindDeepChild(target.transform, "Head");
            
            if (head != null)
            {
                // Check if hit point is near head
                var distance = Vector3.Distance(hit.point, head.position);
                return distance < 0.3f; // 30cm tolerance
            }
            
            // Check if hit collider is head collider
            if (hit.collider.name.Contains("Head", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            return false;
        }
        
        private Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    return child;
                }
            }
            return null;
        }
        
        private bool IsVisible(GameObject target)
        {
            if (!Settings.OnlyVisible) return true;
            
            var renderer = target.GetComponentInChildren<Renderer>();
            if (renderer == null) return true;
            
            // Simple visibility check
            return renderer.isVisible;
        }
        
        private void SimulateFire()
        {
            // Simulate left mouse button press and release
            Debug.Log("[CoreDev] TriggerBot: Firing!");
            
            // In actual implementation, we'd use:
            // 1. InputSimulator to simulate mouse clicks
            // 2. Or directly call weapon fire methods
            // 3. Or use Unity's Input system
            
            // This is a placeholder for the actual implementation
            var fireEvent = new InputEvent
            {
                type = InputEventType.MouseButtonDown,
                button = 0
            };
            
            // Process fire event
            ProcessInputEvent(fireEvent);
            
            // Release after a frame
            var releaseEvent = new InputEvent
            {
                type = InputEventType.MouseButtonUp,
                button = 0
            };
            
            ProcessInputEvent(releaseEvent);
        }
        
        private void ProcessInputEvent(InputEvent evnt)
        {
            // This would be handled by the input system
            // In a real implementation, we'd inject the event into Unity's input system
        }
        
        // Input event struct
        private struct InputEvent
        {
            public InputEventType type;
            public int button;
        }
        
        private enum InputEventType
        {
            MouseButtonDown,
            MouseButtonUp,
            KeyDown,
            KeyUp
        }
    }
}
