// C# / OverlayGUI.cs / IMGUI Overlay System / Runtime: Unity 2021+
// CoreDev - Complete IMGUI Overlay with Menu, ESP Toggle, Aimbot, etc.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cheat.UI
{
    public class OverlayGUI : MonoBehaviour
    {
        // GUI State
        private bool _menuVisible = true;
        private bool _espMenuVisible = false;
        private bool _aimbotMenuVisible = false;
        private bool _miscMenuVisible = false;
        private bool _playerListVisible = false;
        
        // Drag and drop
        private Rect _menuRect = new Rect(10, 10, 300, 400);
        private bool _isDragging = false;
        private Vector2 _dragStartPos;
        private Rect _dragStartRect;
        
        // Styles
        private GUIStyle _windowStyle;
        private GUIStyle _buttonStyle;
        private GUIStyle _toggleStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _sliderStyle;
        private GUIStyle _textFieldStyle;
        
        // Textures
        private Texture2D _windowTexture;
        private Texture2D _buttonTexture;
        
        private void Awake()
        {
            // Create textures
            _windowTexture = MakeTexture(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.95f));
            _buttonTexture = MakeTexture(2, 2, new Color(0.2f, 0.2f, 0.2f, 1f));
            
            // Setup styles
            SetupStyles();
        }
        
        private void SetupStyles()
        {
            // Window style
            _windowStyle = new GUIStyle(GUI.skin.window);
            _windowStyle.normal.background = _windowTexture;
            _windowStyle.fontSize = 14;
            _windowStyle.normal.textColor = Color.white;
            _windowStyle.border = new RectOffset(12, 12, 20, 20);
            
            // Button style
            _buttonStyle = new GUIStyle(GUI.skin.button);
            _buttonStyle.normal.background = _buttonTexture;
            _buttonStyle.normal.textColor = Color.white;
            _buttonStyle.hover.textColor = Color.cyan;
            _buttonStyle.active.textColor = Color.green;
            _buttonStyle.fontSize = 13;
            _buttonStyle.padding = new RectOffset(5, 5, 3, 3);
            _buttonStyle.margin = new RectOffset(2, 2, 2, 2);
            
            // Toggle style
            _toggleStyle = new GUIStyle(GUI.skin.toggle);
            _toggleStyle.normal.textColor = Color.white;
            _toggleStyle.fontSize = 13;
            
            // Label style
            _labelStyle = new GUIStyle(GUI.skin.label);
            _labelStyle.normal.textColor = Color.white;
            _labelStyle.fontSize = 13;
            
            // Slider style
            _sliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
            
            // Text field style
            _textFieldStyle = new GUIStyle(GUI.skin.textField);
            _textFieldStyle.normal.textColor = Color.white;
        }
        
        private Texture2D MakeTexture(int width, int height, Color color)
        {
            var tex = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    tex.SetPixel(x, y, color);
            tex.Apply();
            return tex;
        }
        
        public void OnGUI()
        {
            if (!Cheat.ESP.ESPManager.Settings.Enabled) return;
            
            // Toggle menu with INSERT key
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Insert)
            {
                _menuVisible = !_menuVisible;
                Event.current.Use();
            }
            
            if (_menuVisible)
            {
                DrawMainMenu();
            }
            
            // Draw player info on screen
            DrawPlayerInfo();
            
            // Draw crosshair if enabled
            if (Settings.ShowCrosshair)
            {
                DrawCrosshair();
            }
        }
        
        private void DrawMainMenu()
        {
            _menuRect = GUI.Window(0, _menuRect, DrawMenuWindow, "CoreDev Cheat Menu", _windowStyle);
        }
        
        private void DrawMenuWindow(int windowID)
        {
            // Enable dragging
            if (Event.current.type == EventType.MouseDown && _menuRect.Contains(Event.current.mousePosition))
            {
                _isDragging = true;
                _dragStartPos = Event.current.mousePosition;
                _dragStartRect = _menuRect;
                Event.current.Use();
            }
            
            if (Event.current.type == EventType.MouseDrag && _isDragging)
            {
                _menuRect.x = _dragStartRect.x + (Event.current.mousePosition.x - _dragStartPos.x);
                _menuRect.y = _dragStartRect.y + (Event.current.mousePosition.y - _dragStartPos.y);
                Event.current.Use();
            }
            
            if (Event.current.type == EventType.MouseUp)
            {
                _isDragging = false;
            }
            
            GUILayout.BeginVertical();
            
            // Tabs
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ESP", _buttonStyle, GUILayout.Height(30)))
                _espMenuVisible = !_espMenuVisible;
            if (GUILayout.Button("Aimbot", _buttonStyle, GUILayout.Height(30)))
                _aimbotMenuVisible = !_aimbotMenuVisible;
            if (GUILayout.Button("Misc", _buttonStyle, GUILayout.Height(30)))
                _miscMenuVisible = !_miscMenuVisible;
            if (GUILayout.Button("Players", _buttonStyle, GUILayout.Height(30)))
                _playerListVisible = !_playerListVisible;
            GUILayout.EndHorizontal();
            
            // ESP Menu
            if (_espMenuVisible)
            {
                DrawESPMenu();
            }
            
            // Aimbot Menu
            if (_aimbotMenuVisible)
            {
                DrawAimbotMenu();
            }
            
            // Misc Menu
            if (_miscMenuVisible)
            {
                DrawMiscMenu();
            }
            
            // Player List
            if (_playerListVisible)
            {
                DrawPlayerList();
            }
            
            // Close button
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close Menu", _buttonStyle, GUILayout.Width(120)))
            {
                _menuVisible = false;
            }
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }
        
        private void DrawESPMenu()
        {
            GUILayout.Label("--- ESP Settings ---", _labelStyle);
            
            Cheat.ESP.ESPManager.Settings.Enabled = GUILayout.Toggle(
                Cheat.ESP.ESPManager.Settings.Enabled, "ESP Enabled", _toggleStyle);
            
            Cheat.ESP.ESPManager.Settings.ShowBox = GUILayout.Toggle(
                Cheat.ESP.ESPManager.Settings.ShowBox, "Show Box", _toggleStyle);
            Cheat.ESP.ESPManager.Settings.ShowLine = GUILayout.Toggle(
                Cheat.ESP.ESPManager.Settings.ShowLine, "Show Line", _toggleStyle);
            Cheat.ESP.ESPManager.Settings.ShowName = GUILayout.Toggle(
                Cheat.ESP.ESPManager.Settings.ShowName, "Show Name", _toggleStyle);
            Cheat.ESP.ESPManager.Settings.ShowHealth = GUILayout.Toggle(
                Cheat.ESP.ESPManager.Settings.ShowHealth, "Show Health", _toggleStyle);
            Cheat.ESP.ESPManager.Settings.ShowSkeleton = GUILayout.Toggle(
                Cheat.ESP.ESPManager.Settings.ShowSkeleton, "Show Skeleton", _toggleStyle);
            Cheat.ESP.ESPManager.Settings.ShowDistance = GUILayout.Toggle(
                Cheat.ESP.ESPManager.Settings.ShowDistance, "Show Distance", _toggleStyle);
            Cheat.ESP.ESPManager.Settings.ShowWeapon = GUILayout.Toggle(
                Cheat.ESP.ESPManager.Settings.ShowWeapon, "Show Weapon", _toggleStyle);
            Cheat.ESP.ESPManager.Settings.ShowTeam = GUILayout.Toggle(
                Cheat.ESP.ESPManager.Settings.ShowTeam, "Show Team Info", _toggleStyle);
            
            GUILayout.Space(5);
            
            GUILayout.Label("Max Distance: " + Cheat.ESP.ESPManager.Settings.MaxDistance.ToString("F0"), _labelStyle);
            Cheat.ESP.ESPManager.Settings.MaxDistance = GUILayout.HorizontalSlider(
                Cheat.ESP.ESPManager.Settings.MaxDistance, 50f, 1000f, _sliderStyle);
            
            GUILayout.Space(10);
            
            GUILayout.Label("Box Color:", _labelStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label("R:", GUILayout.Width(20));
            Cheat.ESP.ESPManager.Settings.BoxColor.r = GUILayout.HorizontalSlider(
                Cheat.ESP.ESPManager.Settings.BoxColor.r, 0f, 1f, _sliderStyle);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("G:", GUILayout.Width(20));
            Cheat.ESP.ESPManager.Settings.BoxColor.g = GUILayout.HorizontalSlider(
                Cheat.ESP.ESPManager.Settings.BoxColor.g, 0f, 1f, _sliderStyle);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("B:", GUILayout.Width(20));
            Cheat.ESP.ESPManager.Settings.BoxColor.b = GUILayout.HorizontalSlider(
                Cheat.ESP.ESPManager.Settings.BoxColor.b, 0f, 1f, _sliderStyle);
            GUILayout.EndHorizontal();
        }
        
        private void DrawAimbotMenu()
        {
            GUILayout.Label("--- Aimbot Settings ---", _labelStyle);
            
            Settings.AimbotEnabled = GUILayout.Toggle(
                Settings.AimbotEnabled, "Aimbot Enabled", _toggleStyle);
            Settings.AimKey = (KeyCode)EditorGUILayout.EnumPopup("Aim Key:", Settings.AimKey);
            Settings.AimbotFOV = GUILayout.HorizontalSlider(
                Settings.AimbotFOV, 1f, 360f, _sliderStyle);
            GUILayout.Label("FOV: " + Settings.AimbotFOV.ToString("F0"), _labelStyle);
            
            Settings.AimbotSmooth = GUILayout.Toggle(
                Settings.AimbotSmooth, "Smooth Aim", _toggleStyle);
            Settings.AimbotSmoothAmount = GUILayout.HorizontalSlider(
                Settings.AimbotSmoothAmount, 1f, 50f, _sliderStyle);
            GUILayout.Label("Smooth: " + Settings.AimbotSmoothAmount.ToString("F0"), _labelStyle);
            
            Settings.AimbotBone = (AimbotBone)EditorGUILayout.EnumPopup("Target Bone:", Settings.AimbotBone);
            Settings.AimbotThroughWalls = GUILayout.Toggle(
                Settings.AimbotThroughWalls, "Through Walls", _toggleStyle);
        }
        
        private void DrawMiscMenu()
        {
            GUILayout.Label("--- Misc Settings ---", _labelStyle);
            
            Settings.ShowCrosshair = GUILayout.Toggle(
                Settings.ShowCrosshair, "Show Crosshair", _toggleStyle);
            Settings.CrosshairSize = GUILayout.HorizontalSlider(
                Settings.CrosshairSize, 5f, 50f, _sliderStyle);
            GUILayout.Label("Crosshair Size: " + Settings.CrosshairSize.ToString("F0"), _labelStyle);
            
            Settings.ShowFPS = GUILayout.Toggle(
                Settings.ShowFPS, "Show FPS", _toggleStyle);
            Settings.ShowPlayerCount = GUILayout.Toggle(
                Settings.ShowPlayerCount, "Show Player Count", _toggleStyle);
        }
        
        private void DrawPlayerList()
        {
            GUILayout.Label("--- Player List ---", _labelStyle);
            
            // This would be populated with actual player data
            GUILayout.Label("Players: 0", _labelStyle);
        }
        
        private void DrawPlayerInfo()
        {
            // Draw FPS
            if (Settings.ShowFPS)
            {
                var fpsRect = new Rect(Screen.width - 100, 10, 90, 20);
                GUI.Label(fpsRect, "FPS: " + (1f / Time.unscaledDeltaTime).ToString("F0"), _labelStyle);
            }
            
            // Draw player count
            if (Settings.ShowPlayerCount)
            {
                var playerCountRect = new Rect(Screen.width - 100, 30, 90, 20);
                GUI.Label(playerCountRect, "Players: 0", _labelStyle);
            }
        }
        
        private void DrawCrosshair()
        {
            var center = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var size = Settings.CrosshairSize;
            
            // Horizontal line
            GUI.DrawTexture(new Rect(center.x - size, center.y - 1, size * 2, 2), _windowTexture);
            // Vertical line
            GUI.DrawTexture(new Rect(center.x - 1, center.y - size, 2, size * 2), _windowTexture);
        }
        
        // Settings class
        public static class Settings
        {
            public static bool AimbotEnabled = false;
            public static KeyCode AimKey = KeyCode.Mouse2;
            public static float AimbotFOV = 90f;
            public static bool AimbotSmooth = true;
            public static float AimbotSmoothAmount = 10f;
            public static AimbotBone AimbotBone = AimbotBone.Head;
            public static bool AimbotThroughWalls = false;
            
            public static bool ShowCrosshair = true;
            public static float CrosshairSize = 15f;
            public static bool ShowFPS = true;
            public static bool ShowPlayerCount = true;
        }
        
        public enum AimbotBone
        {
            Head,
            Neck,
            Chest,
            Hips
        }
    }
}
