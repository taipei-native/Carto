<p align="center">
    <picture>
        <source media="(prefers-color-scheme: dark)" srcset="./Documentation/Banner-Dark.png">
        <source media="(prefers-color-scheme: light)" srcset="./Documentation/Banner-Bright.png">
        <img alt="Carto Logo" src="./Documentation/Banner-Bright.png" width=250>
    </picture>
</p>

# Carto

**Carto** is a Cities: Skylines II mod designed to extract in-game spatial data and convert it into widely used geospatial file formats.

## Features

* **Vector data export** – Export buildings, districts, map tiles, pathways, POIs, roads, routes, tracks, zoning cells, and more.
* **Raster data export** – Export terrain elevation and water depth.
* **Attribute-rich output** – Includes object attributes such as names, direction, and other metadata.
* **Multiple file formats** – Supports GeoJSON and ESRI Shapefile for vector data; GeoTIFF for raster data.
* **Projection support** – Uses WGS84 / UTM (EPSG: 326xx / 327xx) by default. Custom Transverse Mercator projections are also supported.

## Installation

Carto is available exclusively via [Paradox Mods](https://mods.paradoxplaza.com/mods/87428/Windows), the official distribution platform. You can install it through the in-game mod browser, their website, or via [Skyve](https://github.com/JadHajjar/Skyve).

## Getting Started

1. Load a save, start a new game, or open the editor.
2. Open **Options** and select **Carto** from the sidebar.
3. In the **General** tab, choose file format and output name.
4. Select data types to export in the **Feature** tab.
5. Configure geometry and attributes in the **Properties** tab.
6. Set your projection in the **Projection** tab.
7. Click **Export Files** in the **General** tab.
8. Your data will be saved to: `C:\Users\<YourName>\AppData\LocalLow\Colossal Order\Cities Skylines II\ModsData\Carto`

See the [Tutorials](https://github.com/taipei-native/Carto/wiki/Tutorial) for making maps with QGIS, and the [User Manual](https://github.com/taipei-native/Carto/wiki) for detailed documentation.

## Peer-mod integration

Carto exposes a public API at `Carto.IO.IO.Export(Carto.IO.Options)` so other mods can drive exports on their own cadence — for example, a storytelling mod that snapshots the city every few minutes. The peer-API path does not read the player's saved settings, validates inputs without showing `ErrorDialog`s, and reports outcome through `ExportResult` (`Success`, `FilesWritten`, `ErrorMessage`). Completion sound and dialog are honored from the caller's own `Options.CompletionSound` / `Options.CompletionDialog` flags (default `true` on a fresh `Options` — peer mods that want silent runs should explicitly set them `false`).

The caller is responsible for building a valid `Options`. In particular, the **full projection chain** must be wired so output lands in meters rather than degrees:

```csharp
var options = new Carto.IO.Options
{
    CustomDirectory = @"C:\path\to\output",
    VectorFormat    = Carto.IO.FileFormat.GeoJSON,
    Systems         = Carto.IO.System.Area | Carto.IO.System.Building | Carto.IO.System.Network,
    Features        = Carto.IO.Feature.District | Carto.IO.Feature.Building | Carto.IO.Feature.Road,
    Properties      = /* per-system property sets */,
    VectorKinds     = /* per-system VectorKind selections */,
    Display         = /* per-property display flags */,
    CompletionSound = false,
    CompletionDialog = false,

    // Projection chain — required for meter-scale output.
    SourceProjection           = Carto.Geodata.CRS.UTM,
    TargetProjection           = Carto.Geodata.CRS.UTM,
    TargetEllipsoid            = Carto.IO.Ellipsoid.WGS84,
    SourceCoordinates          = new Carto.Geodata.Coord(0, 0, Carto.Geodata.Hemisphere.North, 31),
    SourceProjectionDefinition = new Carto.Geodata.ProjectionDefinition(
        Carto.IO.IO.EllipsoidTable[Carto.IO.Ellipsoid.WGS84],
        0d, 0d, 5E5d, 0d, 0.9996d,
        new Carto.Geodata.HelmertTransform(new double[0])),
    TargetProjectionDefinition = /* same shape */,
};

Carto.IO.ExportResult result = Carto.IO.IO.Export(options);
```

Because Carto is rebuilt against each Cities: Skylines II patch, downstream mods should reflect on Carto's assembly at runtime (see `Carto.Domain.RoadBuilder` and `Carto.Domain.ExtendedTransportManager` for the established pattern) rather than taking a hard reference.

```csharp
public class ExportResult
{
    public bool Success { get; set; }
    public string[] FilesWritten { get; set; }
    public string ErrorMessage { get; set; }     // populated on failure
}

public static ExportResult Export(Options options);
```

## Credits

Since April 2024, when I started to develop Carto, I’ve learned from and been inspired by the open-source work of amazing Cities: Skylines II modders including **algernon**, **Guo**, **krzychu124**, **TDW**, and **yenyang** — a big shout-out to them!

Special thanks also to beta testers like **Allegretic**, **Excellent Guy**, **jefferyharrell**, **LightLight**, and members of the *Cities: Skylines Modding* and *Cities: Skylines Taiwan Assets* Discord servers for their feedback and support.

Carto also includes C# adaptations of parts of [PROJ](https://github.com/OSGeo/PROJ) and [PROJ4JS](https://github.com/proj4js/proj4js), optimized for Unity’s Burst compiler. License details are available in the `Licenses` directory.

Finally, sincere thanks to everyone who contributed translations on [Crowdin](https://crowdin.com/project/carto):

* Dutch: **GeraspteGatenKaas**
* French: **Morgan · Toverux** and **Wateir**

## Contact

You can reach out via:

* [Paradox Forum](https://forum.paradoxplaza.com/forum/threads/carto.1699089/) (English / Chinese)
* [Cities: Skylines Modding Discord](https://discord.gg/HTav7ARPs2) (English)
* [Cities: Skylines Taiwan Assets Discord](https://discord.gg/Gz4K66jT64) (Chinese preferred)

For bug reports or in-depth discussions, please use [GitHub Issues](https://github.com/taipei-native/Carto/issues) or [Discussions](https://github.com/taipei-native/Carto/discussions) to keep things organized.

## License

This project is licensed under the MIT License. See [LICENSE.txt](LICENSE.txt) for full details.
