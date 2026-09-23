// C++ / TriggerBot.h / Trigger Bot System / Runtime: Unity 2021+
// CoreDev - Automatic Trigger Bot with Delay and Randomization

#pragma once

#include <Windows.h>
#include <vector>

// Forward declarations
class Camera;
class Transform;
class MonoBehaviour;

namespace Cheat::Utils
{
    // Trigger Bot settings
    struct TriggerBotSettings
    {
        bool enabled = false;
        int triggerKey = VK_LBUTTON; // Left mouse button
        float activationDelay = 0.05f; // 50ms delay
        float randomization = 0.02f; // 20ms randomization
        bool onlyHeadshots = true;
        bool onlyVisible = true;
        float maxDistance = 500.0f;
    };
    
    // Trigger Bot class
    class TriggerBot
    {
    public:
        // Singleton
        static TriggerBot* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        // Settings access
        static TriggerBotSettings& GetSettings();
        
        // Update trigger bot
        void Update();
        
        // Set camera
        void SetCamera(Camera* camera);
        
        // Check for target at crosshair
        void CheckForTarget();
        
        // Simulate fire
        void SimulateFire();
        
    private:
        TriggerBot();
        ~TriggerBot();
        
        static TriggerBot* s_instance;
        TriggerBotSettings m_settings;
        Camera* m_mainCamera;
        bool m_shouldFire;
        float m_fireTime;
        float m_lastFireTime;
        
        // Private methods
        bool IsValidTarget(void* target);
        bool IsHeadshot(const void* hitInfo);
        bool IsVisible(void* target);
        Transform* FindDeepChild(Transform* parent, const std::string& name);
    };
}
