using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Reflection;

using UnityEngine;
using UnityEngine.U2D;

using VehicleFramework;
using VehicleFramework.VehicleParts;
using VehicleFramework.VehicleTypes;
using VehicleFramework.Engines;

using Nautilus.Options;

using BepInEx.Configuration;
using Nautilus.Utility;

namespace MobysDick
{
    public class MobysDick : Submersible
    {
        public static GameObject model = null;
        public static GameObject controlPanel = null;
        public static Atlas.Sprite pingSprite = null;
        public static Atlas.Sprite crafterSprite = null;

        public static void GetAssets()
        {
            // load the asset bundle
            string modPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(modPath, "assets/mobysdick"));
            if (myLoadedAssetBundle == null)
            {
                Patch.Log.LogError("Failed to load AssetBundle.");
                return;
            }
            object[] arr = myLoadedAssetBundle.LoadAllAssets();

            foreach (object obj in arr)
            {
                if (obj.ToString().Contains("SpriteAtlas"))
                {
                    SpriteAtlas thisAtlas = (SpriteAtlas)obj;

                    Sprite ping = thisAtlas.GetSprite("PingSprite");
                    pingSprite = new Atlas.Sprite(ping);

                    Sprite ping3 = thisAtlas.GetSprite("CrafterSprite");
                    crafterSprite = new Atlas.Sprite(ping3);
                }
                else if (obj.ToString().Contains("Moby's dick"))
                {
                    model = (GameObject)obj;
                }
            }
        }

        public override Dictionary<TechType, int> Recipe
        {
            get
            {
                Dictionary<TechType, int> recipe = new()
                {
                    { TechType.TitaniumIngot, 1 },
                    { TechType.Lubricant, 10 },
                    { TechType.AdvancedWiringKit, 1 },
                    { TechType.Silicone, 1 },
                    { TechType.Lead, 2 }
                };
                return recipe;
            }
        }

        public static IEnumerator Register()
        {
            GetAssets();
            Submersible mobysdick = model.EnsureComponent<MobysDick>();

            yield return UWE.CoroutineHost.StartCoroutine(VehicleRegistrar.RegisterVehicle(mobysdick));
        }

        public override string vehicleDefaultName
        {
            get
            {
                return "MOBY'S DICK";
            }
        }

        public override string Description
        {
            get
            {
                return "A submersible with excellent aerodynamic and water penetration properties.";
            }
        }

        public override string EncyclopediaEntry
        {
            get
            {
                /*
                 * The Formula:
                 * 2 or 3 sentence blurb
                 * Features
                 * Advice
                 * Ratings
                 * Kek
                 */
                string ency = "A submersible with excellent aerodynamic and water penetration properties.\n\n";
                ency += "Ratings:\n";
                ency += "- Top Speed: 12m/s \n";
                ency += "- Acceleration: 6m/s/s \n";
                ency += "- Distance per Power Cell: 7km \n";
                ency += "- Crush Depth: 200 \n";
                ency += "- Max Crush Depth (upgrade required): 900 \n";
                ency += "- Upgrade Slots: 4 \n";
                ency += "- Persons: 1\n";
                return ency;
            }
        }

        public override List<VehicleBattery> Batteries
        {
            get
            {
                var list = new List<VehicleBattery>();

                VehicleBattery vb1 = new()
                {
                    BatterySlot = transform.Find("CollisionModel/collider_mid1").gameObject
                };
                list.Add(vb1);

                return list;
            }
        }

        public override GameObject VehicleModel
        {
            get
            {
                return model;
            }
        }

        public override GameObject StorageRootObject
        {
            get
            {
                return transform.Find("StorageRoot").gameObject;
            }
        }

        public override GameObject ModulesRootObject
        {
            get
            {
                return transform.Find("ModulesRoot").gameObject;
            }
        }

        public override List<VehicleHatchStruct> Hatches
        {
            get
            {
                var list = new List<VehicleHatchStruct>();
                VehicleHatchStruct vhs = new();
                Transform hatches = transform.Find("Hatches");
                vhs.Hatch = transform.Find("CollisionModel/collider_front").gameObject;
                vhs.ExitLocation = hatches.Find("Exit");
                vhs.SurfaceExitLocation = hatches.Find("Exit");
                list.Add(vhs);
                return list;
            }
        }

        public override List<VehicleFloodLight> HeadLights
        {
            // https://github.com/NeisesMike/SubnauticaAtramaVehicle/wiki/Writing-the-C%23-for-your-Vehicle-Mod#list-headlights
            get
            {
                var list = new List<VehicleFloodLight>();

                VehicleFloodLight light1 = new()
                {
                    Light = transform.Find("lights_parent/headlights/light1").gameObject,
                    Angle = Patch.MDConfig.headlightsAngle,
                    Color = new Color(
                        Patch.MDConfig.headlightsRedColor,
                        Patch.MDConfig.headlightsGreenColor,
                        Patch.MDConfig.headlightsBlueColor),
                    Intensity = Patch.MDConfig.headlightsIntensity,
                    Range = Patch.MDConfig.headlightsRange
                };
                list.Add(light1);

                return list;
            }
        }

        public override List<VehicleStorage> InnateStorages
        {
            get
            {
                var list = new List<VehicleStorage>();

                Transform storage = transform.Find("CollisionModel/collider_back");

                VehicleStorage leftVS = new()
                {
                    Container = storage.gameObject,
                    Height = 8,
                    Width = 6
                };
                list.Add(leftVS);

                return list;
            }
        }

        public override List<VehicleUpgrades> Upgrades
        {
            get
            {
                var list = new List<VehicleUpgrades>();
                VehicleUpgrades vu = new()
                {
                    Interface = transform.Find("CollisionModel/collider_mid3").gameObject
                };
                vu.Flap = vu.Interface;
                list.Add(vu);
                return list;
            }
        }

        public override GameObject BoundingBox
        {
            get
            {
                return transform.Find("BoundingBox").gameObject;
            }
        }

        public override List<GameObject> WaterClipProxies
        {
            get
            {
                var list = new List<GameObject>();
                foreach (Transform child in transform.Find("WaterClipProxies"))
                {
                    list.Add(child.gameObject);
                }
                return list;
            }
        }

        public override List<GameObject> CanopyWindows
        {
            get
            {
                var list = new List<GameObject>
                {
                    transform.Find("Canopy").gameObject
                };
                return list;
            }
        }

        public override List<VehicleBattery> BackupBatteries => new();

        public override GameObject CollisionModel
        {
            get
            {
                return transform.Find("CollisionModel").gameObject;
            }
        }

        public override VehiclePilotSeat PilotSeat
        {
            get
            {
                VehiclePilotSeat vps = new();
                Transform pilotSeat = transform.Find("PilotSeat");
                vps.Seat = pilotSeat.gameObject;
                vps.SitLocation = pilotSeat.Find("SitLocation").gameObject;
                vps.LeftHandLocation = pilotSeat;
                vps.RightHandLocation = pilotSeat;
                return vps;
            }
        }

        public override ModVehicleEngine Engine
        {
            get
            {
                return gameObject.EnsureComponent<Engine>();
            }
        }

        public override Atlas.Sprite PingSprite
        {
            get
            {
                return pingSprite;
            }
        }

        public override int BaseCrushDepth
        {
            get
            {
                return 200;
            }
        }

        public override int CrushDepthUpgrade1
        {
            get
            {
                return 100; // 300
            }
        }

        public override int CrushDepthUpgrade2
        {
            get
            {
                return 200; // 500
            }
        }

        public override int CrushDepthUpgrade3
        {
            get
            {
                return 400; // 900
            }
        }

        public override int MaxHealth
        {
            get
            {
                return 420;
            }
        }

        public override int Mass
        {
            get
            {
                return 500;
            }
        }

        public override int NumModules
        {
            get
            {
                return 4;
            }
        }

        public override bool HasArms
        {
            get
            {
                return false;
            }
        }

        public override Atlas.Sprite CraftingSprite
        {
            get
            {
                return crafterSprite;
            }
        }

        public override List<VehicleStorage> ModularStorages
        {
            get
            {
                var list = new List<VehicleStorage>();

                VehicleStorage thisVS = new();
                Transform thisStorage = transform.Find("CollisionModel/collider_mid2");
                thisVS.Container = thisStorage.gameObject;
                thisVS.Height = 6;
                thisVS.Width = 5;
                list.Add(thisVS);

                return list;
            }
        }
    }
}
