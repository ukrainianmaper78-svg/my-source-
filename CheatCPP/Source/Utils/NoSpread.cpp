// C++ / NoSpread.cpp / No Spread Implementation / Runtime: Unity 2021+
// CoreDev - Bullet Spread Removal Implementation

#include "Utils/NoSpread.h"
#include <Windows.h>
#include <string>
#include <unordered_map>
#include <algorithm>

namespace Cheat::Utils
{
    NoSpread* NoSpread::s_instance = nullptr;
    
    NoSpread* NoSpread::GetInstance()
    {
        if (!s_instance)
        {
            s_instance = new NoSpread();
        }
        return s_instance;
    }
    
    void NoSpread::Initialize()
    {
        if (s_instance) return;
        s_instance = new NoSpread();
    }
    
    void NoSpread::Shutdown()
    {
        if (s_instance)
        {
            delete s_instance;
            s_instance = nullptr;
        }
    }
    
    NoSpread::NoSpread()
    {
        // Initialize settings
        m_settings.enabled = false;
        m_settings.spreadReduction = 100.0f;
        m_settings.affectAllWeapons = true;
        
        // Initialize default weapon spreads
        m_weaponSpreads["pistol"] = 2.0f;
        m_weaponSpreads["rifle"] = 0.5f;
        m_weaponSpreads["shotgun"] = 10.0f;
        m_weaponSpreads["sniper"] = 0.1f;
    }
    
    NoSpread::~NoSpread()
    {
        m_weaponSpreads.clear();
    }
    
    NoSpreadSettings& NoSpread::GetSettings()
    {
        return m_settings;
    }
    
    void NoSpread::Update()
    {
        if (!m_settings.enabled) return;
        
        // Placeholder for update logic
    }
    
    void NoSpread::OnBulletFire(D3DXVECTOR3& direction, const std::string& weaponName)
    {
        if (!m_settings.enabled) return;
        
        // Get weapon spread
        float spread = GetWeaponSpread(weaponName);
        
        // Reduce spread
        float reducedSpread = spread * (1.0f - m_settings.spreadReduction / 100.0f);
        
        // Apply reduced spread
        // Generate random direction within reduced spread
        D3DXVECTOR3 randomOffset;
        randomOffset.x = static_cast<float>(rand()) / RAND_MAX * 2.0f - 1.0f;
        randomOffset.y = static_cast<float>(rand()) / RAND_MAX * 2.0f - 1.0f;
        randomOffset.z = static_cast<float>(rand()) / RAND_MAX * 2.0f - 1.0f;
        D3DXVec3Normalize(&randomOffset, &randomOffset);
        randomOffset *= reducedSpread * 0.0174533f; // Convert degrees to radians
        
        direction += randomOffset;
        D3DXVec3Normalize(&direction, &direction);
    }
    
    void NoSpread::OnWeaponUpdate(float& spread, const std::string& weaponName)
    {
        if (!m_settings.enabled) return;
        
        spread *= (1.0f - m_settings.spreadReduction / 100.0f);
    }
    
    float NoSpread::GetWeaponSpread(const std::string& weaponName)
    {
        std::string lowerName = weaponName;
        std::transform(lowerName.begin(), lowerName.end(), lowerName.begin(), ::tolower);
        
        auto it = m_weaponSpreads.find(lowerName);
        if (it != m_weaponSpreads.end())
        {
            return it->second;
        }
        
        return GetDefaultWeaponSpread(lowerName);
    }
    
    float NoSpread::GetDefaultWeaponSpread(const std::string& weaponName)
    {
        // Return default spread for unknown weapons
        return 1.0f;
    }
}
