using System.Collections.Generic;
using UnityEngine;

namespace Cjayride.BossGroupAffix
{
    internal static class BossNearbyPlayers
    {
        private static readonly List<Player> Buffer = new List<Player>();

        internal static int CountLivingNear(Character boss)
        {
            if (!boss)
            {
                return 0;
            }

            return CountLivingNear(boss.transform.position);
        }

        internal static int CountLivingNear(Vector3 position)
        {
            float range = Plugin.PlayerRange?.Value ?? 0f;
            if (range <= 0f)
            {
                return 0;
            }

            Buffer.Clear();
            Player.GetPlayersInRange(position, range, Buffer);
            int count = 0;
            for (int i = 0; i < Buffer.Count; i++)
            {
                Player player = Buffer[i];
                if (player && !player.IsDead())
                {
                    count++;
                }
            }

            Buffer.Clear();
            return count;
        }
    }
}
