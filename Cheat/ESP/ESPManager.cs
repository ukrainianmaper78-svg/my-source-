// C# / ESPManager.cs / ESP System Manager / Runtime: Unity 2021+
// CoreDev - Complete ESP System with Box, Line, Name, Health, Skeleton

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cheat.ESP
{
    public class ESPManager : MonoBehaviour
    {
        // ESP Settings
        public static class Settings
        {
            public static bool Enabled = true;
            public static bool ShowBox = true;
            public static bool ShowLine = true;
            public static bool ShowName = true;
            public static bool ShowHealth = true;
            public static bool ShowSkeleton = true;
            public static bool ShowDistance = true;
            public static bool ShowWeapon = true;
            public static bool ShowTeam = true;
            
            public static Color BoxColor = Color.white;
            public static Color EnemyColor = Color.red;
            public static Color TeamColor = Color.green;
            public static Color LineColor = Color.white;
            public static Color TextColor = Color.white;
            
            public static float MaxDistance = 500f;
            public static float BoxThickness = 2f;
            public static float LineThickness = 2f;
            public static int FontSize = 14;
        }
        
        // Player data cache
        private class PlayerESPData
        {
            public Transform Transform;
            public string Name;
            public float Health;
            public float MaxHealth;
            public bool IsTeam;
            public string WeaponName;
            public Vector3[] BonePositions;
            public float Distance;
            public Rect ScreenRect;
            public bool IsVisible;
        }
        
        private List<PlayerESPData> _playerData = new List<PlayerESPData>();
        private Camera _mainCamera;
        private GUIStyle _espStyle;
        private Texture2D _whiteTexture;
        
        // Bone names for skeleton ESP
        private static readonly string[] _boneNames = new string[]
        {
            "Head", "Neck", "Spine", "Hips",
            "LeftShoulder", "LeftArm", "LeftForearm", "LeftHand",
            "RightShoulder", "RightArm", "RightForearm", "RightHand",
            "LeftThigh", "LeftCalf", "LeftFoot",
            "RightThigh", "RightCalf", "RightFoot"
        };
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            
            // Create white texture for drawing
            _whiteTexture = new Texture2D(1, 1);
            _whiteTexture.SetPixel(0, 0, Color.white);
            _whiteTexture.Apply();
            
            // Setup ESP style
            _espStyle = new GUIStyle();
            _espStyle.fontSize = Settings.FontSize;
            _espStyle.normal.textColor = Settings.TextColor;
            _espStyle.alignment = TextAnchor.MiddleCenter;
            
            Debug.Log("[CoreDev] ESP Manager Awake");
        }
        
        private void Update()
        {
            if (!Settings.Enabled) return;
            
            // Clear old data
            _playerData.Clear();
            
            // Find all players and cache their data
            CachePlayerData();
        }
        
        private void CachePlayerData()
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
                if (_mainCamera == null) return;
            }
            
            // Find all FPSPlayer instances
            var fpsPlayers = FindObjectsOfType<MonoBehaviour>();
            foreach (var mb in fpsPlayers)
            {
                if (mb.GetType().Name == "FPSPlayer" || mb.GetType().Name.Contains("Player"))
                {
                    var playerData = new PlayerESPData
                    {
                        Transform = mb.transform,
                        Name = mb.gameObject.name,
                        Health = 100f, // Default, will try to get real value
                        MaxHealth = 100f,
                        IsTeam = false,
                        WeaponName = "Unknown"
                    };
                    
                    // Try to get health component
                    var healthComponent = mb.GetComponentInChildren<MonoBehaviour>();
                    
                    // Try to get player info from PlayerData class
                    var playerDataType = Type.GetType("PlayerData");
                    if (playerDataType != null)
                    {
                        var playerDataObj = mb.GetComponent(playerDataType);
                        if (playerDataObj != null)
                        {
                            var healthField = playerDataType.GetField("health");
                            if (healthField != null)
                            {
                                playerData.Health = (float)healthField.GetValue(playerDataObj);
                            }
                        }
                    }
                    
                    // Calculate distance
                    if (mb.transform != null)
                    {
                        playerData.Distance = Vector3.Distance(_mainCamera.transform.position, mb.transform.position);
                        
                        // Check visibility
                        playerData.IsVisible = IsVisible(mb.transform);
                        
                        // Get screen position
                        var headPos = GetHeadPosition(mb.transform);
                        var footPos = GetFootPosition(mb.transform);
                        
                        var headScreen = _mainCamera.WorldToScreenPoint(headPos);
                        var footScreen = _mainCamera.WorldToScreenPoint(footPos);
                        
                        if (headScreen.z > 0 && footScreen.z > 0)
                        {
                            playerData.ScreenRect = new Rect(
                                footScreen.x, Screen.height - headScreen.y,
                                footScreen.x - headScreen.x,
                                headScreen.y - footScreen.y);
                        }
                        
                        // Get bone positions for skeleton
                        playerData.BonePositions = GetBonePositions(mb.transform);
                    }
                    
                    _playerData.Add(playerData);
                }
            }
        }
        
        public void OnPreRender(Camera camera)
        {
            // Pre-render logic if needed
        }
        
        public void OnPostRender(Camera camera)
        {
            if (!Settings.Enabled) return;
            
            // Draw ESP in screen space
            DrawESP();
        }
        
        private void DrawESP()
        {
            if (_mainCamera == null || _playerData.Count == 0) return;
            
            foreach (var player in _playerData)
            {
                if (player.Distance > Settings.MaxDistance) continue;
                if (!player.IsVisible && Settings.OnlyVisible) continue;
                
                var color = player.IsTeam ? Settings.TeamColor : Settings.EnemyColor;
                
                // Draw box
                if (Settings.ShowBox)
                {
                    DrawBox(player.ScreenRect, color, Settings.BoxThickness);
                }
                
                // Draw line to player
                if (Settings.ShowLine)
                {
                    DrawLineToPlayer(player, Settings.LineColor, Settings.LineThickness);
                }
                
                // Draw name
                if (Settings.ShowName)
                {
                    DrawText(player.ScreenRect, player.Name, color);
                }
                
                // Draw health
                if (Settings.ShowHealth)
                {
                    DrawHealthBar(player.ScreenRect, player.Health, player.MaxHealth, color);
                }
                
                // Draw distance
                if (Settings.ShowDistance)
                {
                    DrawText(new Rect(player.ScreenRect.x, player.ScreenRect.y - 20, 100, 20),
                            $"{player.Distance:F1}m", Settings.TextColor);
                }
                
                // Draw weapon
                if (Settings.ShowWeapon)
                {
                    DrawText(new Rect(player.ScreenRect.x, player.ScreenRect.y + player.ScreenRect.height + 5, 100, 20),
                            player.WeaponName, Settings.TextColor);
                }
                
                // Draw skeleton
                if (Settings.ShowSkeleton && player.BonePositions != null)
                {
                    DrawSkeleton(player.BonePositions, color);
                }
            }
        }
        
        private void DrawBox(Rect rect, Color color, float thickness)
        {
            if (rect.width <= 0 || rect.height <= 0) return;
            
            // Draw box outline
            DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y), color, thickness);
            DrawLine(new Vector2(rect.x + rect.width, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, thickness);
            DrawLine(new Vector2(rect.x + rect.width, rect.y + rect.height), new Vector2(rect.x, rect.y + rect.height), color, thickness);
            DrawLine(new Vector2(rect.x, rect.y + rect.height), new Vector2(rect.x, rect.y), color, thickness);
        }
        
        private void DrawLineToPlayer(PlayerESPData player, Color color, float thickness)
        {
            if (player.ScreenRect.width <= 0 || player.ScreenRect.height <= 0) return;
            
            var center = new Vector2(player.ScreenRect.center.x, player.ScreenRect.center.y);
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            
            DrawLine(screenCenter, center, color, thickness);
        }
        
        private void DrawLine(Vector2 start, Vector2 end, Color color, float thickness)
        {
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(start);
            GL.Vertex(end);
            GL.End();
        }
        
        private void DrawText(Rect rect, string text, Color color)
        {
            var oldColor = GUI.color;
            var oldContentColor = GUI.contentColor;
            var oldBackgroundColor = GUI.backgroundColor;
            
            GUI.color = color;
            GUI.contentColor = color;
            GUI.backgroundColor = Color.clear;
            
            var style = new GUIStyle(_espStyle);
            style.normal.textColor = color;
            
            GUI.Label(rect, text, style);
            
            GUI.color = oldColor;
            GUI.contentColor = oldContentColor;
            GUI.backgroundColor = oldBackgroundColor;
        }
        
        private void DrawHealthBar(Rect rect, float health, float maxHealth, Color color)
        {
            var healthPercent = Mathf.Clamp01(health / maxHealth);
            var barHeight = 5f;
            var barWidth = rect.width;
            
            var barRect = new Rect(rect.x, rect.y - barHeight - 5, barWidth, barHeight);
            
            // Background
            DrawBox(barRect, Color.black, 1f);
            
            // Health fill
            var fillRect = new Rect(barRect.x, barRect.y, barRect.width * healthPercent, barRect.height);
            DrawBox(fillRect, Color.green, barHeight);
            
            // Health text
            DrawText(new Rect(rect.x, rect.y - barHeight - 20, barWidth, 20),
                   $"{health:F0}/{maxHealth:F0}", Color.white);
        }
        
        private void DrawSkeleton(Vector3[] bonePositions, Color color)
        {
            if (bonePositions == null || bonePositions.Length == 0) return;
            
            // Define bone connections
            var connections = new int[][]
            {
                new int[] { 0, 1 }, // Head to Neck
                new int[] { 1, 2 }, // Neck to Spine
                new int[] { 2, 3 }, // Spine to Hips
                new int[] { 1, 4 }, // Neck to LeftShoulder
                new int[] { 4, 5 }, // LeftShoulder to LeftArm
                new int[] { 5, 6 }, // LeftArm to LeftForearm
                new int[] { 6, 7 }, // LeftForearm to LeftHand
                new int[] { 1, 8 }, // Neck to RightShoulder
                new int[] { 8, 9 }, // RightShoulder to RightArm
                new int[] { 9, 10 }, // RightArm to RightForearm
                new int[] { 10, 11 }, // RightForearm to RightHand
                new int[] { 3, 12 }, // Hips to LeftThigh
                new int[] { 12, 13 }, // LeftThigh to LeftCalf
                new int[] { 13, 14 }, // LeftCalf to LeftFoot
                new int[] { 3, 15 }, // Hips to RightThigh
                new int[] { 15, 16 }, // RightThigh to RightCalf
                new int[] { 16, 17 }  // RightCalf to RightFoot
            };
            
            foreach (var connection in connections)
            {
                if (connection.Length != 2) continue;
                
                var start = _mainCamera.WorldToScreenPoint(bonePositions[connection[0]]);
                var end = _mainCamera.WorldToScreenPoint(bonePositions[connection[1]]);
                
                if (start.z > 0 && end.z > 0)
                {
                    DrawLine(start, end, color, 1f);
                }
            }
        }
        
        private bool IsVisible(Transform transform)
        {
            if (transform == null) return false;
            
            // Simple visibility check using Raycast
            var direction = transform.position - _mainCamera.transform.position;
            if (Physics.Raycast(_mainCamera.transform.position, direction.normalized, out var hit, direction.magnitude))
            {
                // Check if we hit the player
                var playerRenderer = transform.GetComponentInChildren<Renderer>();
                if (playerRenderer != null && hit.collider.gameObject == playerRenderer.gameObject)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
        
        private Vector3 GetHeadPosition(Transform transform)
        {
            // Try to find head bone
            var head = FindChild(transform, "Head");
            if (head != null) return head.position;
            
            // Fallback to top of collider
            var collider = transform.GetComponent<Collider>();
            if (collider != null)
            {
                return collider.bounds.center + Vector3.up * collider.bounds.extents.y;
            }
            
            return transform.position + Vector3.up * 1.8f;
        }
        
        private Vector3 GetFootPosition(Transform transform)
        {
            var collider = transform.GetComponent<Collider>();
            if (collider != null)
            {
                return collider.bounds.center - Vector3.up * collider.bounds.extents.y;
            }
            
            return transform.position - Vector3.up * 0.9f;
        }
        
        private Vector3[] GetBonePositions(Transform transform)
        {
            var bones = new Vector3[_boneNames.Length];
            
            for (int i = 0; i < _boneNames.Length; i++)
            {
                var bone = FindChild(transform, _boneNames[i]);
                bones[i] = bone != null ? bone.position : transform.position;
            }
            
            return bones;
        }
        
        private Transform FindChild(Transform parent, string name)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    return child;
                }
            }
            return null;
        }
        
        // Settings class for runtime modification
        public static class OnlyVisible
        {
            public static bool Value = false;
        }
    }
}
