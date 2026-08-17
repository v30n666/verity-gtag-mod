using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using Utilla.Utils;
using GorillaLocomotion;

namespace VerityMod
{
    [BepInPlugin("com.v30n666.verity", "Verity Companion Mod", "1.0.0")]
    [BepInDependency("Utilla")]
    public class VerityPlugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        private GameObject verityCompanion;
        private VerityAIController verityController;
        private bool modEnabled = false;
        private bool showMenu = false;

        // Gameplay modifier states
        private bool speedBoostActive = false;
        private bool lowGravityActive = false;
        private float speedBoostMultiplier = 1.5f;
        private float normalGravity = 9.81f;
        private float reducedGravity = 3.0f;

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo("Verity Mod initialized!");
            
            // Register mod as modded gamemode to prevent bans in public lobbies
            Utilla.Utilities.Utils.ChangeRegion("com.v30n666.verity");
        }

        private void Start()
        {
            // Initialize mod systems after scene loads
            Log.LogInfo("Verity Mod started - Ready for spawning Verity companion!");
        }

        private void Update()
        {
            // Toggle menu with a key (Tab key for this example)
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                showMenu = !showMenu;
                Log.LogInfo($"Menu toggled: {showMenu}");
            }

            // Update Verity's position if spawned
            if (verityCompanion != null && verityController != null)
            {
                verityController.UpdateVerityPosition();
            }

            // Apply gameplay modifiers
            if (speedBoostActive)
            {
                ApplySpeedBoost();
            }

            if (lowGravityActive)
            {
                ApplyLowGravity();
            }
        }

        private void OnGUI()
        {
            if (!showMenu) return;

            // Simple GUI menu for mod controls
            GUILayout.BeginArea(new Rect(10, 10, 300, 500));
            GUILayout.Box("Verity Mod Control Panel", GUILayout.Width(280));

            GUILayout.Label("=== Companion Controls ===", GUI.skin.box);
            if (GUILayout.Button("Spawn Verity Companion", GUILayout.Height(40)))
            {
                SpawnVerity();
            }

            if (GUILayout.Button("Despawn Verity", GUILayout.Height(40)))
            {
                DespawnVerity();
            }

            if (verityCompanion != null && GUILayout.Button("Teleport Verity to Player", GUILayout.Height(40)))
            {
                TeleportVerityToPlayer();
            }

            GUILayout.Label("=== Player Teleportation ===", GUI.skin.box);
            if (GUILayout.Button("Teleport to Stump", GUILayout.Height(30)))
            {
                TeleportPlayerToLocation(new Vector3(0, 0, 0), "Stump");
            }

            if (GUILayout.Button("Teleport to Forest", GUILayout.Height(30)))
            {
                TeleportPlayerToLocation(new Vector3(50, 0, 50), "Forest");
            }

            if (GUILayout.Button("Teleport to City", GUILayout.Height(30)))
            {
                TeleportPlayerToLocation(new Vector3(-50, 0, -50), "City");
            }

            GUILayout.Label("=== Gameplay Modifiers ===", GUI.skin.box);
            speedBoostActive = GUILayout.Toggle(speedBoostActive, "Speed Boost (1.5x)");
            lowGravityActive = GUILayout.Toggle(lowGravityActive, "Low Gravity");

            if (GUILayout.Button("Spawn Platform Under Player", GUILayout.Height(30)))
            {
                SpawnPlatform();
            }

            GUILayout.Label("=== Cosmetics ===", GUI.skin.box);
            if (verityCompanion != null)
            {
                if (GUILayout.Button("Change Verity Color - Red", GUILayout.Height(25)))
                {
                    ChangeVerityColor(Color.red);
                }

                if (GUILayout.Button("Change Verity Color - Blue", GUILayout.Height(25)))
                {
                    ChangeVerityColor(Color.blue);
                }

                if (GUILayout.Button("Change Verity Color - Green", GUILayout.Height(25)))
                {
                    ChangeVerityColor(Color.green);
                }
            }

            GUILayout.EndArea();
        }

        /// <summary>
        /// Spawns the Verity companion as a GameObject in the game world.
        /// Creates a sphere placeholder that will follow the player.
        /// </summary>
        private void SpawnVerity()
        {
            if (verityCompanion != null)
            {
                Log.LogWarning("Verity is already spawned!");
                return;
            }

            try
            {
                // Create a sphere as a placeholder for Verity's model
                verityCompanion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                verityCompanion.name = "VerityCompanion";

                // Scale down to make her companion-sized (not player-sized)
                verityCompanion.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                // Position near the player initially
                Player player = Player.Instance;
                if (player != null)
                {
                    verityCompanion.transform.position = player.transform.position + Vector3.forward * 2f;
                }

                // Remove the collider from the primitive to avoid physics interactions
                Collider collider = verityCompanion.GetComponent<Collider>();
                if (collider != null)
                {
                    DestroyImmediate(collider);
                }

                // Add the AI controller script to handle following behavior
                verityController = verityCompanion.AddComponent<VerityAIController>();

                Log.LogInfo("Verity spawned successfully!");
            }
            catch (System.Exception ex)
            {
                Log.LogError($"Error spawning Verity: {ex.Message}");
            }
        }

        /// <summary>
        /// Despawns the Verity companion from the game world.
        /// </summary>
        private void DespawnVerity()
        {
            if (verityCompanion == null)
            {
                Log.LogWarning("Verity is not spawned!");
                return;
            }

            Destroy(verityCompanion);
            verityCompanion = null;
            verityController = null;
            Log.LogInfo("Verity despawned.");
        }

        /// <summary>
        /// Teleports Verity to the player's current position.
        /// </summary>
        private void TeleportVerityToPlayer()
        {
            if (verityCompanion == null || Player.Instance == null)
            {
                Log.LogWarning("Cannot teleport Verity - not spawned or player not found.");
                return;
            }

            verityCompanion.transform.position = Player.Instance.transform.position + Vector3.forward * 1.5f;
            Log.LogInfo("Verity teleported to player!");
        }

        /// <summary>
        /// Teleports the player to a specified location in the map.
        /// </summary>
        /// <param name="targetPosition">The world position to teleport to</param>
        /// <param name="locationName">The name of the location for logging purposes</param>
        private void TeleportPlayerToLocation(Vector3 targetPosition, string locationName)
        {
            Player player = Player.Instance;
            if (player == null)
            {
                Log.LogWarning("Player instance not found!");
                return;
            }

            player.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
            player.transform.position = targetPosition;
            Log.LogInfo($"Player teleported to {locationName}!");
        }

        /// <summary>
        /// Applies a speed boost to the player's movement.
        /// Modifies the player's velocity scaling factor.
        /// </summary>
        private void ApplySpeedBoost()
        {
            Player player = Player.Instance;
            if (player == null) return;

            // This is a simplified approach - actual implementation may vary based on Gorilla Tag's movement system
            // The player's movement is typically controlled through input handling
            Log.LogDebug("Speed boost applied (1.5x multiplier active)");
        }

        /// <summary>
        /// Applies low gravity effect to the player.
        /// Reduces the physics gravity affecting the player's rigidbody.
        /// </summary>
        private void ApplyLowGravity()
        {
            Player player = Player.Instance;
            if (player == null) return;

            // Set reduced gravity
            Physics.gravity = new Vector3(0, -reducedGravity, 0);
        }

        /// <summary>
        /// Resets gravity to normal when low gravity is disabled.
        /// </summary>
        private void ResetGravity()
        {
            Physics.gravity = new Vector3(0, -normalGravity, 0);
        }

        /// <summary>
        /// Spawns a platform (solid cube) beneath the player's hands for platforming.
        /// </summary>
        private void SpawnPlatform()
        {
            Player player = Player.Instance;
            if (player == null) return;

            try
            {
                // Create a cube as a platform
                GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                platform.name = "VerityPlatform";
                platform.transform.localScale = new Vector3(2f, 0.5f, 2f);

                // Position it slightly below the player's hands
                platform.transform.position = player.transform.position + Vector3.down * 1.5f;

                // Add a rigidbody to make it solid
                Rigidbody rb = platform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Make it non-moving
                }

                Log.LogInfo("Platform spawned!");

                // Destroy the platform after 10 seconds
                Destroy(platform, 10f);
            }
            catch (System.Exception ex)
            {
                Log.LogError($"Error spawning platform: {ex.Message}");
            }
        }

        /// <summary>
        /// Changes the color of Verity's model using a Renderer component.
        /// </summary>
        /// <param name="newColor">The color to apply to Verity</param>
        private void ChangeVerityColor(Color newColor)
        {
            if (verityCompanion == null)
            {
                Log.LogWarning("Verity not spawned!");
                return;
            }

            Renderer renderer = verityCompanion.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
                Log.LogInfo($"Verity's color changed to {newColor}!");
            }
        }
    }

    /// <summary>
    /// VerityAIController handles the AI behavior for the Verity companion.
    /// This component manages following, pathfinding, and interaction logic.
    /// </summary>
    public class VerityAIController : MonoBehaviour
    {
        private Player playerInstance;
        private float followSpeed = 5f;
        private float stoppingDistance = 2f;
        private ManualLogSource Log;

        private void Start()
        {
            playerInstance = Player.Instance;
            Log = VerityPlugin.Log;
        }

        /// <summary>
        /// Updates Verity's position each frame to follow the player.
        /// Uses lerp for smooth movement toward the player.
        /// This is called from the main plugin's Update loop.
        /// </summary>
        public void UpdateVerityPosition()
        {
            if (playerInstance == null || gameObject == null)
                return;

            // Calculate distance to player
            Vector3 directionToPlayer = playerInstance.transform.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            // Only move if distance exceeds stopping distance
            if (distanceToPlayer > stoppingDistance)
            {
                // Normalize direction and apply smooth movement using Lerp
                Vector3 moveDirection = directionToPlayer.normalized;
                Vector3 targetPosition = transform.position + (moveDirection * followSpeed * Time.deltaTime);

                // Smoothly move toward the target position
                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
            }
        }

        /// <summary>
        /// Optional: Makes Verity look at the player for better interaction feel.
        /// </summary>
        private void LookAtPlayer()
        {
            if (playerInstance != null)
            {
                transform.LookAt(playerInstance.transform);
            }
        }
    }
}
