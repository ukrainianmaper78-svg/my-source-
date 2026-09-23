// C++ / NoRecoil.cpp / No Recoil Implementation / Runtime: Unity 2021+
// CoreDev - Weapon Recoil Cancellation Implementation

#include "Utils/NoRecoil.h"
#include <Windows.h>
#include <string>
#include <unordered_map>

namespace Cheat::Utils
{
    NoRecoil* NoRecoil::s_instance = nullptr;
    
    NoRecoil* NoRecoil::GetInstance()
    {
        if (!s_instance)
        {
            s_instance = new NoRecoil();
        }
        return s_instance;
    }
    
    void NoRecoil::Initialize()
    {
        if (s_instance) return;
        s_instance = new NoRecoil();
    }
    
    void NoRecoil::Shutdown()
    {
        if (s_instance)
        {
            delete s_instance;
            s_instance = nullptr;
        }
    }
    
    NoRecoil::NoRecoil()
    {
        // Initialize settings
        m_settings.enabled = false;
        m_settings.recoilReduction = 100.0f;
        m_settings.affectVertical = true;
        m_settings.affectHorizontal = true;
        m_settings.affectAllWeapons = true;
    }
    
    NoRecoil::~NoRecoil()
    {
        m_weaponData.clear();
    }
    
    NoRecoilSettings& NoRecoil::GetSettings()
    {
        return m_settings;
    }
    
    void NoRecoil::Update()
    {
        if (!m_settings.enabled) return;
        
        // Placeholder for update logic
        // In real implementation, we'd update recoil compensation
    }
    
    void NoRecoil::OnWeaponFire(const std::string& weaponName)
    {
        if (!m_settings.enabled) return;
        
        // Get or create weapon data
        if (m_weaponData.find(weaponName) == m_weaponData.end())
        {
            WeaponData weapon;
            weapon.name = weaponName;
            weapon.verticalRecoil = 1.0f;
            weapon.horizontalRecoil = 1.0f;
            m_weaponData[weaponName] = weapon;
        }
    }
    
    void NoRecoil::OnWeaponUpdate(const std::string& weaponName, float recoilAmount)
    {
        auto it = m_weaponData.find(weaponName);
        if (it != m_weaponData.end())
        {
            it->second.currentRecoil = recoilAmount;
        }
    }
    
    void NoRecoil::CompensateRecoil(float& x, float& y)
    {
        if (!m_settings.enabled) return;
        
        if (m_settings.affectVertical)
        {
            x = -x * (m_settings.recoilReduction / 100.0f);
        }
        
        if (m_settings.affectHorizontal)
        {
            y = -y * (m_settings.recoilReduction / 100.0f);
        }
    }
    
    float NoRecoil::GetWeaponRecoil(const std::string& weaponName)
    {
        auto it = m_weaponData.find(weaponName);
        if (it != m_weaponData.end())
        {
            return it->second.currentRecoil;
        }
        return 0.0f;
    }
}
