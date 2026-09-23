// C++ / SpeedHack.cpp / Speed Hack Implementation / Runtime: Unity 2021+
// CoreDev - Player Movement Speed Modifier Implementation

#include "Utils/SpeedHack.h"
#include <Windows.h>
#include <vector>
#include <string>

namespace Cheat::Utils
{
    SpeedHack* SpeedHack::s_instance = nullptr;
    
    SpeedHack* SpeedHack::GetInstance()
    {
        if (!s_instance)
        {
            s_instance = new SpeedHack();
        }
        return s_instance;
    }
    
    void SpeedHack::Initialize()
    {
        if (s_instance) return;
        s_instance = new SpeedHack();
    }
    
    void SpeedHack::Shutdown()
    {
        if (s_instance)
        {
            delete s_instance;
            s_instance = nullptr;
        }
    }
    
    SpeedHack::SpeedHack() : m_localPlayer(nullptr)
    {
        // Initialize settings
        m_settings.enabled = false;
        m_settings.speedMultiplier = 2.0f;
        m_settings.affectAllPlayers = false;
        m_settings.onlyLocalPlayer = true;
        m_settings.toggleKey = 'F';
    }
    
    SpeedHack::~SpeedHack()
    {
        RemoveSpeedHack();
    }
    
    SpeedHackSettings& SpeedHack::GetSettings()
    {
        return m_settings;
    }
    
    void SpeedHack::Update()
    {
        // Toggle with key
        if (GetAsyncKeyState(m_settings.toggleKey) & 1)
        {
            m_settings.enabled = !m_settings.enabled;
        }
        
        if (m_settings.enabled)
        {
            ApplySpeedHack();
        }
        else
        {
            RemoveSpeedHack();
        }
    }
    
    void SpeedHack::ApplySpeedHack()
    {
        // Clear previous hooks
        RemoveSpeedHack();
        
        if (!m_settings.enabled) return;
        
        // Placeholder for finding all players
        // In real implementation, we'd iterate through all players
        
        // For now, this is a placeholder
    }
    
    void SpeedHack::RemoveSpeedHack()
    {
        for (auto& hooked : m_hookedPlayers)
        {
            if (hooked.player)
            {
                SetPlayerSpeed(hooked.player, hooked.originalSpeed);
            }
        }
        m_hookedPlayers.clear();
    }
    
    bool SpeedHack::IsLocalPlayer(MonoBehaviour* player)
    {
        if (!player) return false;
        
        // Placeholder - in real implementation, we'd check if this is local player
        // For FPS games, this would be the player with Camera component
        return true;
    }
    
    float SpeedHack::GetPlayerSpeed(MonoBehaviour* player)
    {
        if (!player) return 0.0f;
        
        // Placeholder - in real implementation, we'd get speed from player component
        return 5.0f; // Default speed
    }
    
    void SpeedHack::SetPlayerSpeed(MonoBehaviour* player, float speed)
    {
        if (!player) return;
        
        // Placeholder - in real implementation, we'd set speed on player component
        // This would modify the moveSpeed or speed field
    }
    
    void SpeedHack::HookPlayer(MonoBehaviour* player)
    {
        if (!player) return;
        
        float originalSpeed = GetPlayerSpeed(player);
        float newSpeed = originalSpeed * m_settings.speedMultiplier;
        
        HookedPlayer hooked;
        hooked.player = player;
        hooked.originalSpeed = originalSpeed;
        
        SetPlayerSpeed(player, newSpeed);
        m_hookedPlayers.push_back(hooked);
    }
    
    void SpeedHack::UnhookPlayer(MonoBehaviour* player)
    {
        for (auto it = m_hookedPlayers.begin(); it != m_hookedPlayers.end(); ++it)
        {
            if (it->player == player)
            {
                SetPlayerSpeed(player, it->originalSpeed);
                m_hookedPlayers.erase(it);
                break;
            }
        }
    }
}
