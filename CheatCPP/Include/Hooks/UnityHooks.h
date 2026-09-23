// C++ / UnityHooks.h / Unity Hook System / Runtime: Unity 2021+
// CoreDev - Unity Hook System with ESP Integration
// Target: Unity MonoBehaviour, Camera hooks

#pragma once

#include <Windows.h>
#include <vector>
#include <string>
#include <memory>
#include <functional>

// Forward declarations
class MonoBehaviour;
class Camera;
class GameObject;

namespace Cheat::Hooks
{
    // Type aliases for Unity classes
    using UnityObject = void*;
    using MonoBehaviourPtr = MonoBehaviour*;
    using CameraPtr = Camera*;
    using GameObjectPtr = GameObject*;
    
    // Function type aliases
    using OnGUIFunc = std::function<void()>;
    using UpdateFunc = std::function<void()>;
    using CameraRenderFunc = std::function<void(Camera*)>;
    
    // Main hook manager class
    class UnityHooks
    {
    public:
        // Initialize the hook system
        static bool Initialize();
        
        // Shutdown and remove all hooks
        static void Shutdown();
        
        // Hook a MonoBehaviour's OnGUI method
        static bool HookOnGUI(MonoBehaviourPtr behaviour, OnGUIFunc hook);
        
        // Hook a MonoBehaviour's Update method
        static bool HookUpdate(MonoBehaviourPtr behaviour, UpdateFunc hook);
        
        // Hook Camera OnPreRender
        static bool HookCameraPreRender(CameraPtr camera, CameraRenderFunc hook);
        
        // Hook Camera OnPostRender
        static bool HookCameraPostRender(CameraPtr camera, CameraRenderFunc hook);
        
        // Hook all MonoBehaviours in scene
        static void HookAllBehaviours();
        
        // Hook main camera
        static bool HookMainCamera();
        
        // Get original OnGUI for a behaviour
        static OnGUIFunc GetOriginalOnGUI(MonoBehaviourPtr behaviour);
        
        // Get original Update for a behaviour
        static UpdateFunc GetOriginalUpdate(MonoBehaviourPtr behaviour);
        
        // Check if hooks are installed
        static bool AreHooksInstalled();
        
        // Call original OnGUI
        static void CallOriginalOnGUI(MonoBehaviourPtr behaviour);
        
        // Call original Update
        static void CallOriginalUpdate(MonoBehaviourPtr behaviour);
        
    private:
        // Internal hook data
        struct HookData
        {
            MonoBehaviourPtr behaviour;
            OnGUIFunc originalOnGUI;
            UpdateFunc originalUpdate;
            OnGUIFunc hookedOnGUI;
            UpdateFunc hookedUpdate;
        };
        
        struct CameraHookData
        {
            CameraPtr camera;
            CameraRenderFunc originalPreRender;
            CameraRenderFunc originalPostRender;
            CameraRenderFunc hookedPreRender;
            CameraRenderFunc hookedPostRender;
        };
        
        static std::vector<HookData> s_behaviourHooks;
        static std::vector<CameraHookData> s_cameraHooks;
        static bool s_initialized;
        
        // Internal hook installation
        static bool InstallBehaviourHooks(MonoBehaviourPtr behaviour);
        static bool InstallCameraHooks(CameraPtr camera);
        
        // Method detouring
        static void* DetourMethod(void* original, void* hook, size_t size);
        static void RestoreMethod(void* original, void* backup, size_t size);
    };
    
    // Camera hook component
    class CameraHook
    {
    public:
        CameraHook(CameraPtr camera);
        ~CameraHook();
        
        void Initialize();
        void OnPreRender();
        void OnPostRender();
        
    private:
        CameraPtr m_camera;
        bool m_initialized;
    };
    
    // Behaviour hook component
    class BehaviourHook
    {
    public:
        BehaviourHook(MonoBehaviourPtr behaviour);
        ~BehaviourHook();
        
        void Initialize();
        void OnGUI();
        void Update();
        
    private:
        MonoBehaviourPtr m_behaviour;
        bool m_initialized;
    };
}
