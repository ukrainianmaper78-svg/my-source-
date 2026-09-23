// C++ / Aimbot.h / Aimbot System / Runtime: Unity 2021+
// CoreDev - Advanced Aimbot with Smooth Aim, Bone Targeting, FOV Check

#pragma once

#include <Windows.h>
#include <vector>
#include <d3d9.h>
#include <d3dx9.h>

// Forward declarations
class Camera;
class Transform;
class MonoBehaviour;

namespace Cheat::Utils
{
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
        bool predictMovement = false;
        bool autoFire = false;
    };
    
    enum class TargetBone
    {
        Head,
        Neck,
        Chest,
        Hips,
        Random
    };
    
    // Target data
    struct TargetInfo
    {
        Transform* transform;
        D3DXVECTOR3 bonePosition;
        float distance;
        float angleToTarget;
        bool isVisible;
    };
    
    // Aimbot class
    class Aimbot
    {
    public:
        // Singleton
        static Aimbot* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        // Settings access
        static AimbotSettings& GetSettings();
        
        // Update aimbot
        void Update();
        
        // Set camera
        void SetCamera(Camera* camera);
        
        // Add target
        void AddTarget(Transform* transform, bool isVisible);
        
        // Remove target
        void RemoveTarget(Transform* transform);
        
        // Clear all targets
        void ClearTargets();
        
        // Get current target
        TargetInfo* GetCurrentTarget();
        
        // Aim at target
        void AimAtTarget();
        
        // Check if target is visible
        bool IsTargetVisible(Transform* transform);
        
        // Get bone position
        D3DXVECTOR3 GetBonePosition(Transform* transform, TargetBone bone);
        
    private:
        Aimbot();
        ~Aimbot();
        
        static Aimbot* s_instance;
        AimbotSettings m_settings;
        Camera* m_mainCamera;
        std::vector<TargetInfo> m_targets;
        TargetInfo* m_currentTarget;
        float m_nextScanTime;
        float m_scanInterval;
        
        // Bone names
        static const std::string s_boneNames[];
        
        // Private methods
        void ScanForTargets();
        float GetAngleToTarget(const D3DXVECTOR3& targetPosition);
        TargetInfo* GetBestTarget();
        D3DXVECTOR3 GetHeadPosition(Transform* transform);
        Transform* FindDeepChild(Transform* parent, const std::string& name);
        void SimulateMouseClick();
    };
}
