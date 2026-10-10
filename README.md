# BossGroupAffix

Picks a boss's Creature Level & Loot Control affix from how many players are near it.

- One player nearby: the boss gets **Mending** (heals over time).
- Two or more players nearby: the boss gets **Summoner** (calls in adds).

The choice updates every 2 seconds while the boss is at full health. Once the boss takes damage, the affix is locked for the rest of the fight.

Twin and Godlike bosses are left alone.

## Configuration

`cjayride.bossgroupaffix.cfg`

| Setting | Default | Description |
|---|---|---|
| Enabled | true | Turn the mod on or off. |
| SoloAffix | Mending | Affix for a boss with one player nearby. |
| GroupAffix | Summoner | Affix for a boss with two or more players nearby. |
| PlayerRange | 400 | Meters from the boss. Players this close count. |

Affix choices: None, Reflective, Shielded, Mending, Summoner, Elementalist, Enraged, Sturdy.

Mending strength comes from Creature Level & Loot Control's `Mending healing factor` setting. The default of 1 heals noticeably; very low values like 0.05 barely heal.

Install on the server and all clients.
