# Related APIs

This document lists the game APIs and systems used by the Carto mod.

> [!INFO]
> It was tested using the following software versions:
>
> * Cities: Skylines II - `1.5.7f1`
> * Carto - `1.0.13`

## Colossal.IO.AssetDatabase

class　**AssetDatabase** \
├　property　*global* \
└　method　**LoadSettings(string, object, object, bool)**

* Carto.Mod

## Colossal.Localization

class　**LocalizationManager**

* Carto.Instance

## Colossal.Logging

interface　**ILog** \
├　method　**Info(object)** \
└　method　**SetShowsErrorsInUI(bool)**

* Carto.Instance
* Carto.Mod

class　**LogManager** \
└　method　**GetLogger(string)**

* Carto.Instance

## Colossal.PSI.Environment

class　**EnvPath** \
└　property *kUserDataPath*

* Carto.Instance

## Game

enum　**GameMode**

* Carto.Instance

enum　**SystemUpdatePhase**

* Carto.Mod

class　**UpdateSystem** \
└　method　**UpdateBefore\<T\>(Game.SystemUpdatePhase)**

* Carto.Mod

### Game.Audio

class　**AudioManager** \
└　method　**PlayUISound(Unity.Entities.Entity, float)**

* Carto.Instance
* Carto.Systems.SoundSystem

### Game.City

class　**CityConfigurationSystem**

* Carto.Instance

### Game.Economy

enum　**Resource**

* Carto.Domain.BuildingStat

### Game.Modding

class　**ModManager** \
└　class　**ModInfo** \
&ensp;　├　property　*asset* \
&ensp;　└　property　*name*

* Carto.Instance
* Carto.Domain.ExtendedTransportManager
* Carto.Domain.RoadBuilder
* Carto.Doamin.RoadSpeedAdjuster
* Carto.Domain.ZoneColorChanger

class　**ModSetting** \
├　method　**RegisterInOptionsUI()** \
└　method　**UnregisterInOptionsUI()**

* Carto.Mod

### Game.Prefabs

struct　**PrefabData**

* Carto.Domain.ZoningType

class　**PrefabSystem** \
└　method　**GetPrefabName(Unity.Entities.Entity)**

* Carto.Instance
* Carto.Domain.NameManager

### Game.SceneFlow

class　**GameManager** \
├　property　*gameMode* \
├　property　*instance* \
├　property　*localizationManager* \
├　property　*modManager* \
├　property　*settings* \
└　property　*userInterface*

* Carto.Instance

### Game.Settings

class　**SharedSettings**

* Carto.Instance

### Game.Simulation

class　**SimulationSystem**

* Carto.Instance

class　**TerrainSystem**

* Carto.Instance

class　**TimeSystem**

* Carto.Instance

class　**WaterSystem**

* Carto.Instance

### Game.UI

class　**MapMetadataSystem**

* Carto.Instance

class　**NameSystem** \
├　method　**GetDebugName(Unity.Entities.Entity)** \
└　method　**GetRenderedLabelName(Unity.Entities.Entity)**

* Carto.Domain.NameManager
