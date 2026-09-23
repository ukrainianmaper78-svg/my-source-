// C# / CheatLoader.cs / Main Cheat Loader / Runtime: Unity 2021+
// CoreDev - Main Cheat Initialization and Management

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cheat
{
    public class CheatLoader : MonoBehaviour
    {
        private static bool _initialized = false;
        private static CheatLoader _instance;
        
        // Cheat components
        private Hooks.UnityHooks _unityHooks;
        private ESP.ESPManager _espManager;
        private UI.OverlayGUI _overlayGUI;
        private UI.MenuGUI _menuGUI;
        private Utils.Aimbot _aimbot;
        private Utils.TriggerBot _triggerBot;
        private Utils.NoRecoil _noRecoil;
        private Utils.NoSpread _noSpread;
        private Utils.WallHack _wallHack;
        private Utils.SpeedHack _speedHack;
        private Utils.Teleport _teleport;
        
        private void Awake()
        {
            // Singleton pattern
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeCheat();
        }
        
        private void InitializeCheat()
        {
            if (_initialized) return;
            
            _initialized = true;
            Debug.Log("[CoreDev] CheatLoader: Initializing...");
            
            try
            {
                // Initialize hooks
                Hooks.UnityHooks.Initialize();
                _unityHooks = Hooks.UnityHooks.Instance;
                
                // Create ESP Manager if not exists
                if (_espManager == null)
                {
                    var espObj = new GameObject("ESPManager");
                    _espManager = espObj.AddComponent<ESP.ESPManager>();
                    DontDestroyOnLoad(espObj);
                }
                
                // Create Overlay GUI if not exists
                if (_overlayGUI == null)
                {
                    var guiObj = new GameObject("OverlayGUI");
                    _overlayGUI = guiObj.AddComponent<UI.OverlayGUI>();
                    DontDestroyOnLoad(guiObj);
                }
                
                // Create Menu GUI if not exists
                if (_menuGUI == null)
                {
                    var menuObj = new GameObject("MenuGUI");
                    _menuGUI = menuObj.AddComponent<UI.MenuGUI>();
                    DontDestroyOnLoad(menuObj);
                }
                
                // Create utility components
                CreateUtilityComponents();
                
                // Hook into scene changes
                SceneManager.sceneLoaded += OnSceneLoaded;
                
                Debug.Log("[CoreDev] CheatLoader: Initialization Complete!");
            }
            catch (Exception ex)
            {
                Debug.LogError("[CoreDev] CheatLoader: Initialization Failed! " + ex.Message);
            }
        }
        
        private void CreateUtilityComponents()
        {
            // Aimbot
            var aimbotObj = new GameObject("Aimbot");
            _aimbot = aimbotObj.AddComponent<Utils.Aimbot>();
            DontDestroyOnLoad(aimbotObj);
            
            // Trigger Bot
            var triggerBotObj = new GameObject("TriggerBot");
            _triggerBot = triggerBotObj.AddComponent<Utils.TriggerBot>();
            DontDestroyOnLoad(triggerBotObj);
            
            // No Recoil
            var noRecoilObj = new GameObject("NoRecoil");
            _noRecoil = noRecoilObj.AddComponent<Utils.NoRecoil>();
            DontDestroyOnLoad(noRecoilObj);
            
            // No Spread
            var noSpreadObj = new GameObject("NoSpread");
            _noSpread = noSpreadObj.AddComponent<Utils.NoSpread>();
            DontDestroyOnLoad(noSpreadObj);
            
            // Wall Hack
            var wallHackObj = new GameObject("WallHack");
            _wallHack = wallHackObj.AddComponent<Utils.WallHack>();
            DontDestroyOnLoad(wallHackObj);
            
            // Speed Hack
            var speedHackObj = new GameObject("SpeedHack");
            _speedHack = speedHackObj.AddComponent<Utils.SpeedHack>();
            DontDestroyOnLoad(speedHackObj);
            
            // Teleport
            var teleportObj = new GameObject("Teleport");
            _teleport = teleportObj.AddComponent<Utils.Teleport>();
            DontDestroyOnLoad(teleportObj);
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("[CoreDev] Scene Loaded: " + scene.name);
            
            // Re-hook components in new scene
            StartCoroutine(RehookAfterDelay(1f));
        }
        
        private IEnumerator RehookAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            // Re-initialize hooks for new scene
            Hooks.UnityHooks.Initialize();
            
            // Find and hook main menu if exists
            var mainMenu = FindObjectOfType<MonoBehaviour>();
            foreach (var mb in FindObjectsOfType<MonoBehaviour>())
            {
                if (mb.GetType().Name == "MainMenu")
                {
                    Hooks.UnityHooks.HookBehaviour(mb);
                    break;
                }
            }
            
            // Find and hook FPSPlayer if exists
            foreach (var mb in FindObjectsOfType<MonoBehaviour>())
            {
                if (mb.GetType().Name == "FPSPlayer")
                {
                    Hooks.UnityHooks.HookBehaviour(mb);
                }
            }
        }
        
        private void Update()
        {
            // Update all components
            if (_espManager != null)
                _espManager.Update();
        }
        
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
                _initialized = false;
                
                // Clean up
                SceneManager.sceneLoaded -= OnSceneLoaded;
                
                // Unhook all
                Hooks.UnityHooks.UnhookAll();
                
                Debug.Log("[CoreDev] CheatLoader: Cleanup Complete");
            }
        }
        
        // Public methods to access components
        public static ESP.ESPManager GetESPManager()
        {
            if (_instance != null)
                return _instance._espManager;
            return null;
        }
        
        public static UI.OverlayGUI GetOverlayGUI()
        {
            if (_instance != null)
                return _instance._overlayGUI;
            return null;
        }
        
        public static UI.MenuGUI GetMenuGUI()
        {
            if (_instance != null)
                return _instance._menuGUI;
            return null;
        }
        
        public static Utils.Aimbot GetAimbot()
        {
            if (_instance != null)
                return _instance._aimbot;
            return null;
        }
        
        public static Utils.TriggerBot GetTriggerBot()
        {
            if (_instance != null)
                return _instance._triggerBot;
            return null;
        }
        
        public static Utils.NoRecoil GetNoRecoil()
        {
            if (_instance != null)
                return _instance._noRecoil;
            return null;
        }
        
        public static Utils.NoSpread GetNoSpread()
        {
            if (_instance != null)
                return _instance._noSpread;
            return null;
        }
        
        public static Utils.WallHack GetWallHack()
        {
            if (_instance != null)
                return _instance._wallHack;
            return null;
        }
        
        public static Utils.SpeedHack GetSpeedHack()
        {
            if (_instance != null)
                return _instance._speedHack;
            return null;
        }
        
        public static Utils.Teleport GetTeleport()
        {
            if (_instance != null)
                return _instance._teleport;
            return null;
        }
    }
}
