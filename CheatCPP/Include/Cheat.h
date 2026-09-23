// C++ / Cheat.h / Main Cheat Header / Runtime: Unity 2021+
// CoreDev - Main Cheat System Header

#pragma once

// Include all cheat components
#include "Hooks/UnityHooks.h"
#include "ESP/ESPManager.h"
#include "UI/OverlayGUI.h"
#include "Utils/Aimbot.h"
#include "Utils/TriggerBot.h"
#include "Utils/NoRecoil.h"
#include "Utils/NoSpread.h"
#include "Utils/WallHack.h"
#include "Utils/SpeedHack.h"
#include "Utils/Teleport.h"

namespace Cheat
{
    // Main cheat initialization
    class CheatManager
    {
    public:
        // Initialize all cheat systems
        static bool Initialize();
        
        // Shutdown all cheat systems
        static void Shutdown();
        
        // Update all cheat systems
        static void Update();
        
        // Render all cheat overlays
        static void Render();
        
        // Get component instances
        static Hooks::UnityHooks* GetUnityHooks();
        static ESP::ESPManager* GetESPManager();
        static UI::OverlayGUI* GetOverlayGUI();
        static UI::MenuGUI* GetMenuGUI();
        static Utils::Aimbot* GetAimbot();
        static Utils::TriggerBot* GetTriggerBot();
        static Utils::NoRecoil* GetNoRecoil();
        static Utils::NoSpread* GetNoSpread();
        static Utils::WallHack* GetWallHack();
        static Utils::SpeedHack* GetSpeedHack();
        static Utils::Teleport* GetTeleport();
        
    private:
        CheatManager();
        ~CheatManager();
        
        static bool s_initialized;
    };
}

// Convenience macros
#define CHEAT_INIT() Cheat::CheatManager::Initialize()
#define CHEAT_SHUTDOWN() Cheat::CheatManager::Shutdown()
#define CHEAT_UPDATE() Cheat::CheatManager::Update()
#define CHEAT_RENDER() Cheat::CheatManager::Render()
