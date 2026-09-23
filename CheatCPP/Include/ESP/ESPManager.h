// C++ / ESPManager.h / ESP System Manager / Runtime: Unity 2021+
// CoreDev - Complete ESP System with Box, Line, Name, Health, Skeleton

#pragma once

#include <Windows.h>
#include <vector>
#include <string>
#include <memory>
#include <d3d9.h>
#include <d3dx9.h>

// Forward declarations
class Camera;
class Transform;
class MonoBehaviour;

namespace Cheat::ESP
{
    // ESP Settings structure
    struct ESPSettings
    {
        bool enabled = true;
        bool showBox = true;
        bool showLine = true;
        bool showName = true;
        bool showHealth = true;
        bool showSkeleton = true;
        bool showDistance = true;
        bool showWeapon = true;
        bool showTeam = true;
        bool onlyVisible = false;
        
        float maxDistance = 500.0f;
        float boxThickness = 2.0f;
        float lineThickness = 2.0f;
        int fontSize = 14;
        
        D3DCOLOR boxColor = D3DCOLOR_ARGB(255, 255, 255, 255);
        D3DCOLOR enemyColor = D3DCOLOR_ARGB(255, 255, 0, 0);
        D3DCOLOR teamColor = D3DCOLOR_ARGB(255, 0, 255, 0);
        D3DCOLOR lineColor = D3DCOLOR_ARGB(255, 255, 255, 255);
        D3DCOLOR textColor = D3DCOLOR_ARGB(255, 255, 255, 255);
    };
    
    // Player data for ESP
    struct PlayerESPData
    {
        Transform* transform;
        std::string name;
        float health;
        float maxHealth;
        bool isTeam;
        std::string weaponName;
        D3DXVECTOR3* bonePositions;
        int boneCount;
        float distance;
        RECT screenRect;
        bool isVisible;
    };
    
    // Main ESP Manager class
    class ESPManager
    {
    public:
        // Singleton
        static ESPManager* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        // Update ESP data
        void Update();
        
        // Render callbacks
        void OnPreRender(Camera* camera);
        void OnPostRender(Camera* camera);
        
        // Draw ESP for all players
        void DrawESP();
        
        // Settings access
        static ESPSettings& GetSettings();
        
        // Add player to ESP list
        void AddPlayer(Transform* transform, const std::string& name, float health, float maxHealth, bool isTeam, const std::string& weaponName);
        
        // Remove player from ESP list
        void RemovePlayer(Transform* transform);
        
        // Clear all players
        void ClearPlayers();
        
        // Check if player is visible
        bool IsPlayerVisible(Transform* transform);
        
        // Get bone positions
        void GetBonePositions(Transform* transform, D3DXVECTOR3* bones, int maxBones);
        
    private:
        ESPManager();
        ~ESPManager();
        
        static ESPManager* s_instance;
        ESPSettings m_settings;
        std::vector<PlayerESPData> m_playerData;
        Camera* m_mainCamera;
        ID3DXFont* m_font;
        IDirect3DDevice9* m_device;
        
        // Private methods
        void CachePlayerData();
        void DrawBox(const RECT& rect, D3DCOLOR color, float thickness);
        void DrawLine(const D3DXVECTOR2& start, const D3DXVECTOR2& end, D3DCOLOR color, float thickness);
        void DrawText(const RECT& rect, const std::string& text, D3DCOLOR color);
        void DrawHealthBar(const RECT& rect, float health, float maxHealth, D3DCOLOR color);
        void DrawSkeleton(D3DXVECTOR3* bonePositions, int boneCount, D3DCOLOR color);
        
        D3DXVECTOR3 GetHeadPosition(Transform* transform);
        D3DXVECTOR3 GetFootPosition(Transform* transform);
        Transform* FindChild(Transform* parent, const std::string& name);
        
        // Bone names for skeleton ESP
        static const std::string s_boneNames[];
    };
}
