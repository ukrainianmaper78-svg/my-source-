// C# / WallHack.cs / Wall Hack System / Runtime: Unity 2021+
// CoreDev - See Through Walls with Shader Modification

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cheat.Utils
{
    public class WallHack : MonoBehaviour
    {
        // Settings
        public static class Settings
        {
            public static bool Enabled = false;
            public static float WallAlpha = 0.3f; // 30% opacity
            public static bool AffectPlayers = true;
            public static bool AffectWeapons = true;
            public static bool AffectAllObjects = false;
        }
        
        private List<Renderer> _modifiedRenderers = new List<Renderer>();
        private List<Color> _originalColors = new List<Color>();
        private List<Shader> _originalShaders = new List<Shader>();
        
        private void Update()
        {
            if (Settings.Enabled)
            {
                EnableWallHack();
            }
            else
            {
                DisableWallHack();
            }
        }
        
        private void EnableWallHack()
        {
            // Clear previous modifications
            DisableWallHack();
            
            if (!Settings.Enabled) return;
            
            // Find all renderers
            var allRenderers = FindObjectsOfType<Renderer>();
            
            foreach (var renderer in allRenderers)
            {
                if (ShouldAffectRenderer(renderer))
                {
                    // Store original state
                    _modifiedRenderers.Add(renderer);
                    
                    // Store original color
                    var originalColor = renderer.material.color;
                    _originalColors.Add(originalColor);
                    
                    // Store original shader
                    _originalShaders.Add(renderer.material.shader);
                    
                    // Modify material
                    var newColor = new Color(
                        originalColor.r,
                        originalColor.g,
                        originalColor.b,
                        Settings.WallAlpha);
                    
                    renderer.material.color = newColor;
                    
                    // Use transparent shader if needed
                    if (renderer.material.shader.name != "Transparent/Diffuse" &&
                        renderer.material.shader.name != "Standard" &&
                        !renderer.material.shader.name.Contains("Transparent"))
                    {
                        renderer.material.shader = Shader.Find("Transparent/Diffuse");
                    }
                    
                    // Enable transparency
                    renderer.material.SetFloat("_Mode", 2f); // Fade mode
                    renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    renderer.material.SetInt("_ZWrite", 0);
                    renderer.material.DisableKeyword("_ALPHATEST_ON");
                    renderer.material.EnableKeyword("_ALPHABLEND_ON");
                    renderer.material.renderQueue = 3000; // Transparent queue
                }
            }
        }
        
        private void DisableWallHack()
        {
            for (int i = 0; i < _modifiedRenderers.Count; i++)
            {
                if (_modifiedRenderers[i] != null && i < _originalColors.Count)
                {
                    // Restore original color
                    _modifiedRenderers[i].material.color = _originalColors[i];
                    
                    // Restore original shader
                    if (i < _originalShaders.Count && _originalShaders[i] != null)
                    {
                        _modifiedRenderers[i].material.shader = _originalShaders[i];
                    }
                    
                    // Restore original render settings
                    _modifiedRenderers[i].material.SetFloat("_Mode", 0f);
                    _modifiedRenderers[i].material.renderQueue = -1;
                }
            }
            
            _modifiedRenderers.Clear();
            _originalColors.Clear();
            _originalShaders.Clear();
        }
        
        private bool ShouldAffectRenderer(Renderer renderer)
        {
            if (renderer == null) return false;
            
            // Skip if already transparent
            if (renderer.material.color.a < 0.5f) return false;
            
            // Check if it's a player
            var player = renderer.GetComponentInParent<MonoBehaviour>();
            if (player != null)
            {
                var typeName = player.GetType().Name;
                if (typeName.Contains("Player") || typeName.Contains("Character"))
                {
                    return Settings.AffectPlayers;
                }
            }
            
            // Check if it's a weapon
            var weapon = renderer.GetComponentInParent<MonoBehaviour>();
            if (weapon != null)
            {
                var typeName = weapon.GetType().Name;
                if (typeName.Contains("Weapon") || typeName.Contains("Gun"))
                {
                    return Settings.AffectWeapons;
                }
            }
            
            return Settings.AffectAllObjects;
        }
        
        private void OnDisable()
        {
            DisableWallHack();
        }
        
        private void OnDestroy()
        {
            DisableWallHack();
        }
    }
}
