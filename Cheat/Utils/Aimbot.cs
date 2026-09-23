// C# / Aimbot.cs / Aimbot System / Runtime: Unity 2021+
// CoreDev - Advanced Aimbot with Smooth Aim, Bone Targeting, FOV Check

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cheat.Utils
{
    public class Aimbot : MonoBehaviour
    {
        // Settings
        public static class Settings
        {
            public static bool Enabled = false;
            public static KeyCode AimKey = KeyCode.Mouse2;
            public static float FOV = 90f;
            public static bool SmoothAim = true;
            public static float SmoothAmount = 10f;
            public static TargetBone Bone = TargetBone.Head;
            public static bool ThroughWalls = false;
            public static bool PredictMovement = false;
            public static bool AutoFire = false;
        }
        
        public enum TargetBone
        {
            Head,
            Neck,
            Chest,
            Hips,
            Random
        }
        
        // Target data
        private class TargetInfo
        {
            public Transform Transform;
            public Vector3 BonePosition;
            public float Distance;
            public float AngleToTarget;
            public bool IsVisible;
        }
        
        private Camera _mainCamera;
        private List<TargetInfo> _targets = new List<TargetInfo>();
        private TargetInfo _currentTarget;
        private float _nextScanTime = 0f;
        private float _scanInterval = 0.1f;
        
        // Bone names
        private static readonly string[] _boneNames = new string[]
        {
            "Head",
            "Neck",
            "Spine2",
            "Hips"
        };
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
            {
                Debug.LogError("[CoreDev] Aimbot: No main camera found!");
                enabled = false;
            }
        }
        
        private void Update()
        {
            if (!Settings.Enabled) return;
            
            // Scan for targets
            if (Time.time >= _nextScanTime)
            {
                ScanForTargets();
                _nextScanTime = Time.time + _scanInterval;
            }
            
            // Check aim key
            if (Input.GetKey(Settings.AimKey) && _currentTarget != null)
            {
                AimAtTarget();
            }
            
            // Auto fire
            if (Settings.AutoFire && Input.GetKey(Settings.AimKey) && _currentTarget != null)
            {
                SimulateMouseClick();
            }
        }
        
        private void ScanForTargets()
        {
            _targets.Clear();
            _currentTarget = null;
            
            if (_mainCamera == null) return;
            
            // Find all potential targets (players)
            var allObjects = FindObjectsOfType<MonoBehaviour>();
            
            foreach (var obj in allObjects)
            {
                if (IsValidTarget(obj.transform))
                {
                    var targetInfo = new TargetInfo
                    {
                        Transform = obj.transform,
                        Distance = Vector3.Distance(_mainCamera.transform.position, obj.transform.position),
                        IsVisible = IsTargetVisible(obj.transform)
                    };
                    
                    // Get bone position
                    targetInfo.BonePosition = GetBonePosition(obj.transform, Settings.Bone);
                    
                    // Calculate angle to target
                    targetInfo.AngleToTarget = GetAngleToTarget(targetInfo.BonePosition);
                    
                    // Check FOV
                    if (targetInfo.AngleToTarget <= Settings.FOV / 2f || !Settings.Enabled)
                    {
                        _targets.Add(targetInfo);
                    }
                }
            }
            
            // Select best target
            if (_targets.Count > 0)
            {
                _currentTarget = GetBestTarget();
            }
        }
        
        private bool IsValidTarget(Transform transform)
        {
            if (transform == null) return false;
            if (transform == _mainCamera.transform) return false;
            if (transform.name.Contains("Player") || transform.name.Contains("Character"))
            {
                // Additional checks to filter out non-player objects
                return true;
            }
            return false;
        }
        
        private bool IsTargetVisible(Transform transform)
        {
            if (Settings.ThroughWalls) return true;
            
            if (transform == null) return false;
            
            var direction = transform.position - _mainCamera.transform.position;
            if (Physics.Raycast(_mainCamera.transform.position, direction.normalized, out var hit, direction.magnitude))
            {
                // Check if ray hit the target
                var renderer = transform.GetComponentInChildren<Renderer>();
                if (renderer != null && hit.collider.gameObject == renderer.gameObject)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
        
        private Vector3 GetBonePosition(Transform transform, TargetBone bone)
        {
            var boneIndex = (int)bone;
            if (boneIndex >= 0 && boneIndex < _boneNames.Length)
            {
                var boneTransform = FindDeepChild(transform, _boneNames[boneIndex]);
                if (boneTransform != null)
                {
                    return boneTransform.position;
                }
            }
            
            // Fallback to head position
            return GetHeadPosition(transform);
        }
        
        private Vector3 GetHeadPosition(Transform transform)
        {
            var head = FindDeepChild(transform, "Head");
            if (head != null) return head.position;
            
            var collider = transform.GetComponent<Collider>();
            if (collider != null)
            {
                return collider.bounds.center + Vector3.up * collider.bounds.extents.y;
            }
            
            return transform.position + Vector3.up * 1.8f;
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
        
        private float GetAngleToTarget(Vector3 targetPosition)
        {
            var direction = targetPosition - _mainCamera.transform.position;
            var viewDirection = _mainCamera.transform.forward;
            
            var angle = Vector3.Angle(viewDirection, direction);
            return angle;
        }
        
        private TargetInfo GetBestTarget()
        {
            TargetInfo bestTarget = null;
            float bestScore = float.MaxValue;
            
            foreach (var target in _targets)
            {
                // Simple scoring: lower distance = better
                var score = target.Distance;
                
                // Prefer visible targets
                if (!target.IsVisible && !Settings.ThroughWalls)
                {
                    score += 10000f;
                }
                
                // Prefer targets closer to center of screen
                var screenPos = _mainCamera.WorldToScreenPoint(target.BonePosition);
                var centerDist = Vector2.Distance(screenPos, new Vector2(Screen.width / 2f, Screen.height / 2f));
                score += centerDist * 0.1f;
                
                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = target;
                }
            }
            
            return bestTarget;
        }
        
        private void AimAtTarget()
        {
            if (_currentTarget == null || _mainCamera == null) return;
            
            var targetPosition = _currentTarget.BonePosition;
            
            // Predict movement if enabled
            if (Settings.PredictMovement)
            {
                var rigidbody = _currentTarget.Transform.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    targetPosition += rigidbody.velocity * 0.1f;
                }
            }
            
            // Get direction to target
            var direction = targetPosition - _mainCamera.transform.position;
            direction.Normalize();
            
            // Calculate target rotation
            var targetRotation = Quaternion.LookRotation(direction);
            var currentRotation = _mainCamera.transform.rotation;
            
            if (Settings.SmoothAim)
            {
                // Smooth aim
                var smoothFactor = Settings.SmoothAmount * Time.deltaTime;
                _mainCamera.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, smoothFactor);
            }
            else
            {
                // Instant aim
                _mainCamera.transform.rotation = targetRotation;
            }
        }
        
        private void SimulateMouseClick()
        {
            // Simulate left mouse button press
            var mouseDownEvent = new InputEvent
            {
                type = InputEventType.MouseButtonDown,
                button = 0 // Left mouse button
            };
            
            // This would be handled by input system
            // In actual implementation, we'd use InputSimulator or similar
        }
        
        // Input event struct for simulation
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
