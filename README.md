# Boss Affix By Players

Thunderstore package name: **BossAffixByPlayers** (assembly: `Cjayride.BossGroupAffix`).

Before a boss takes damage, this mod sets its **Creature Level & Loot Control** affix from how many living players are within range. You choose **any** CL&LC boss affix for solo and for group in config. Twin and Godlike bosses are left alone.

The affix is rechecked every 2 seconds while the boss is at full health, then locked for the rest of the fight once it has taken damage.

## Configuration

**`BepInEx/config/cjayride.bossgroupaffix.cfg`**

The plugin writes this file on first run (defaults: Reflective solo, Summoner group, 180 m range).

| Setting | Default | Description |
|---|---|---|
| Enabled | true | Turn the mod on or off. |
| SoloAffix | Reflective | Affix when one player is in range. |
| GroupAffix | Summoner | Affix when two or more players are in range. |
| PlayerRange | 180 | Meters from the boss. |
| MinPlayersToDamage | 2 | Boss ignores damage if fewer living players are in range (0 = off). |

**SoloAffix / GroupAffix:** `None`, `Reflective`, `Shielded`, `Mending`, `Summoner`, `Elementalist`, `Enraged`, `Sturdy`.

Affix power (Mending heal rate, Reflective damage, Summoner count, etc.) comes from **Creature Level & Loot Control**'s boss affix settings.

Install on the **server and all clients**.
