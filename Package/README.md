# Boss Affix By Players

Control when bosses can be summoned and optionally adjust their Creature Level and Loot Control (CL&LC) affixes based on the number of nearby players.

**Thunderstore Package:** `BossAffixByPlayers`  
**Assembly:** `Cjayride.BossGroupAffix`

## Features

### Boss Summon Requirements

By default, bosses cannot be summoned until enough living players are within range of the boss's spawn point.

- **Minimum Players:** 2 living players by default.
- **Player Range:** 180 meters from the boss spawn point.
- **Offering Protection:** Summoning items are not consumed when the requirements are not met.
- **HUD Notification:** Displays a center-screen message when a summon is blocked.
- **Configurable Messages:** Customize the notification text and cooldown.

Set `MinPlayersToStartFight = 0` to disable the summon requirement.

### Optional Solo/Group Affix Switching

The mod can optionally change a boss's CL&LC affix based on the number of nearby players.

When enabled with `SwapAffixByPlayerCount = true`:

- Applies the configured solo or group affix based on nearby player count.
- Updates the affix while the boss is at full health.
- Locks the affix when the boss first takes damage.

**Disabled by default.** When `SwapAffixByPlayerCount = false`, this mod does not modify boss affixes.

## Configuration

Config file:

`BepInEx/config/cjayride.bossgroupaffix.cfg`

| Setting | Default | Description |
|---|---|---|
| `Enabled` | `true` | Enables or disables the mod. |
| `MinPlayersToStartFight` | `2` | Minimum living players required within range to summon a boss. Set to `0` to disable the requirement. |
| `PlayerRange` | `180` | Distance in meters used to check nearby players. |
| `FightBlockedMessage` | See config file | HUD notification shown when a summon is blocked. Supports `{0}` for the required player count, `{1}` for the range, and `{2}` for the current player count. |
| `MessageCooldownSeconds` | `3` | Minimum time between repeated notifications. |
| `SwapAffixByPlayerCount` | `false` | Enables optional solo/group CL&LC affix switching. |

### Affix Configuration

These settings apply only when `SwapAffixByPlayerCount = true`:

- `SoloAffix` — Affix used when fighting solo.
- `GroupAffix` — Affix used when fighting with a group.

## Installation

Install **Boss Affix By Players** on the **server and all clients**.

## Compatibility

- Verified against Valheim 1.0.21. BepInEx dependency: `denikson-BepInExPack_Valheim-5.4.2360`.

