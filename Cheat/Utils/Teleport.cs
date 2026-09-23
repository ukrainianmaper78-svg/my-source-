// C# / Teleport.cs / Teleport System / Runtime: Unity 2021+
// CoreDev - Player Teleportation with Noclip

using System;
using UnityEngine;

namespace Cheat.Utils
{
    public class Teleport : MonoBehaviour
    {
        // Settings
        public static class Settings
        {
            public static bool Enabled = false;
            public static float TeleportDistance = 10f;
            public static bool NoclipEnabled = false;
            public static KeyCode TeleportForwardKey = KeyCode.T;
            public static KeyCode TeleportBackwardKey = KeyCode.G;
            public static KeyCode TeleportUpKey = KeyCode.Y;
            public static KeyCode TeleportDownKey = KeyCode.H;
            public static KeyCode NoclipToggleKey = KeyCode.N;
        }
        
        private Camera _mainCamera;
        private MonoBehaviour _localPlayer;
        private bool _noclipActive = false;
        private CharacterController _characterController;
        private Collider _collider;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            
            // Find local player
            _localPlayer = FindObjectOfType<MonoBehaviour>();
            foreach (var mb in FindObjectsOfType<MonoBehaviour>())
            {
                if (mb.GetType().Name == "FPSPlayer")
                {
                    _localPlayer = mb;
                    break;
                }
            }
            
            if (_localPlayer != null)
            {
                _characterController = _localPlayer.GetComponent<CharacterController>();
                _collider = _localPlayer.GetComponent<Collider>();
            }
            
            if (_mainCamera == null)
            {
                Debug.LogError("[CoreDev] Teleport: No main camera found!");
                enabled = false;
            }
        }
        
        private void Update()
        {
            // Toggle noclip
            if (Input.GetKeyDown(Settings.NoclipToggleKey))
            {
                _noclipActive = !_noclipActive;
                Debug.Log("[CoreDev] Noclip: " + (_noclipActive ? "Enabled" : "Disabled"));
            }
            
            // Noclip movement
            if (Settings.NoclipEnabled && _noclipActive && _characterController != null)
            {
                NoclipMovement();
            }
            
            // Teleport
            if (Settings.Enabled)
            {
                HandleTeleport();
            }
        }
        
        private void NoclipMovement()
        {
            if (_characterController == null) return;
            
            // Disable collision
            _characterController.enabled = false;
            
            // Get input
            var moveSpeed = 10f;
            var moveDirection = Vector3.zero;
            
            if (Input.GetKey(KeyCode.W)) moveDirection += Vector3.forward;
            if (Input.GetKey(KeyCode.S)) moveDirection += Vector3.back;
            if (Input.GetKey(KeyCode.A)) moveDirection += Vector3.left;
            if (Input.GetKey(KeyCode.D)) moveDirection += Vector3.right;
            if (Input.GetKey(KeyCode.Space)) moveDirection += Vector3.up;
            if (Input.GetKey(KeyCode.C)) moveDirection += Vector3.down;
            
            moveDirection.Normalize();
            
            // Apply movement
            if (_localPlayer != null)
            {
                var transform = _localPlayer.transform;
                transform.position += transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;
            }
            
            // Re-enable collision at end of frame
            _characterController.enabled = true;
        }
        
        private void HandleTeleport()
        {
            if (_mainCamera == null || _localPlayer == null) return;
            
            var transform = _localPlayer.transform;
            var cameraForward = _mainCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            
            // Teleport forward
            if (Input.GetKeyDown(Settings.TeleportForwardKey))
            {
                transform.position += cameraForward * Settings.TeleportDistance;
                Debug.Log("[CoreDev] Teleport: Forward");
            }
            
            // Teleport backward
            if (Input.GetKeyDown(Settings.TeleportBackwardKey))
            {
                transform.position -= cameraForward * Settings.TeleportDistance;
                Debug.Log("[CoreDev] Teleport: Backward");
            }
            
            // Teleport up
            if (Input.GetKeyDown(Settings.TeleportUpKey))
            {
                transform.position += Vector3.up * Settings.TeleportDistance;
                Debug.Log("[CoreDev] Teleport: Up");
            }
            
            // Teleport down
            if (Input.GetKeyDown(Settings.TeleportDownKey))
            {
                transform.position -= Vector3.up * Settings.TeleportDistance;
                Debug.Log("[CoreDev] Teleport: Down");
            }
        }
        
        // Teleport to position
        public static void TeleportTo(Vector3 position)
        {
            if (!Settings.Enabled) return;
            
            var localPlayer = FindObjectOfType<MonoBehaviour>();
            foreach (var mb in FindObjectsOfType<MonoBehaviour>())
            {
                if (mb.GetType().Name == "FPSPlayer")
                {
                    localPlayer = mb;
                    break;
                }
            }
            
            if (localPlayer != null)
            {
                localPlayer.transform.position = position;
                Debug.Log("[CoreDev] Teleport: To " + position);
            }
        }
        
        // Teleport to player
        public static void TeleportToPlayer(string playerName)
        {
            if (!Settings.Enabled) return;
            
            var target = FindPlayerByName(playerName);
            if (target != null)
            {
                TeleportTo(target.transform.position);
            }
        }
        
        private static MonoBehaviour FindPlayerByName(string name)
        {
            foreach (var mb in FindObjectsOfType<MonoBehaviour>())
            {
                if (mb.gameObject.name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    return mb;
                }
            }
            return null;
        }
    }
}
