# Changelog

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
