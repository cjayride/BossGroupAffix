using HarmonyLib;

namespace Cjayride.BossGroupAffix
{
    /// <summary>
    /// Optional minimum living players within PlayerRange before a boss takes damage.
    /// Stops solo chip damage and avoids "solo affix then group cleanup" on the same spawn.
    /// </summary>
    [HarmonyPatch(typeof(Character), "RPC_Damage")]
    internal static class BossDamageGate
    {
        private static bool Prefix(Character __instance, HitData hit)
        {
            if (!Plugin.Enabled?.Value ?? true)
            {
                return true;
            }

            int required = Plugin.MinPlayersToDamage?.Value ?? 0;
            if (required <= 0 || __instance == null || hit == null || !__instance.IsBoss())
            {
                return true;
            }

            if (!__instance.m_nview || !__instance.m_nview.IsOwner())
            {
                return true;
            }

            if (BossNearbyPlayers.CountLivingNear(__instance) >= required)
            {
                return true;
            }

            return false;
        }
    }
}
