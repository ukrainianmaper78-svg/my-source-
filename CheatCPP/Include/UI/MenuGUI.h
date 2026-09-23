// C++ / MenuGUI.h / Advanced Menu System / Runtime: Unity 2021+
// CoreDev - Tab-based Menu System with Animation

#pragma once

#include <Windows.h>

namespace Cheat::UI
{
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
