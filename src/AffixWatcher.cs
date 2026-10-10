using System.Collections.Generic;
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

        private static readonly List<Player> Nearby = new List<Player>();

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
            if (!Plugin.Enabled.Value || !_boss || _boss.IsDead() || !_boss.m_nview || !_boss.m_nview.IsValid()
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
                zdo.Set(ZdoLocked, true);
                enabled = false;
                return;
            }

            string current = Cllc.Get(_boss);
            if (current == "Twin" || current == "Godlike")
            {
                return;
            }

            string wanted = CountPlayers() <= 1 ? Plugin.SoloAffix.Value : Plugin.GroupAffix.Value;
            if (!Cllc.IsValid(wanted) || wanted == current)
            {
                return;
            }

            Cllc.Set(_boss, wanted);
            Plugin.Log.LogInfo($"BossGroupAffix: {_boss.m_name} affix {current} -> {wanted}.");
        }

        private int CountPlayers()
        {
            Nearby.Clear();
            Player.GetPlayersInRange(_boss.transform.position, Plugin.PlayerRange.Value, Nearby);
            int count = 0;
            foreach (Player player in Nearby)
            {
                if (player && !player.IsDead())
                {
                    count++;
                }
            }

            Nearby.Clear();
            return count;
        }
    }
}
