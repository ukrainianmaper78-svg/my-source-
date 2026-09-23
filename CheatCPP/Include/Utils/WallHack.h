// C++ / WallHack.h / Wall Hack System / Runtime: Unity 2021+
// CoreDev - See Through Walls with Shader Modification

#pragma once

#include <Windows.h>
#include <vector>
#include <d3d9.h>

// Forward declarations
class Renderer;
class MonoBehaviour;

namespace Cheat::Utils
{
    // Wall Hack settings
    struct WallHackSettings
    {
        bool enabled = false;
        float wallAlpha = 0.3f; // 30% opacity
        bool affectPlayers = true;
        bool affectWeapons = true;
        bool affectAllObjects = false;
    };
    
    // Modified renderer data
    struct ModifiedRenderer
    {
        Renderer* renderer;
        D3DCOLOR originalColor;
        IDirect3DBaseTexture9* originalTexture;
    };
    
    // Wall Hack class
    class WallHack
    {
    public:
        // Singleton
        static WallHack* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        // Settings access
        static WallHackSettings& GetSettings();
        
        // Update wall hack
        void Update();
        
        // Enable wall hack
        void EnableWallHack();
        
        // Disable wall hack
        void DisableWallHack();
        
        // Check if should affect renderer
        bool ShouldAffectRenderer(Renderer* renderer);
        
    private:
        WallHack();
        ~WallHack();
        
        static WallHack* s_instance;
        WallHackSettings m_settings;
        std::vector<ModifiedRenderer> m_modifiedRenderers;
        IDirect3DDevice9* m_device;
        
        // Private methods
        void ModifyRenderer(Renderer* renderer);
        void RestoreRenderer(ModifiedRenderer& modified);
    };
}
