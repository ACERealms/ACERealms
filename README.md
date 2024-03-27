# SeasonalACE

## Summary

Seasonal ACE is a fork of [ACEmulator](https://github.com/ACEmulator/ACE) and  [ACRealms](https://github.com/ACRealms/ACRealms.WorldServer). It comes with instancing functionality, specifically realms, also known as "seasons". A season can be thought of as an instance of an entire AC world server, however, Seasonal ACE can support multiple seasons on a single server simultaneously. Additionally, a season also has the capability to support instance dungeon landblocks, where multiple dungeons of the same landblock can exist, with their own rules. 

This project is experimental, and exists as a base for any projects that wish to have instancing with Asheron's Call, and can be played the same as any other ACE server. 


## Caveats

1. `LandblockInstance` entities are shared across seasons. This means all static guids belonging to a `LandblockInstance` are the same no matter which realm  your character exists in. 
2. Because all static guids are shared, housing cannot exist in multiple seasons independently. SeasonalACE only supports housing in the current active season, past seasons that are no longer active have all housing related biotas removed and housing storage cleared (characters can still be played in past seasons, just without housing).
3. There are still unforseen bugs. ACRealms never had much exposure and is not very battle tested but there should be no serious impact on performance or player experience. 


## Custom Realm Config
* The `Content\json\realms.jsonc` is your root config file. In this file you define key/value pairs, where the key is the name of a realm you have created (more on this next), and the value is the realm identifier that is used internally.
* The `Content\json\realms\realm` directory is where your realm JSON files exist, these are what I would describe as your seasons. Realms have a parent/child relationship, multiple realms can inherit rules from other realms. You will find many config files that you can use as a foundation and example to learn from. 

For example the default season for SeasonalACE is the `realms-pvp.jsonc` file:

This realm inherits from the `Modern Realm`.

```json
{
  "name": "Modern Realm (PvP)",
  "type": "Realm",
  "parent": "Modern Realm",
  "properties": {
    "Description": "The Customized PvP Realm with the latest and greatest features.",
    "IsPKOnly": true
  }
}
```


* The `Custom\json\realms\rulesets` directory contains rulesets config files. Rulesets should be used for dungeon landblock instancing only. Rulesets are unique as they can be composed with other rulesets, and the values for certain rules can be applied with RNG values.

Here is a `random-test` ruleset example:

```json
{
  "name": "random-test",
  "type": "Ruleset",
  "properties": {
    "Description": "A test ruleset for random properties.",
    "Spellcasting_Max_Angle": {
      "low": "5",
      "high": "180",
      "reroll": "landblock" /*never, always, landblock, admin */,
      "locked": true,
      "probability": 0.5
    },
    "CreatureSpawnHPMultiplier": {
      "low": 0.5,
      "high": 2.0,
      "compose": "add"
    }
  },
  "apply_rulesets": ["creature-attribute-randomizer"],
  "apply_rulesets_random": [
    [{ "creature-hp-boost": 0.5 }, { "creature-hp-boost": 1.0 }],
    { "pvp-ruleset": "auto" },
    { "creature-attribute-randomizer": 0.5 }
  ]
}
```

## Commands

**Admin Commands**
1. `/change-season <realmId>` - This command changes the current active season to a new realmId, newly created players will be created in this season, having no physical interactions with other seasoned characters. Transitioning to a new season clears housing and housing storage from the previous active season.
2. `/exiti` - Exits the current ephemeral realm (dungeon landblock instance).

**Player Commands**
1. `/season-info` - This command gives you information about the current active season that a player belongs to.
2. `/season-list` - This command displays a list of all available seasons to choose from, highlighting the current active season.
3. `/realm-list` - This command displays all realms that exist in the realms.jsonc file.
4. `/ruleset-list` - This command displays all rulesets that exist in rulesets directory.
5. `/realm-info` - This command displays all realm information for the current realm a player exists in (useful for dungeon landblock realms, also known as ephemeral realms).