using HarmonyLib;
using UnityEngine;

namespace Cjayride.BossGroupAffix
{
    [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
    internal static class Character_Awake_Patch
    {
        private static void Postfix(Character __instance)
        {
            if (__instance && __instance.IsBoss() && !__instance.GetComponent<AffixWatcher>())
            {
                __instance.gameObject.AddComponent<AffixWatcher>();
            }
        }
    }

    internal sealed class AffixWatcher : MonoBehaviour
    {
        private const string ZdoLocked = "cjayride.BossGroupAffix.Locked";
        private const float Interval = 2f;

        private Character _boss;
        private float _timer;

        private void Awake()
        {
            _boss = GetComponent<Character>();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < Interval)
            {
                return;
            }

            _timer = 0f;
            Check();
        }

        private void Check()
        {
            if (!Cllc.Ready || !Plugin.Enabled.Value || !_boss || _boss.IsDead() || !_boss.m_nview || !_boss.m_nview.IsValid()
                || !_boss.m_nview.IsOwner())
            {
                return;
            }

            ZDO zdo = _boss.m_nview.GetZDO();
            if (zdo.GetBool(ZdoLocked))
            {
                enabled = false;
                return;
            }

            if (_boss.GetHealth() < _boss.GetMaxHealth())
            {
                ApplyAffixForPlayerCount();
                zdo.Set(ZdoLocked, true);
                enabled = false;
                return;
            }

            ApplyAffixForPlayerCount();
        }

        private void ApplyAffixForPlayerCount()
        {
            string current = Cllc.Get(_boss);
            if (current == "Twin" || current == "Godlike")
            {
                return;
            }

            int count = BossNearbyPlayers.CountLivingNear(_boss);
            string wanted = count <= 1 ? Plugin.SoloAffix.Value : Plugin.GroupAffix.Value;
            if (!Cllc.IsValid(wanted) || wanted == current)
            {
                return;
            }

            Cllc.Set(_boss, wanted);
            Plugin.Log.LogInfo($"Boss Affix By Players: {_boss.m_name} affix {current} -> {wanted} ({count} player(s) in range).");
        }
    }
}
