using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using UnityEngine;
using CUCoreLib.ContentReload;
using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using CUCoreLib.Saving;
using Newtonsoft.Json.Linq;
using System.Drawing.Text;

namespace ModNamespace
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInDependency("net.cucorelib", BepInDependency.DependencyFlags.HardDependency)]
    
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "com.lesothedoes.stimStogie";
        public const string ModName = "Mod that adds a cigar to the game. Homage to the phantom cigar, effects not canon."; // To change .dll name, change the name in vars.targets
        public const string ModVersion = "1.0.0.0";

        internal static new ManualLogSource Logger;
        private readonly Harmony _harmony = new(ModGUID);
        public static Plugin Instance { get; private set; } = null!;

        public void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            _harmony.PatchAll();
            Logger.LogInfo($"Plugin {ModName} is loaded!");

            DoStuff();
            ContentReloadManager.EnableHotReload(ModGUID); // Required for hot reload, put any registry registrations in the RegisterReloadable method below
            RegisterReloadable();
        }

        private void RegisterReloadable()
        {
            Sprite stimStogie = AssetLoader.LoadEmbeddedSprite("images.stimStogie.png");

            ItemRegistry.Register(
                "stimStogie",
                new CustomItemInfo
                {
                    fullName = "Stimulating Stogie",
                    description = "A cigar that gives you a boost of energy, happiness, and spry.",
                    category = "drug",
                    tags = "cangetwet",
                    wearable = true,
                    wearableCanBeHeld = true,
                    desiredWearLimb = "Head",
                    wearSlotId = "mouth",
                    weight = 0.1f,
                    wearableArmor = 0.045f,
                    wearableIsolation = 0.7f,
                    wearableHitDurabilityLossMultiplier = 0.25f,
                    decayInfo = 17,
                    wearableVisualOffset = 4,
                    value = 20,
                    SpawnFrequency = 1,
                    DropPool = DropPool.All,
                    WorldSpawnPerChunk = 0.0375f,
                    SpriteScale = 1f,
                    WornSpriteOffset = new Vector2(0f, -0.04f),
                    WornSprite = stimStogie,
                    rec = new Recognition(9),
                    Battery = new BatteryProperties
                    {
                        StartCharge = 100f,
                        Preset = BatteryItem.BatteryPreset.Medium
                    },
                    Light = new LightProperties
                    {
                        Color = Color.yellow,
                        Intensity = 0.1875f,
                        FalloffIntensity = 1f,
                        LightType = CustomLightType.Point,
                        Rotation = 0f,
                        Offset = Vector2.zero
                    },
                    CustomData =
                    {
                        ["happinessBonus"] = 10f,
                        ["energyBonus"] = 5f,
                        ["spryBonus"] = 1.1875f,
                        ["warmthBonus"] = 0.7f
                    }
                }
                .AddSpawnComponent<stimStogieScript>(),
                stimStogie
                ); 
        }

        public sealed class stimStogieScript : MonoBehaviour
        {
            private Item item;
            private float warmthBonus = 0.7f;
            private float happinessBonus = 10f;
            private float staminaBonus = 5f;
            private float spryBonus = 1.1875f;


            private void Awake()
            {
                item = GetComponent<Item>();

                if (item != null)
                    warmthBonus = ItemRegistry.GetCustomData(item, "warmthBonus", 0.25f);
                    happinessBonus = ItemRegistry.GetCustomData(item, "happinessBonus", 10f);
                    staminaBonus = ItemRegistry.GetCustomData(item, "staminaBonus", 5f);
                    spryBonus = ItemRegistry.GetCustomData(item, "spryBonus", 1.1875f);
            }

            private void Update()
            {
                Limb wornLimb = item != null && item.transform.parent != null
                    ? item.transform.parent.GetComponent<Limb>()
                    : null;
                if (wornLimb == null) return;

                Body body = wornLimb.body;
                if (body == null) return;

                body.clothingTemperature += warmthBonus * Time.deltaTime;
                body.maxSpeed += spryBonus * Time.deltaTime;
                body.jumpSpeed += spryBonus * Time.deltaTime;
                body.clawRegrowTime -= spryBonus * Time.deltaTime;
                body.maxEncumberance += spryBonus + 0.3f * Time.deltaTime;
                body.stamina += spryBonus + 18.5f * Time.deltaTime;
                body.energy += spryBonus * Time.deltaTime;
                body.dirtyness -= spryBonus * Time.deltaTime;
                body.bloodPressure += 10f;
                body.heartRate += 10f;
                
            }
        }

        private void DoStuff()
        {
            
        }
    }
}
