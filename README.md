# Boss Affix By Players

Thunderstore package name: **BossAffixByPlayers** (assembly: `Cjayride.BossGroupAffix`).

## Boss summon gate (main feature)

By default, a boss **cannot be summoned** from an offering stone until at least **`MinPlayersToStartFight`** living players (default **2**) are within **`PlayerRange`** (default **180 m**) of the **boss spawn point**. The offering is **not consumed**. You get a **center-screen message** if the check fails (throttled, configurable text).

Set **`MinPlayersToStartFight = 0`** to turn the gate off.

## Optional affix swap (off by default)

If **`SwapAffixByPlayerCount = true`**, the mod sets CL&LC affix from nearby player count while the boss is at full health, then locks on first damage. **`SwapAffixByPlayerCount = false`** (default): this mod never changes boss affixes.

## Configuration

**`BepInEx/config/cjayride.bossgroupaffix.cfg`**

| Setting | Default | Description |
|---|---|---|
| Enabled | true | Master switch. |
| MinPlayersToStartFight | 2 | Required living players in range to damage the boss (0 = off). |
| PlayerRange | 180 | Meters from the boss. |
| FightBlockedMessage | (see cfg) | HUD text; `{0}` required, `{1}` range, `{2}` current count. |
| MessageCooldownSeconds | 3 | Throttle for the message. |
| SwapAffixByPlayerCount | false | Enable solo/group CL&LC affix swap. |

**[Affix swap]** (only if swap enabled): SoloAffix, GroupAffix.

Install on the **server and all clients**.
