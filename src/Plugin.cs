using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace Cjayride.BossGroupAffix
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(CllcGuid, BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = "cjayride.bossgroupaffix";
        public const string PluginName = "Boss Affix By Players";
        public const string PluginVersion = "1.0.5";
        internal const string CllcGuid = "org.bepinex.plugins.creaturelevelcontrol";

        internal static Plugin Instance;
        internal static ConfigEntry<bool> Enabled;
        internal static ConfigEntry<bool> SwapAffixByPlayerCount;
        internal static ConfigEntry<string> SoloAffix;
        internal static ConfigEntry<string> GroupAffix;
        internal static ConfigEntry<float> PlayerRange;
        internal static ConfigEntry<int> MinPlayersToStartFight;
        internal static ConfigEntry<string> FightBlockedMessage;
        internal static ConfigEntry<float> MessageCooldownSeconds;

        internal static BepInEx.Logging.ManualLogSource Log => Instance.Logger;

        private void Awake()
        {
            Instance = this;
            Enabled = Config.Bind("General", "Enabled", true,
                "When true, MinPlayersToStartFight and optional affix swapping are active.");
            SwapAffixByPlayerCount = Config.Bind("General", "SwapAffixByPlayerCount", false,
                "When true, CL&LC boss affix is chosen from nearby player count (solo vs group). When false, affix swapping is off.");
            SoloAffix = Config.Bind("Affix swap", "SoloAffix", "Reflective",
                "Used only when SwapAffixByPlayerCount is true and one living player is in range. " +
                "One of: None, Reflective, Shielded, Mending, Summoner, Elementalist, Enraged, Sturdy.");
            GroupAffix = Config.Bind("Affix swap", "GroupAffix", "Summoner",
                "Used only when SwapAffixByPlayerCount is true and two or more living players are in range.");
            PlayerRange = Config.Bind("General", "PlayerRange", 180f,
                "Meters from the boss. Living players inside this radius count toward starting the fight and toward affix swap.");
            MinPlayersToStartFight = Config.Bind("General", "MinPlayersToStartFight", 2,
                new ConfigDescription(
                    "Offering a boss summon requires at least this many living players within PlayerRange of the spawn point. 0 disables the check.",
                    new AcceptableValueRange<int>(0, 10)));
            FightBlockedMessage = Config.Bind("General", "FightBlockedMessage",
                "Need at least {0} players within {1}m of the boss spawn to summon ({2} nearby).",
                "HUD text when a summon is blocked. {0} = required count, {1} = PlayerRange, {2} = current count.");
            MessageCooldownSeconds = Config.Bind("General", "MessageCooldownSeconds", 3f,
                new ConfigDescription(
                    "Minimum seconds between fight-blocked messages for the local player.",
                    new AcceptableValueRange<float>(0.5f, 30f)));

            WarnIfAffixInvalid(SoloAffix);
            WarnIfAffixInvalid(GroupAffix);
            Config.Save();
            Logger.LogInfo($"{PluginName} {PluginVersion} loaded. Config: {Config.ConfigFilePath}");

            new Harmony(PluginGUID).PatchAll(Assembly.GetExecutingAssembly());

            if (!Cllc.Init())
            {
                Logger.LogWarning(
                    "Creature Level & Loot Control API not found; affix swapping is disabled until CL&LC is installed.");
            }
        }

        private static void WarnIfAffixInvalid(ConfigEntry<string> entry)
        {
            if (entry == null || Cllc.IsValid(entry.Value))
            {
                return;
            }

            Log.LogWarning($"Config {entry.Definition.Section}/{entry.Definition.Key} = \"{entry.Value}\" is not a valid CL&LC boss affix.");
        }
    }

    internal static class Cllc
    {
        static Type _affixType;
        static MethodInfo _get;
        static MethodInfo _set;

        internal static bool Ready => _get != null && _set != null;

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
            if (_affixType == null)
            {
                return !string.IsNullOrEmpty(affix) && affix != "Twin" && affix != "Godlike";
            }

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
