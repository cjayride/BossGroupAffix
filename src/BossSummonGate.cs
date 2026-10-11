using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Cjayride.BossGroupAffix
{
    /// <summary>
    /// Boss offerings (OfferingBowl) do not start unless enough living players are within PlayerRange
    /// of the boss spawn point. Items are not consumed when the check fails.
    /// </summary>
    [HarmonyPatch(typeof(OfferingBowl), "InitiateSpawnBoss")]
    internal static class BossSummonGate_Initiate
    {
        private static readonly FieldInfo InteractUser = AccessTools.Field(typeof(OfferingBowl), "m_interactUser");

        private static bool Prefix(OfferingBowl __instance, Vector3 point, bool removeItemsFromInventory)
        {
            if (!Plugin.Enabled?.Value ?? true)
            {
                return true;
            }

            int required = Plugin.MinPlayersToStartFight?.Value ?? 0;
            if (required <= 0 || __instance == null || __instance.m_bossPrefab == null)
            {
                return true;
            }

            int present = BossNearbyPlayers.CountLivingNear(point);
            if (present >= required)
            {
                return true;
            }

            Humanoid user = InteractUser?.GetValue(__instance) as Humanoid;
            BossSummonMessages.Show(user, required, present);
            return false;
        }
    }

    [HarmonyPatch(typeof(OfferingBowl), "RPC_SpawnBoss")]
    internal static class BossSummonGate_Rpc
    {
        private static bool Prefix(OfferingBowl __instance, Vector3 point, bool removeItemsFromInventory)
        {
            if (!Plugin.Enabled?.Value ?? true)
            {
                return true;
            }

            int required = Plugin.MinPlayersToStartFight?.Value ?? 0;
            if (required <= 0 || __instance == null || __instance.m_bossPrefab == null)
            {
                return true;
            }

            if (BossNearbyPlayers.CountLivingNear(point) >= required)
            {
                return true;
            }

            return false;
        }
    }

    internal static class BossSummonMessages
    {
        private static float _lastMessageTime;

        internal static void Show(Humanoid user, int required, int present)
        {
            if (user == null)
            {
                return;
            }

            if (user != Player.m_localPlayer)
            {
                return;
            }

            float cooldown = Plugin.MessageCooldownSeconds?.Value ?? 3f;
            if (Time.time - _lastMessageTime < cooldown)
            {
                return;
            }

            _lastMessageTime = Time.time;
            string template = Plugin.FightBlockedMessage?.Value
                ?? "Need at least {0} players within {1}m of the boss spawn to summon ({2} nearby).";
            string text = string.Format(
                template,
                required,
                Mathf.RoundToInt(Plugin.PlayerRange?.Value ?? 0f),
                present);
            user.Message(MessageHud.MessageType.Center, text);
        }
    }
}
