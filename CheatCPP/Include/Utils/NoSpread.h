// C++ / NoSpread.h / No Spread System / Runtime: Unity 2021+
// CoreDev - Bullet Spread Removal

#pragma once

#include <Windows.h>
#include <string>
#include <unordered_map>

namespace Cheat::Utils
{
    // No Spread settings
    struct NoSpreadSettings
    {
        bool enabled = false;
        float spreadReduction = 100.0f; // 100% = full spread removal
        bool affectAllWeapons = true;
    };
    
    // No Spread class
    class NoSpread
    {
    public:
        // Singleton
        static NoSpread* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        // Settings access
        static NoSpreadSettings& GetSettings();
        
        // Update no spread
        void Update();
        
        // Hook into bullet fire
        void OnBulletFire(D3DXVECTOR3& direction, const std::string& weaponName);
        
        // Hook into weapon update
        void OnWeaponUpdate(float& spread, const std::string& weaponName);
        
        // Get weapon spread
        float GetWeaponSpread(const std::string& weaponName);
        
    private:
        NoSpread();
        ~NoSpread();
        
        static NoSpread* s_instance;
        NoSpreadSettings m_settings;
        std::unordered_map<std::string, float> m_weaponSpreads;
        
        // Private methods
        float GetDefaultWeaponSpread(const std::string& weaponName);
    };
}
