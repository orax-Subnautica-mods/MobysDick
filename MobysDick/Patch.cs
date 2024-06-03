using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Runtime.CompilerServices;
using System.Collections;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using Nautilus.Json;
using Nautilus.Handlers;
using Nautilus.Utility;
using BepInEx;
using BepInEx.Logging;


namespace MobysDick
{
    [BepInPlugin("com.orax.subnautica.mobysdick.mod", "MobysDickVehicle", "1.0.0")]
    [BepInDependency("com.mikjaw.subnautica.vehicleframework.mod")]
    [BepInDependency("com.snmodding.nautilus")]

    public class Patch : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        internal static MobysDickConfig MDConfig { get; private set; }
        public static Patch Instance { get; private set; }

        public Patch()
        {
            Instance = this;
        }

        private void Awake()
        {
            Log = Logger;
        }

        public void Start()
        {
            MDConfig = OptionsPanelHandler.RegisterModOptions<MobysDickConfig>();

            var harmony = new Harmony("com.orax.subnautica.mobysdick.mod");
            harmony.PatchAll();
            UWE.CoroutineHost.StartCoroutine(MobysDick.Register());
        }
    }

    [Menu("Moby's dick")]
    public class MobysDickConfig : ConfigFile
    {
        [Slider("Head lights angle", 0, 180, DefaultValue = 60)]
        public int headlightsAngle = 60;

        [Slider("Head lights red color", 0.0f, 1.0f, DefaultValue = 1.0f, Format = "{0:F2}", Step = 0.01f)]
        public float headlightsRedColor = 1.0f;

        [Slider("Head lights green color", 0.0f, 1.0f, DefaultValue = 1.0f, Format = "{0:F2}", Step = 0.01f)]
        public float headlightsGreenColor = 1.0f;

        [Slider("Head lights blue color", 0.0f, 1.0f, DefaultValue = 1.0f, Format = "{0:F2}", Step = 0.01f)]
        public float headlightsBlueColor = 1.0f;

        [Slider("Head lights intensity", 0.0f, 3.0f, DefaultValue = 1.0f, Format = "{0:F2}", Step = 0.01f)]
        public float headlightsIntensity = 1.0f;

        [Slider("Head lights range", 10, 1000, DefaultValue = 120)]
        public int headlightsRange = 120;
    }
}
