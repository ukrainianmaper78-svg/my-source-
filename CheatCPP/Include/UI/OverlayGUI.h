// C++ / OverlayGUI.h / IMGUI Overlay System / Runtime: Unity 2021+
// CoreDev - Complete IMGUI Overlay with Menu, ESP Toggle, Aimbot, etc.

#pragma once

#include <Windows.h>
#include <string>
#include <vector>
#include <functional>
#include <d3d9.h>
#include <d3dx9.h>

namespace Cheat::UI
{
    // GUI Settings
    struct GUISettings
    {
        bool menuVisible = true;
        bool espMenuVisible = false;
        bool aimbotMenuVisible = false;
        bool miscMenuVisible = false;
        bool playerListVisible = false;
        
        // Position and size
        int menuX = 10;
        int menuY = 10;
        int menuWidth = 300;
        int menuHeight = 400;
        
        // Styles
        bool useCustomStyles = true;
    };
    
    // Aimbot settings
    struct AimbotSettings
    {
        bool enabled = false;
        int aimKey = VK_RBUTTON; // Right mouse button
        float fov = 90.0f;
        bool smoothAim = true;
        float smoothAmount = 10.0f;
        int targetBone = 0; // Head
        bool throughWalls = false;
    };
    
    // Crosshair settings
    struct CrosshairSettings
    {
        bool showCrosshair = true;
        float crosshairSize = 15.0f;
        D3DCOLOR color = D3DCOLOR_ARGB(255, 255, 255, 255);
    };
    
    // Main Overlay GUI class
    class OverlayGUI
    {
    public:
        // Singleton
        static OverlayGUI* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        // Main render function
        void OnGUI();
        
        // Settings access
        static GUISettings& GetGUISettings();
        static AimbotSettings& GetAimbotSettings();
        static CrosshairSettings& GetCrosshairSettings();
        
        // Toggle menu
        void ToggleMenu();
        
        // Draw functions
        void DrawMainMenu();
        void DrawESPMenu();
        void DrawAimbotMenu();
        void DrawMiscMenu();
        void DrawPlayerList();
        void DrawCrosshair();
        void DrawPlayerInfo();
        
    private:
        OverlayGUI();
        ~OverlayGUI();
        
        static OverlayGUI* s_instance;
        GUISettings m_guiSettings;
        AimbotSettings m_aimbotSettings;
        CrosshairSettings m_crosshairSettings;
        
        IDirect3DDevice9* m_device;
        ID3DXFont* m_font;
        ID3DXSprite* m_sprite;
        
        bool m_isDragging;
        POINT m_dragStartPos;
        RECT m_dragStartRect;
        
        // Private methods
        void SetupStyles();
        void HandleInput();
        void DrawWindow(int id, const RECT& rect, const std::string& title);
        void DrawButton(const RECT& rect, const std::string& text, bool active);
        void DrawToggle(const RECT& rect, const std::string& text, bool& value);
        void DrawSlider(const RECT& rect, const std::string& text, float& value, float min, float max);
        void DrawLabel(const RECT& rect, const std::string& text, D3DCOLOR color);
        
        // D3D helpers
        void BeginDraw();
        void EndDraw();
    };
    
    // Menu GUI class (tab-based)
    class MenuGUI
    {
    public:
        enum class MenuTab
        {
            ESP,
            Aimbot,
            Misc,
            Players,
            Settings
        };
        
        static MenuGUI* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        void OnGUI();
        
    private:
        MenuGUI();
        ~MenuGUI();
        
        static MenuGUI* s_instance;
        MenuTab m_currentTab;
        bool m_menuVisible;
        RECT m_menuRect;
        
        bool m_isDragging;
        POINT m_dragStartPos;
        RECT m_dragStartRect;
        
        // Animation
        float m_alpha;
        float m_targetAlpha;
        float m_animationSpeed;
        
        void HandleDragging();
        void DrawTabs();
        void DrawTabContent();
        void DrawESPTab();
        void DrawAimbotTab();
        void DrawMiscTab();
        void DrawPlayersTab();
        void DrawSettingsTab();
    };
}
