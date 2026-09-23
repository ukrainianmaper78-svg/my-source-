// C++ / NoRecoil.h / No Recoil System / Runtime: Unity 2021+
// CoreDev - Weapon Recoil Cancellation

#pragma once

#include <Windows.h>
#include <string>
#include <unordered_map>

namespace Cheat::Utils
{
    // No Recoil settings
    struct NoRecoilSettings
    {
        bool enabled = false;
        float recoilReduction = 100.0f; // 100% = full recoil removal
        bool affectVertical = true;
        bool affectHorizontal = true;
        bool affectAllWeapons = true;
    };
    
    // Weapon data
    struct WeaponData
    {
        std::string name;
        float verticalRecoil;
        float horizontalRecoil;
        float currentRecoil;
    };
    
    // No Recoil class
    class NoRecoil
    {
    public:
        // Singleton
        static NoRecoil* GetInstance();
        static void Initialize();
        static void Shutdown();
        
        // Settings access
        static NoRecoilSettings& GetSettings();
        
        // Update no recoil
        void Update();
        
        // Hook into weapon fire
        void OnWeaponFire(const std::string& weaponName);
        
        // Hook into weapon update
        void OnWeaponUpdate(const std::string& weaponName, float recoilAmount);
        
        // Compensate recoil
        void CompensateRecoil(float& x, float& y);
        
    private:
        NoRecoil();
        ~NoRecoil();
        
        static NoRecoil* s_instance;
        NoRecoilSettings m_settings;
        std::unordered_map<std::string, WeaponData> m_weaponData;
        
        // Private methods
        float GetWeaponRecoil(const std::string& weaponName);
    };
}
