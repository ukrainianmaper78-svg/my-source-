// C# / MenuGUI.cs / Advanced Menu System / Runtime: Unity 2021+
// CoreDev - Tab-based Menu System with Animation

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cheat.UI
{
    public class MenuGUI : MonoBehaviour
    {
        private enum MenuTab
        {
            ESP,
            Aimbot,
            Misc,
            Players,
            Settings
        }
        
        private MenuTab _currentTab = MenuTab.ESP;
        private bool _menuVisible = true;
        private Rect _menuRect = new Rect(20, 20, 400, 500);
        
        private bool _isDragging = false;
        private Vector2 _dragStartPos;
        private Rect _dragStartRect;
        
        // Animation
        private float _alpha = 0f;
        private float _targetAlpha = 1f;
        private float _animationSpeed = 5f;
        
        // Styles
        private GUIStyle _windowStyle;
        private GUIStyle _tabButtonStyle;
        private GUIStyle _tabButtonActiveStyle;
        private GUIStyle _sectionHeaderStyle;
        private GUIStyle _checkboxStyle;
        
        private void Awake()
        {
            SetupStyles();
        }
        
        private void SetupStyles()
        {
            // Window style
            _windowStyle = new GUIStyle(GUI.skin.window);
            _windowStyle.fontSize = 14;
            _windowStyle.normal.textColor = Color.white;
            _windowStyle.normal.background = MakeTexture(2, 2, new Color(0.05f, 0.05f, 0.05f, 0.95f));
            _windowStyle.border = new RectOffset(15, 15, 25, 15);
            
            // Tab button style (inactive)
            _tabButtonStyle = new GUIStyle(GUI.skin.button);
            _tabButtonStyle.normal.background = MakeTexture(2, 2, new Color(0.15f, 0.15f, 0.15f, 1f));
            _tabButtonStyle.normal.textColor = Color.gray;
            _tabButtonStyle.hover.textColor = Color.white;
            _tabButtonStyle.fontSize = 13;
            _tabButtonStyle.padding = new RectOffset(10, 10, 5, 5);
            _tabButtonStyle.margin = new RectOffset(0, 0, 0, 0);
            _tabButtonStyle.fixedHeight = 30;
            
            // Tab button style (active)
            _tabButtonActiveStyle = new GUIStyle(_tabButtonStyle);
            _tabButtonActiveStyle.normal.background = MakeTexture(2, 2, new Color(0.25f, 0.25f, 0.25f, 1f));
            _tabButtonActiveStyle.normal.textColor = Color.white;
            _tabButtonActiveStyle.fontStyle = FontStyle.Bold;
            
            // Section header style
            _sectionHeaderStyle = new GUIStyle(GUI.skin.label);
            _sectionHeaderStyle.fontSize = 14;
            _sectionHeaderStyle.fontStyle = FontStyle.Bold;
            _sectionHeaderStyle.normal.textColor = Color.cyan;
            
            // Checkbox style
            _checkboxStyle = new GUIStyle(GUI.skin.toggle);
            _checkboxStyle.normal.textColor = Color.white;
            _checkboxStyle.fontSize = 13;
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
        
        private void Update()
        {
            // Toggle menu
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                _menuVisible = !_menuVisible;
                _targetAlpha = _menuVisible ? 1f : 0f;
            }
            
            // Animate alpha
            _alpha = Mathf.Lerp(_alpha, _targetAlpha, Time.unscaledDeltaTime * _animationSpeed);
        }
        
        private void OnGUI()
        {
            if (!_menuVisible || _alpha <= 0.01f) return;
            
            // Apply alpha to styles
            var alphaColor = new Color(1f, 1f, 1f, _alpha);
            
            // Draw main menu
            _menuRect = GUI.Window(0, _menuRect, DrawMenuWindow, "", _windowStyle);
        }
        
        private void DrawMenuWindow(int windowID)
        {
            // Enable dragging
            HandleDragging();
            
            GUILayout.BeginVertical();
            
            // Draw tabs
            DrawTabs();
            
            // Draw content based on current tab
            DrawTabContent();
            
            GUILayout.EndVertical();
            
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }
        
        private void HandleDragging()
        {
            var headerRect = new Rect(_menuRect.x, _menuRect.y, _menuRect.width, 30);
            
            if (Event.current.type == EventType.MouseDown && headerRect.Contains(Event.current.mousePosition))
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
        }
        
        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();
            
            for (int i = 0; i < 5; i++)
            {
                var tab = (MenuTab)i;
                var isActive = _currentTab == tab;
                
                if (GUILayout.Button(tab.ToString(), isActive ? _tabButtonActiveStyle : _tabButtonStyle, GUILayout.Height(30)))
                {
                    _currentTab = tab;
                }
            }
            
            GUILayout.EndHorizontal();
        }
        
        private void DrawTabContent()
        {
            switch (_currentTab)
            {
                case MenuTab.ESP:
                    DrawESPTab();
                    break;
                case MenuTab.Aimbot:
                    DrawAimbotTab();
                    break;
                case MenuTab.Misc:
                    DrawMiscTab();
                    break;
                case MenuTab.Players:
                    DrawPlayersTab();
                    break;
                case MenuTab.Settings:
                    DrawSettingsTab();
                    break;
            }
        }
        
        private void DrawESPTab()
        {
            GUILayout.Space(5);
            
            GUILayout.Label("Visual Settings", _sectionHeaderStyle);
            GUILayout.Space(2);
            
            Cheat.ESP.ESPManager.Settings.Enabled = DrawToggle("Master ESP Toggle", Cheat.ESP.ESPManager.Settings.Enabled);
            Cheat.ESP.ESPManager.Settings.ShowBox = DrawToggle("2D Box", Cheat.ESP.ESPManager.Settings.ShowBox);
            Cheat.ESP.ESPManager.Settings.ShowLine = DrawToggle("Snap Line", Cheat.ESP.ESPManager.Settings.ShowLine);
            Cheat.ESP.ESPManager.Settings.ShowName = DrawToggle("Player Name", Cheat.ESP.ESPManager.Settings.ShowName);
            Cheat.ESP.ESPManager.Settings.ShowHealth = DrawToggle("Health Bar", Cheat.ESP.ESPManager.Settings.ShowHealth);
            Cheat.ESP.ESPManager.Settings.ShowSkeleton = DrawToggle("Bone ESP", Cheat.ESP.ESPManager.Settings.ShowSkeleton);
            Cheat.ESP.ESPManager.Settings.ShowDistance = DrawToggle("Distance", Cheat.ESP.ESPManager.Settings.ShowDistance);
            Cheat.ESP.ESPManager.Settings.ShowWeapon = DrawToggle("Weapon Name", Cheat.ESP.ESPManager.Settings.ShowWeapon);
            Cheat.ESP.ESPManager.Settings.ShowTeam = DrawToggle("Team Color", Cheat.ESP.ESPManager.Settings.ShowTeam);
            
            GUILayout.Space(10);
            
            GUILayout.Label("Filter Settings", _sectionHeaderStyle);
            GUILayout.Space(2);
            
            Cheat.ESP.ESPManager.Settings.MaxDistance = DrawSlider("Max Distance", Cheat.ESP.ESPManager.Settings.MaxDistance, 50f, 1000f);
            Cheat.ESP.ESPManager.OnlyVisible.Value = DrawToggle("Only Visible", Cheat.ESP.ESPManager.OnlyVisible.Value);
            
            GUILayout.Space(10);
            
            GUILayout.Label("Color Settings", _sectionHeaderStyle);
            GUILayout.Space(2);
            
            DrawColorSlider("Box Color R", ref Cheat.ESP.ESPManager.Settings.BoxColor.r);
            DrawColorSlider("Box Color G", ref Cheat.ESP.ESPManager.Settings.BoxColor.g);
            DrawColorSlider("Box Color B", ref Cheat.ESP.ESPManager.Settings.BoxColor.b);
        }
        
        private void DrawAimbotTab()
        {
            GUILayout.Space(5);
            
            GUILayout.Label("Aimbot Settings", _sectionHeaderStyle);
            GUILayout.Space(2);
            
            OverlayGUI.Settings.AimbotEnabled = DrawToggle("Aimbot Enabled", OverlayGUI.Settings.AimbotEnabled);
            OverlayGUI.Settings.AimKey = DrawEnumPopup("Aim Key", OverlayGUI.Settings.AimKey);
            OverlayGUI.Settings.AimbotFOV = DrawSlider("Aim FOV", OverlayGUI.Settings.AimbotFOV, 1f, 360f);
            OverlayGUI.Settings.AimbotSmooth = DrawToggle("Smooth Aim", OverlayGUI.Settings.AimbotSmooth);
            
            if (OverlayGUI.Settings.AimbotSmooth)
            {
                OverlayGUI.Settings.AimbotSmoothAmount = DrawSlider("Smooth Amount", OverlayGUI.Settings.AimbotSmoothAmount, 1f, 50f);
            }
            
            OverlayGUI.Settings.AimbotBone = DrawEnumPopup("Target Bone", OverlayGUI.Settings.AimbotBone);
            OverlayGUI.Settings.AimbotThroughWalls = DrawToggle("Through Walls", OverlayGUI.Settings.AimbotThroughWalls);
        }
        
        private void DrawMiscTab()
        {
            GUILayout.Space(5);
            
            GUILayout.Label("HUD Settings", _sectionHeaderStyle);
            GUILayout.Space(2);
            
            OverlayGUI.Settings.ShowCrosshair = DrawToggle("Show Crosshair", OverlayGUI.Settings.ShowCrosshair);
            OverlayGUI.Settings.CrosshairSize = DrawSlider("Crosshair Size", OverlayGUI.Settings.CrosshairSize, 5f, 50f);
            OverlayGUI.Settings.ShowFPS = DrawToggle("Show FPS", OverlayGUI.Settings.ShowFPS);
            OverlayGUI.Settings.ShowPlayerCount = DrawToggle("Show Player Count", OverlayGUI.Settings.ShowPlayerCount);
        }
        
        private void DrawPlayersTab()
        {
            GUILayout.Space(5);
            GUILayout.Label("Player List", _sectionHeaderStyle);
            GUILayout.Space(2);
            
            // This would display actual player list
            GUILayout.Label("No players detected", _checkboxStyle);
        }
        
        private void DrawSettingsTab()
        {
            GUILayout.Space(5);
            
            GUILayout.Label("Menu Settings", _sectionHeaderStyle);
            GUILayout.Space(2);
            
            if (GUILayout.Button("Reset Menu Position", _checkboxStyle))
            {
                _menuRect = new Rect(20, 20, 400, 500);
            }
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Unload Cheat", _checkboxStyle))
            {
                Cheat.Hooks.UnityHooks.UnhookAll();
                Destroy(gameObject);
            }
        }
        
        // Helper methods for drawing controls
        private bool DrawToggle(string label, bool value)
        {
            GUILayout.BeginHorizontal();
            value = GUILayout.Toggle(value, label, _checkboxStyle);
            GUILayout.EndHorizontal();
            return value;
        }
        
        private float DrawSlider(string label, float value, float min, float max)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, _checkboxStyle, GUILayout.Width(150));
            value = GUILayout.HorizontalSlider(value, min, max);
            GUILayout.Label(value.ToString("F0"), _checkboxStyle, GUILayout.Width(50));
            GUILayout.EndHorizontal();
            return value;
        }
        
        private void DrawColorSlider(string label, ref float value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, _checkboxStyle, GUILayout.Width(120));
            value = GUILayout.HorizontalSlider(value, 0f, 1f);
            GUILayout.Label((value * 255).ToString("F0"), _checkboxStyle, GUILayout.Width(40));
            GUILayout.EndHorizontal();
        }
        
        private T DrawEnumPopup<T>(string label, T selected) where T : Enum
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, _checkboxStyle, GUILayout.Width(150));
            selected = EditorGUILayout.EnumPopup(selected);
            GUILayout.EndHorizontal();
            return selected;
        }
    }
}
