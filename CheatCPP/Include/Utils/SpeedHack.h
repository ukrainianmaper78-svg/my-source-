// C++ / SpeedHack.h / Speed Hack System / Runtime: Unity 2021+
// CoreDev - Player Movement Speed Modifier

#pragma once

#include <Windows.h>
#include <vector>
#include <string>

// Forward declarations
class MonoBehaviour;
class Transform;
class CharacterController;
class Rigidbody;

namespace Cheat::Utils
{
    // Speed Hack settings
    struct SpeedHackSettings
    {
        bool enabled = false;
        float speedMultiplier = 2.0f; // 2x speed
        bool affectAllPlayers = false;
        bool onlyLocalPlayer = true;
        int toggleKey = 'F'; // F key
    };
    
    // Hooked player data
    struct HookedPlayer
    {
        MonoBehaviour* player;
        float originalSpeed;
    };
    
    // Speed Hack class
    class SpeedHack
    {
    public:
        // Singleton
        static SpeedHack* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        // Settings access
        static SpeedHackSettings& GetSettings();
        
        // Update speed hack
        void Update();
        
        // Apply speed hack to all players
        void ApplySpeedHack();
        
        // Remove speed hack from all players
        void RemoveSpeedHack();
        
        // Check if player is local
        bool IsLocalPlayer(MonoBehaviour* player);
        
        // Get player speed
        float GetPlayerSpeed(MonoBehaviour* player);
        
        // Set player speed
        void SetPlayerSpeed(MonoBehaviour* player, float speed);
        
    private:
        SpeedHack();
        ~SpeedHack();
        
        static SpeedHack* s_instance;
        SpeedHackSettings m_settings;
        std::vector<HookedPlayer> m_hookedPlayers;
        MonoBehaviour* m_localPlayer;
        
        // Private methods
        void HookPlayer(MonoBehaviour* player);
        void UnhookPlayer(MonoBehaviour* player);
    };
}
