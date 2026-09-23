// C++ / Cheat.cpp / Main Cheat Implementation / Runtime: Unity 2021+
// CoreDev - Main Cheat System Implementation

#include "Cheat.h"
#include <Windows.h>

namespace Cheat
{
    bool CheatManager::s_initialized = false;
    
    bool CheatManager::Initialize()
    {
        if (s_initialized) return true;
        
        s_initialized = true;
        
        // Initialize all components
        Hooks::UnityHooks::Initialize();
        ESP::ESPManager::Initialize();
        UI::OverlayGUI::Initialize();
        UI::MenuGUI::Initialize();
        Utils::Aimbot::Initialize();
        Utils::TriggerBot::Initialize();
        Utils::NoRecoil::Initialize();
        Utils::NoSpread::Initialize();
        Utils::WallHack::Initialize();
        Utils::SpeedHack::Initialize();
        Utils::Teleport::Initialize();
        
        return true;
    }
    
    void CheatManager::Shutdown()
    {
        if (!s_initialized) return;
        
        // Shutdown all components
        Hooks::UnityHooks::Shutdown();
        ESP::ESPManager::Shutdown();
        UI::OverlayGUI::Shutdown();
        UI::MenuGUI::Shutdown();
        Utils::Aimbot::Shutdown();
        Utils::TriggerBot::Shutdown();
        Utils::NoRecoil::Shutdown();
        Utils::NoSpread::Shutdown();
        Utils::WallHack::Shutdown();
        Utils::SpeedHack::Shutdown();
        Utils::Teleport::Shutdown();
        
        s_initialized = false;
    }
    
    void CheatManager::Update()
    {
        if (!s_initialized) return;
        
        // Update all components
        Utils::Aimbot::GetInstance()->Update();
        Utils::TriggerBot::GetInstance()->Update();
        Utils::NoRecoil::GetInstance()->Update();
        Utils::NoSpread::GetInstance()->Update();
        Utils::WallHack::GetInstance()->Update();
        Utils::SpeedHack::GetInstance()->Update();
        Utils::Teleport::GetInstance()->Update();
    }
    
    void CheatManager::Render()
    {
        if (!s_initialized) return;
        
        // Render all components
        UI::OverlayGUI::GetInstance()->OnGUI();
        UI::MenuGUI::GetInstance()->OnGUI();
    }
    
    Hooks::UnityHooks* CheatManager::GetUnityHooks()
    {
        return Hooks::UnityHooks::GetInstance();
    }
    
    ESP::ESPManager* CheatManager::GetESPManager()
    {
        return ESP::ESPManager::GetInstance();
    }
    
    UI::OverlayGUI* CheatManager::GetOverlayGUI()
    {
        return UI::OverlayGUI::GetInstance();
    }
    
    UI::MenuGUI* CheatManager::GetMenuGUI()
    {
        return UI::MenuGUI::GetInstance();
    }
    
    Utils::Aimbot* CheatManager::GetAimbot()
    {
        return Utils::Aimbot::GetInstance();
    }
    
    Utils::TriggerBot* CheatManager::GetTriggerBot()
    {
        return Utils::TriggerBot::GetInstance();
    }
    
    Utils::NoRecoil* CheatManager::GetNoRecoil()
    {
        return Utils::NoRecoil::GetInstance();
    }
    
    Utils::NoSpread* CheatManager::GetNoSpread()
    {
        return Utils::NoSpread::GetInstance();
    }
    
    Utils::WallHack* CheatManager::GetWallHack()
    {
        return Utils::WallHack::GetInstance();
    }
    
    Utils::SpeedHack* CheatManager::GetSpeedHack()
    {
        return Utils::SpeedHack::GetInstance();
    }
    
    Utils::Teleport* CheatManager::GetTeleport()
    {
        return Utils::Teleport::GetInstance();
    }
}
