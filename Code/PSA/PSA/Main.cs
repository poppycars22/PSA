using System;
using BepInEx;
using UnityEngine;
using UnboundLib;
using UnboundLib.Cards;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace PSA
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willuwontu.rounds.tabinfo", BepInDependency.DependencyFlags.SoftDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]

    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class Main : BaseUnityPlugin
    {
        private const string ModId = "com.Poppycars.PSA.Id";
        private const string ModName = "PoppycarsStatAdditions";
        public const string Version = "0.0.1"; // What version are we on (major.minor.patch)?
        public const string ModInitials = "PSA";

        internal static List<BaseUnityPlugin> plugins;
        public static Main instance { get; private set; }

        void Awake()
        {
            var harmony = new Harmony(ModId);

            harmony.PatchAll();
        }
        private void Start()
        {
            plugins = (List<BaseUnityPlugin>)typeof(BepInEx.Bootstrap.Chainloader).GetField("_plugins", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            instance = this;

            if(plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo"))
            {
                TabinfoInterface.Setup();
            }
        }
    }
}
