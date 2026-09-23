// C# / UnityHooks.cs / Unity IMGUI Hooks / Runtime: Unity 2021+
// CoreDev - Unity IMGUI Hook System with ESP Integration

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Cheat.Hooks
{
    public static class UnityHooks
    {
        private static bool _initialized = false;
        private static bool _hooksInstalled = false;
        
        // Original delegates for method detouring
        private static Action _originalOnGUI;
        private static Action _originalUpdate;
        private static Action _originalLateUpdate;
        
        // Hooked components
        private static List<MonoBehaviour> _hookedBehaviours = new List<MonoBehaviour>();
        private static List<Camera> _hookedCameras = new List<Camera>();
        
        // ESP and UI references
        public static ESP.ESPManager ESPManager { get; private set; }
        public static UI.OverlayGUI OverlayGUI { get; private set; }
        
        // Main initialization
        public static void Initialize()
        {
            if (_initialized) return;
            
            _initialized = true;
            
            // Create ESP Manager
            var espGameObject = new GameObject("ESPManager");
            ESPManager = espGameObject.AddComponent<ESP.ESPManager>();
            UnityEngine.Object.DontDestroyOnLoad(espGameObject);
            
            // Create Overlay GUI
            var guiGameObject = new GameObject("OverlayGUI");
            OverlayGUI = guiGameObject.AddComponent<UI.OverlayGUI>();
            UnityEngine.Object.DontDestroyOnLoad(guiGameObject);
            
            // Hook main camera
            HookMainCamera();
            
            // Install method hooks
            InstallHooks();
            
            Debug.Log("[CoreDev] Unity Hooks Initialized");
        }
        
        // Hook main camera for ESP rendering
        private static void HookMainCamera()
        {
            var mainCamera = Camera.main;
            if (mainCamera != null && !_hookedCameras.Contains(mainCamera))
            {
                var cameraHook = mainCamera.gameObject.AddComponent<CameraHook>();
                cameraHook.Initialize(ESPManager);
                _hookedCameras.Add(mainCamera);
                Debug.Log("[CoreDev] Main Camera Hooked");
            }
        }
        
        // Install method hooks using Harmony-like detouring
        private static void InstallHooks()
        {
            if (_hooksInstalled) return;
            
            try
            {
                // Hook OnGUI for all MonoBehaviours
                var onGUIMethod = typeof(MonoBehaviour).GetMethod("OnGUI", BindingFlags.NonPublic | BindingFlags.Instance);
                if (onGUIMethod != null)
                {
                    _originalOnGUI = Delegate.CreateDelegate(typeof(Action), null, onGUIMethod) as Action;
                }
                
                // Hook Update
                var updateMethod = typeof(MonoBehaviour).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
                if (updateMethod != null)
                {
                    _originalUpdate = Delegate.CreateDelegate(typeof(Action), null, updateMethod) as Action;
                }
                
                _hooksInstalled = true;
                Debug.Log("[CoreDev] Method Hooks Installed");
            }
            catch (Exception ex)
            {
                Debug.LogError("[CoreDev] Hook Installation Failed: " + ex.Message);
            }
        }
        
        // Hook a specific MonoBehaviour
        public static void HookBehaviour(MonoBehaviour behaviour)
        {
            if (behaviour == null || _hookedBehaviours.Contains(behaviour)) return;
            
            _hookedBehaviours.Add(behaviour);
            var hookComponent = behaviour.gameObject.AddComponent<BehaviourHook>();
            hookComponent.Initialize(behaviour, ESPManager, OverlayGUI);
            
            Debug.Log("[CoreDev] Behaviour Hooked: " + behaviour.GetType().Name);
        }
        
        // Unhook all
        public static void UnhookAll()
        {
            foreach (var behaviour in _hookedBehaviours)
            {
                var hook = behaviour.GetComponent<BehaviourHook>();
                if (hook != null)
                    UnityEngine.Object.Destroy(hook);
            }
            _hookedBehaviours.Clear();
            
            foreach (var camera in _hookedCameras)
            {
                var hook = camera.GetComponent<CameraHook>();
                if (hook != null)
                    UnityEngine.Object.Destroy(hook);
            }
            _hookedCameras.Clear();
            
            if (ESPManager != null)
                UnityEngine.Object.Destroy(ESPManager.gameObject);
            
            if (OverlayGUI != null)
                UnityEngine.Object.Destroy(OverlayGUI.gameObject);
            
            _initialized = false;
            _hooksInstalled = false;
            
            Debug.Log("[CoreDev] All Hooks Removed");
        }
        
        // Camera hook component
        public class CameraHook : MonoBehaviour
        {
            private ESP.ESPManager _espManager;
            
            public void Initialize(ESP.ESPManager espManager)
            {
                _espManager = espManager;
            }
            
            private void OnPreRender()
            {
                if (_espManager != null)
                    _espManager.OnPreRender(Camera.current);
            }
            
            private void OnPostRender()
            {
                if (_espManager != null)
                    _espManager.OnPostRender(Camera.current);
            }
        }
        
        // Behaviour hook component
        public class BehaviourHook : MonoBehaviour
        {
            private MonoBehaviour _target;
            private ESP.ESPManager _espManager;
            private UI.OverlayGUI _overlayGUI;
            
            public void Initialize(MonoBehaviour target, ESP.ESPManager espManager, UI.OverlayGUI overlayGUI)
            {
                _target = target;
                _espManager = espManager;
                _overlayGUI = overlayGUI;
            }
            
            private void OnGUI()
            {
                if (_overlayGUI != null)
                    _overlayGUI.OnGUI();
            }
            
            private void Update()
            {
                if (_espManager != null)
                    _espManager.Update();
            }
        }
    }
}
