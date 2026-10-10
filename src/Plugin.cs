using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace Cjayride.BossGroupAffix
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(CllcGuid)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = "cjayride.bossgroupaffix";
        public const string PluginName = "BossGroupAffix";
        public const string PluginVersion = "1.0.0";
        internal const string CllcGuid = "org.bepinex.plugins.creaturelevelcontrol";

        internal static Plugin Instance;
        internal static ConfigEntry<bool> Enabled;
        internal static ConfigEntry<string> SoloAffix;
        internal static ConfigEntry<string> GroupAffix;
        internal static ConfigEntry<float> PlayerRange;

        internal static BepInEx.Logging.ManualLogSource Log => Instance.Logger;

        private void Awake()
        {
            Instance = this;
            Enabled = Config.Bind("General", "Enabled", true,
                "Pick the boss affix from how many players are near the boss before the fight starts.");
            SoloAffix = Config.Bind("General", "SoloAffix", "Mending",
                "Creature Level & Loot Control affix for a boss with one player nearby. " +
                "One of: None, Reflective, Shielded, Mending, Summoner, Elementalist, Enraged, Sturdy.");
            GroupAffix = Config.Bind("General", "GroupAffix", "Summoner",
                "Affix for a boss with two or more players nearby. Same choices as SoloAffix.");
            PlayerRange = Config.Bind("General", "PlayerRange", 400f,
                "Meters from the boss. Players this close count toward solo or group.");

            if (!Cllc.Init())
            {
                Logger.LogWarning("BossGroupAffix: Creature Level & Loot Control API not found; boss affixes are left alone.");
                return;
            }

            new Harmony(PluginGUID).PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    internal static class Cllc
    {
        static Type _affixType;
        static MethodInfo _get;
        static MethodInfo _set;

        internal static bool Init()
        {
            Type api = AccessTools.TypeByName("CreatureLevelControl.API");
            _affixType = AccessTools.TypeByName("CreatureLevelControl.BossAffix");
            if (api == null || _affixType == null)
            {
                return false;
            }

            _get = AccessTools.Method(api, "GetAffixBoss", new[] { typeof(Character) });
            _set = AccessTools.Method(api, "SetAffixBoss", new[] { typeof(Character), _affixType });
            return _get != null && _set != null;
        }

        internal static string Get(Character boss)
        {
            return _get.Invoke(null, new object[] { boss })?.ToString() ?? "None";
        }

        internal static void Set(Character boss, string affix)
        {
            _set.Invoke(null, new object[] { boss, Enum.Parse(_affixType, affix) });
        }

        internal static bool IsValid(string affix)
        {
            if (string.IsNullOrEmpty(affix) || affix == "Twin" || affix == "Godlike")
            {
                return false;
            }

            try
            {
                Enum.Parse(_affixType, affix);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
