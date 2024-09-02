# Carto

**Carto** is a Cities: Skylines II mod aiming at collecting in-game objects’ spatial data and converting them into geospatial file formats.

### Features

* Export Vector Data - Carto currently supports exporting districts, map tiles, buildings, roads, tracks, pathways, points of interest (POIs), and zoning cells into GeoJSON and Esri Shapefile.
* Export Raster Data - Carto currently supports exporting the elevation and water depths into GeoTIFF.
* Additional Information - Carto can collect other vital data from your save with the vector geodata, such as asset names, driving directions, etc.
* UTM Projection - Carto uses WGS84 / UTM projections (EPSG: 326xx / 327xx) to reproject your save to real-life locations.

### Usage

Go to the options and click Carto in the sidebar. In the *General* tab, you can set the latitude and the longitude of your map’s center, as well as the exported file format. You can access individual item’s export settings in the *Custom Export* tab, where you can also configure each field’s status by clicking the “Show Advanced” button. Other miscellaneous options are in the *Miscellaneous* tab, including the advanced GeoTIFF format settings and other settings interacting with other mods. When satisfied with the settings, go to the *General* tab and hit the “Export Files” button. Once the button is clickable again, your files are already exported into Carto’s dedicated directory in the user data folder (`C:\Users\UserName\AppData\LocalLow\Colossal Order\Cities Skylines II\ModsData\Carto`). Regarding other features, please refer to the [user manual](https://github.com/taipei-native/Carto/wiki) for further information.

### Known Issues (for now)

* The “Custom Name” text input box and advanced field checkboxes may disappear when alternating options in the dropdown. Please double-click the “Show / Hide Advanced” button to force the game to update the UI status.
* Exporting GeoJSON is extremely slow when the number of features exceeds 10k ~ 20k. Please try to export them as shapefiles.
* Due to the complicated shape of networks (roads, tracks, and pathways), there might be some invalid geometries in the exported file. You can try using the “fix geometry” functions in some GIS software to solve the problems.

### Future Roadmap

* Ability to export public transport routes, natural resources, pollution, land values, etc.
* Advanced statistic module that reports detailed census data by districts or buildings (This may be integrated with other mods.)
* An external website to let users browse their saves (by uploading a compressed binary file) and download rendered maps.
*The list is not sorted by the priority*

### Supported Languages

**English** (en-US), **简体中文** (zh-HANS),  **繁體中文** (zh-HANT)

### Credits

Since April 2024, when I started to develop Carto, I’ve been referring to other amazing Cities: Skylines II modders’ open source codes available on GitHub, including but not limited to **algernon**, **Guo**, **krzychu124**, **TDW**, and **yenyang**. A big shout-out to them! Aside from the inspiring modders, I am really grateful to beta testers for their precious feedback, including but not limited to **Allegretic**, **LightLight**, and other members in the two Discord servers listed below.

### Feedback and Contacts

You can reach me at the [PDX forum’s](https://forum.paradoxplaza.com/forum/threads/carto.1699089/) comment section (English / Chinese), [Cities: Skylines Modding](https://discord.gg/HTav7ARPs2) Discord server (English only), or [Cities: Skylines Taiwan Assets](https://discord.gg/Gz4K66jT64) Discord server (Chinese preferred). If you need to start a more in-depth discussion or report complicated bugs, I’d suggest using GitHub’s issues or discussions, as it isn’t easy to track them between threads.
