// C++ / Teleport.h / Teleport System / Runtime: Unity 2021+
// CoreDev - Player Teleportation with Noclip

#pragma once

#include <Windows.h>
#include <d3d9.h>
#include <d3dx9.h>

// Forward declarations
class MonoBehaviour;
class Transform;
class Camera;
class CharacterController;

namespace Cheat::Utils
{
    // Teleport settings
    struct TeleportSettings
    {
        bool enabled = false;
        float teleportDistance = 10.0f;
        bool noclipEnabled = false;
        int teleportForwardKey = 'T';
        int teleportBackwardKey = 'G';
        int teleportUpKey = 'Y';
        int teleportDownKey = 'H';
        int noclipToggleKey = 'N';
    };
    
    // Teleport class
    class Teleport
    {
    public:
        // Singleton
        static Teleport* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        // Settings access
        static TeleportSettings& GetSettings();
        
        // Update teleport
        void Update();
        
        // Set local player
        void SetLocalPlayer(MonoBehaviour* player);
        
        // Set camera
        void SetCamera(Camera* camera);
        
        // Teleport to position
        static void TeleportTo(const D3DXVECTOR3& position);
        
        // Teleport to player
        static void TeleportToPlayer(const std::string& playerName);
        
        // Noclip movement
        void NoclipMovement();
        
    private:
        Teleport();
        ~Teleport();
        
        static Teleport* s_instance;
        TeleportSettings m_settings;
        MonoBehaviour* m_localPlayer;
        Camera* m_mainCamera;
        CharacterController* m_characterController;
        bool m_noclipActive;
        
        // Private methods
        void HandleTeleport();
        MonoBehaviour* FindPlayerByName(const std::string& name);
    };
}
