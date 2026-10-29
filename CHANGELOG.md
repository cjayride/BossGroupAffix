# Changelog

## 1.0.5

- Verified against Valheim 1.0.21 (Release build).
- BepInEx dependency set to `denikson-BepInExPack_Valheim-5.4.2360`.

## 1.0.4

- **MinPlayersToStartFight** (default 2): boss **summon** (OfferingBowl) is blocked until enough living players are within **PlayerRange** of the spawn point; trophies are not consumed; local player gets a throttled center HUD message.
- **SwapAffixByPlayerCount** (default **false**): optional CL&LC solo/group affix swap; when off, this mod does not change affixes.

## 1.0.3

- **MinPlayersToDamage** (default 2): bosses take no damage unless that many living players are within PlayerRange. Stops solo chip and group cleanup after a solo-tagged fight.
- Defaults: SoloAffix Reflective, PlayerRange 180. Affix is refreshed from nearby player count when the boss first takes damage, then locked.

## 1.0.2

- Thunderstore package renamed to **BossAffixByPlayers**; description covers any configurable solo/group CL&LC affix.
- Soft dependency on CL&LC so the plugin and config file still load if CL&LC is missing (affix swap stays off until CL&LC is present).
- Saves config on startup and logs the config path.

## 1.0.1

- New icon.

## 1.0.0

- First release. Bosses get Mending when fought solo and Summoner when fought as a group.
