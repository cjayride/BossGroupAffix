# Boss Affix By Players

Before a boss takes damage, sets its **Creature Level & Loot Control** affix from how many living players are nearby. You pick the solo affix and the group affix in config (Mending, Reflective, Summoner, Sturdy, or any other CL&LC boss affix except Twin/Godlike).

Rechecks every 2 seconds at full health; locks when the boss takes damage.

## Configuration

`BepInEx/config/cjayride.bossgroupaffix.cfg`

| Setting | Default | Description |
|---|---|---|
| Enabled | true | Turn the mod on or off. |
| SoloAffix | Reflective | One player in PlayerRange. |
| GroupAffix | Summoner | Two or more players in range. |
| PlayerRange | 180 | Meters from the boss. |
| MinPlayersToDamage | 2 | Boss ignores damage if fewer players are in range (0 = off). |

Choices: `None`, `Reflective`, `Shielded`, `Mending`, `Summoner`, `Elementalist`, `Enraged`, `Sturdy`.

Strength of each affix is tuned in CL&LC's config, not here.

Install on the server and all clients.
