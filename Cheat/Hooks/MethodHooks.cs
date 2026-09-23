// C# / MethodHooks.cs / Method Detouring System / Runtime: Unity 2021+
// CoreDev - Advanced Method Hooking with Detouring

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Cheat.Hooks
{
    public static class MethodHooks
    {
        private static Dictionary<MethodInfo, Delegate> _originalMethods = new Dictionary<MethodInfo, Delegate>();
        private static Dictionary<MethodInfo, Delegate> _hookedMethods = new Dictionary<MethodInfo, Delegate>();
        
        // Hook a method with a replacement
        public static bool HookMethod(Type targetType, string methodName, Delegate hookDelegate, out Delegate original)
        {
            try
            {
                var method = targetType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (method == null)
                {
                    original = null;
                    return false;
                }
                
                // Store original
                original = Delegate.CreateDelegate(hookDelegate.GetType(), null, method);
                _originalMethods[method] = original;
                
                // Create dynamic method for hook
                var dynamicMethod = new DynamicMethod(
                    method.Name + "_Hook",
                    method.ReturnType,
                    GetParameterTypes(method),
                    method.DeclaringType.Module);
                
                var il = dynamicMethod.GetILGenerator();
                
                // Call hook delegate
                il.Emit(OpCodes.Ldarg_0);
                for (int i = 1; i < method.GetParameters().Length + 1; i++)
                {
                    il.Emit(OpCodes.Ldarg, i);
                }
                il.Emit(OpCodes.Tailcall);
                il.Emit(OpCodes.Call, hookDelegate.Method);
                il.Emit(OpCodes.Ret);
                
                // Replace method (simplified - in real Unity we'd need Mono modding)
                _hookedMethods[method] = dynamicMethod.CreateDelegate(hookDelegate.GetType());
                
                Debug.Log("[CoreDev] Method Hooked: " + targetType.Name + "." + methodName);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("[CoreDev] Method Hook Failed: " + ex.Message);
                original = null;
                return false;
            }
        }
        
        // Unhook a method
        public static bool UnhookMethod(MethodInfo method)
        {
            if (_hookedMethods.ContainsKey(method))
            {
                _hookedMethods.Remove(method);
                Debug.Log("[CoreDev] Method Unhooked: " + method.Name);
                return true;
            }
            return false;
        }
        
        // Get original method delegate
        public static Delegate GetOriginal(MethodInfo method)
        {
            if (_originalMethods.ContainsKey(method))
                return _originalMethods[method];
            return null;
        }
        
        // Helper to get parameter types
        private static Type[] GetParameterTypes(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var types = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                types[i] = parameters[i].ParameterType;
            }
            return types;
        }
        
        // Hook FPSPlayer methods
        public static void HookFPSPlayer()
        {
            var fpsPlayerType = Type.GetType("FPSPlayer");
            if (fpsPlayerType == null)
            {
                Debug.LogWarning("[CoreDev] FPSPlayer type not found");
                return;
            }
            
            // Hook Update method
            Delegate originalUpdate;
            HookMethod(fpsPlayerType, "Update", new Action(HookedFPSPlayerUpdate), out originalUpdate);
            
            // Hook OnGUI method  
            Delegate originalOnGUI;
            HookMethod(fpsPlayerType, "OnGUI", new Action(HookedFPSPlayerOnGUI), out originalOnGUI);
        }
        
        private static void HookedFPSPlayerUpdate()
        {
            // Call original
            var original = GetOriginal(typeof(MonoBehaviour).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance)) as Action;
            original?.Invoke();
            
            // Our logic
            UnityHooks.ESPManager?.Update();
        }
        
        private static void HookedFPSPlayerOnGUI()
        {
            // Call original
            var original = GetOriginal(typeof(MonoBehaviour).GetMethod("OnGUI", BindingFlags.NonPublic | BindingFlags.Instance)) as Action;
            original?.Invoke();
            
            // Our GUI
            UnityHooks.OverlayGUI?.OnGUI();
        }
        
        // Hook MainMenu for menu manipulation
        public static void HookMainMenu()
        {
            var mainMenuType = Type.GetType("MainMenu");
            if (mainMenuType == null)
            {
                Debug.LogWarning("[CoreDev] MainMenu type not found");
                return;
            }
            
            Delegate originalOnGUI;
            HookMethod(mainMenuType, "OnGUI", new Action(HookedMainMenuOnGUI), out originalOnGUI);
        }
        
        private static void HookedMainMenuOnGUI()
        {
            // Draw our overlay on top
            UnityHooks.OverlayGUI?.OnGUI();
        }
    }
}
