using Carto.Domain;
using Carto.Geodata;
using Colossal.Logging;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.Entities;

namespace Carto.IO
{
    /// <summary>
    /// The class that provides the interface to input / output files.
    /// （提供輸入／輸出檔案介面的類別。）<br/>
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The list of available properties for each system.
        /// （各系統可用的屬性列表。）
        /// </summary>
        public static readonly Dictionary<System, HashSet<Property>> AvailablePropertyTable = new()
        {
            { System.Unknown, new() { } },
            { System.Area, new() { Property.Name, Property.Object, Property.Age, Property.Area, Property.Company, Property.Employee, Property.Household, Property.Labor, Property.Profit, Property.Resident, Property.SexRatio, Property.Unlocked, Property.Wage} },
            { System.Building, new() { Property.Name, Property.Object, Property.Address, Property.Age, Property.Asset, Property.Brand, Property.Category, Property.Elevation, Property.Employee, Property.Height, Property.Household, Property.Labor, Property.Level, Property.Product, Property.Profit, Property.Resident, Property.SexRatio, Property.Storey, Property.Theme, Property.Value, Property.Wage, Property.Zone, Property.Zoning } },
            { System.Network, new() { Property.Name, Property.Object, Property.Asset, Property.Capacity, Property.Category, Property.Direction, Property.Discharge, Property.Elevation, Property.Form, Property.Lane, Property.Length, Property.Limit, Property.Load, Property.Volume, Property.Width } },
            { System.POI, new() { Property.Name, Property.Object, Property.Address, Property.Category} },
            { System.Route, new() { Property.Name, Property.Object, Property.Color, Property.Length, Property.Model, Property.Passenger, Property.Route, Property.Stop, Property.Transport, Property.Usage, Property.Vehicle, Property.Weight} },
            { System.Zoning, new() { Property.Name, Property.Object, Property.Color, Property.Density, Property.Theme, Property.Zoning} }
        };

        /// <summary>
        /// The array sorted by each <see cref="BuildingCategory"/>'s display order.
        /// （根據每個 <see cref="BuildingCategory"/> 顯示順序排序的陣列。）
        /// </summary>
        public static BuildingCategory[] BuildingCategoryDisplayOrder = new BuildingCategory[]
        {
            // Group A: Public facilities（A 組：公共設施）
            BuildingCategory.Transportation, BuildingCategory.Police, BuildingCategory.Fire, BuildingCategory.Health, BuildingCategory.Mortuary,
            BuildingCategory.Education, BuildingCategory.Research, BuildingCategory.Post, BuildingCategory.Communication, BuildingCategory.Disaster,
            BuildingCategory.Waste, BuildingCategory.Power, BuildingCategory.Water, BuildingCategory.Sewage, BuildingCategory.Maintenance, BuildingCategory.Admin,
            BuildingCategory.Park, BuildingCategory.Parking, BuildingCategory.Public,

            // Group B: Private facilities（B 組：私人設施）
            BuildingCategory.Property, BuildingCategory.Extractor,

            // Group C: Other tags（C 組：其他標籤）
            BuildingCategory.Extractor, BuildingCategory.Decoration,

            // Group D: Special industries（D 組：特殊工業）
            BuildingCategory.Landfill, BuildingCategory.OilField, BuildingCategory.Quarry, BuildingCategory.Forestry, BuildingCategory.Ranch,
            BuildingCategory.Farmland, BuildingCategory.Fishery,

            // Group E: Building status（E 組：建築狀態）
            BuildingCategory.Destroyed, BuildingCategory.Condemned, BuildingCategory.Abandoned, BuildingCategory.Construction, BuildingCategory.Extension,

            // Group F: Fallback value（F 組：後備值）
            BuildingCategory.None
        };

        /// <summary>
        /// The properties that are imcompatible with Burst compile.
        /// （與 Burst 編譯不合的屬性集合。）
        /// </summary>
        public static readonly HashSet<Property> BurstImcompatiblePropertyTable = new()
        {
            Property.Asset, Property.Brand, Property.Category, Property.Color, Property.Density, Property.Direction, Property.Form, Property.Model, Property.Name, Property.Object,
            Property.Product, Property.Route, Property.Theme, Property.Transport, Property.Zone, Property.Zoning
        };

        /// <summary>
        /// The look-up table of composite property's sub-field name.
        /// （每個複合屬性的子欄位名稱對照表。）
        /// </summary>
        public static readonly Dictionary<Property, Dictionary<FileFormat, string[]>> CompositePropertyTable = new()
        {
            {
                Property.Address,
                new Dictionary<FileFormat, string[]>
                {
                    { FileFormat.Unknown, new string[3] { "Address_District", "Address_Street", "Address_Number" } },
                    { FileFormat.Shapefile, new string[3] { "Addr_dist", "Addr_strt", "Addr_nmbr" } }
                }
            },
            {
                Property.Resident,
                new Dictionary<FileFormat, string[]>
                {
                    { FileFormat.Unknown, new string[2] { "Resident_Female", "Resident_Male" } },
                    { FileFormat.Shapefile, new string[2] { "Rsdt_fmle", "Rsdt_male" } }
                }
            }
        };

        /// <summary>
        /// The array sorted by each <see cref="Feature"/>'s display order.
        /// （根據每個 <see cref="Feature"/> 顯示順序排序的陣列。）
        /// </summary>
        public static readonly Feature[] FeatureDisplayOrder = new Feature[]
        {
            // Group A: Area（A 組：區域）
            Feature.District, Feature.MapTile,

            // Group B: Networks（B 組：網路）
            Feature.Runway, Feature.Taxiway, Feature.Road, Feature.Track, Feature.Pathway,
            Feature.Waterway, Feature.Cable, Feature.Pipe, Feature.Fence,

            // Group C: Buildings（C 組：建築）
            Feature.Building, Feature.Extractor, Feature.Landfill,

            // Group D: Routes（D 組：路線）
            Feature.RoutePassenger, Feature.RouteCargo,

            // Group E: POIs（E 組：興趣點）
            Feature.POITransport, Feature.POIPublic, Feature.POIPrivate, Feature.POIUtility,

            // Group F: Zonings（F 組：分區）
            Feature.Zoning,

            // Group G: Surfaces（G 組：表面）
            Feature.Surface,

            // Group H: Fallback value（H 組：後備值）
            Feature.None
        };

        /// <summary>
        /// The predefined ellipsoids.
        /// （預先定義的橢球體。）
        /// </summary>
        public static readonly Dictionary<Ellipsoid, EllipsoidDefinition> EllipsoidTable = new()
        {
            { Ellipsoid.Airy30, new(6377563.396d, 299.3249646d) },
            { Ellipsoid.Bssl41, new(6377397.155d, 299.1528128d) },
            { Ellipsoid.Clrk66, new(6378206.4d, 294.978698213898d) },
            { Ellipsoid.Evrst37, new(6377276.345d, 300.8017d) },
            { Ellipsoid.GRS67, new(6378160d, 298.247167427d) },
            { Ellipsoid.GRS80, new(6378137d, 298.257222101d) },
            { Ellipsoid.GSK11, new(6378136.5d, 298.2564151d) },
            { Ellipsoid.IGN80, new(6378249.2d, 293.466021293627d) },
            { Ellipsoid.Intl24, new(6378388d, 297d) },
            { Ellipsoid.Krsky40, new(6378245d, 298.3d) },
            { Ellipsoid.RGS80, new(6378249.145d, 293.465d) },
            { Ellipsoid.WGS84, new(6378137d, 298.257223563d) },
            { Ellipsoid.Custom, new() }
        };

        /// <summary>
        /// The look-up table of each file format's extension.
        /// （每個檔案格式副檔名的對照表。）
        /// </summary>
        public static readonly Dictionary<FileFormat, string> FileExtensionTable = new()
        {
            { FileFormat.GeoJSON, "json" },
            { FileFormat.GeoPackage, "gpkg" },
            { FileFormat.GeoTIFF, "tif" },
            { FileFormat.Shapefile, "shp" }
        };

        /// <summary>
        ///  The array sorted by each <see cref="NetworkCategory"/>'s display order.
        /// （根據每個 <see cref="NetworkCategory"/> 顯示順序排序的陣列。）
        /// </summary>
        public static readonly NetworkCategory[] NetworkCategoryDisplayOrder = new NetworkCategory[]
        {
            // Group A: Road networks（A 組：道路網路）
            NetworkCategory.Highway, NetworkCategory.Large, NetworkCategory.Medium, NetworkCategory.Small, NetworkCategory.Bus,
            NetworkCategory.Bicycle, NetworkCategory.Runway, NetworkCategory.Taxiway,

            // Group B: Track networks（B 組：軌道網路）
            NetworkCategory.Train, NetworkCategory.Subway, NetworkCategory.Tram,

            // Group C: Utility networks（C 組：公用事業網路）
            NetworkCategory.HighCable, NetworkCategory.LowCable, NetworkCategory.WaterPipe, NetworkCategory.SewagePipe, NetworkCategory.StormPipe,

            // Group D: Other networks（D 組：其他網路）
            NetworkCategory.Waterway, NetworkCategory.Pathway, NetworkCategory.Fence, NetworkCategory.RoadBuilder,

            // Group E: Fallback value（E 組：後備值）
            NetworkCategory.None
        };

        /// <summary>
        /// The array sorted by each <see cref="POICategory"/>'s display order.
        /// （根據每個 <see cref="POICategory"/> 顯示順序排序的陣列。）
        /// </summary>
        public static readonly POICategory[] POICategoryDisplayOrder = new POICategory[]
        {
            // Group A: Transportation POIs（A 組：運輸興趣點）
            //  Group A1: Heavy Transportation Buildings（A1 組：重型運輸建築）
            POICategory.BuildingPassengerAirplane, POICategory.BuildingCargoAirplane, POICategory.BuildingPassengerShip, POICategory.BuildingCargoShip,

            //  Group A2: Rail Transportation Buildings（A2 組：軌道運輸建築）
            POICategory.BuildingPassengerTrain, POICategory.BuildingCargoTrain, POICategory.BuildingSubway, POICategory.BuildingTram,

            //  Group A3: Road Transportation Buildings（A3 組：道路運輸建築）
            POICategory.BuildingBus, POICategory.BuildingTaxi, POICategory.BuildingBicycle,

            //  Group A4: Uncommon Transportation Buildings（A4 組：非常見運輸建築）
            POICategory.BuildingFerry, POICategory.BuildingHelicopter, POICategory.SpaceCenter,

            //  Group A5: Depots（A5 組：機廠）
            POICategory.DepotTrain, POICategory.DepotSubway, POICategory.DepotTram, POICategory.DepotBus, POICategory.DepotTaxi, POICategory.DepotFerry, POICategory.DepotGeneric,

            //  Group A6: Heavy Transportation Stops（A6 組：重型運輸站點）
            POICategory.StopPassengerAirplane, POICategory.StopCargoAirplane, POICategory.StopPassengerShip, POICategory.StopCargoShip,

            //  Group A7: Rail Transportation Stops（A7 組：軌道運輸站點）
            POICategory.StopPassengerTrain, POICategory.StopCargoTrain, POICategory.StopSubway, POICategory.StopTram,

            //  Group A8: Road Transportation Stops（A8 組：道路運輸站點）
            POICategory.StopBus, POICategory.StopTaxi, POICategory.BicycleStand,

            //  Group A9: Uncommon Transportation Stops（A9 組：非常見運輸站點）
            POICategory.StopFerry, POICategory.StopHelicopter,

            //  Group A10: Transportation Fallback Value（A10 組：運輸相關後備值）
            POICategory.TransportationGeneric,

            // Group B: Public POIs（B 組：公共設施興趣點）
            //  Group B1: Emergency Services（B1 組：緊急服務）
            POICategory.Fire, POICategory.FireWatchTower, POICategory.Disaster, POICategory.Health, POICategory.MortuaryCemetery, POICategory.MortuaryCrematorium, POICategory.MortuaryGeneric,

            //  Group B2: Administrative Services（B2 組：行政服務）
            POICategory.Police, POICategory.Prison, POICategory.Post, POICategory.Maintenance, POICategory.Admin,

            //  Group B3: Education & Research（B3 組：教育研究）
            POICategory.Research, POICategory.EducationUniversity, POICategory.EducationCollege, POICategory.EducationHigh, POICategory.EducationElementary, POICategory.EducationGeneric,

            //  Group B4: Recreation & Others（B4 組：遊憩與其他）
            POICategory.Attraction, POICategory.Park, POICategory.Parking,

            // Group C: Utility POIs（C 組：公用設施興趣點）
            //  Group C1: Electricity（C1 組：電力）
            POICategory.PowerBattery, POICategory.PowerDam, POICategory.PowerTurbine, POICategory.PowerPlant, POICategory.PowerSubstation, POICategory.PowerGeneric,

            //  Group C2: Water & Sewage（C2 組：自來水與污水）
            POICategory.Water, POICategory.Sewage,

            //  Group C3: Communication（C3 組：通訊）
            POICategory.Communication,

            //  Group C4: Waste & Others（C4 組：廢棄物與其他）
            POICategory.Waste,

            // Group D: Private POIs（D 組：私人興趣點）
            //  Group D1: Offices（D1 組：辦公室）
            POICategory.StoreBank, POICategory.StoreSoftware, POICategory.StoreTelecom, POICategory.StoreMedia, POICategory.StoreOffice,

            //  Group D2: Commercials（D2 組：商業）
            POICategory.StoreHotel, POICategory.StoreGasStation,
            POICategory.StoreRestaurant, POICategory.StoreBar, POICategory.StoreBeverage, POICategory.StoreFood, POICategory.StoreConvenienceStore,
            POICategory.StoreBookStore, POICategory.StoreCarStore, POICategory.StoreChemicals, POICategory.StoreDrugStore, POICategory.StoreElectronics,
            POICategory.StoreFashionStore, POICategory.StoreFurniture, POICategory.StorePlastics, POICategory.StoreRecreation, POICategory.StoreGeneric,

            //  Group D3: Industrials（D3 組：工業）
            POICategory.IndustrialCoal, POICategory.IndustrialCotton, POICategory.IndustrialGrain, POICategory.IndustrialLivestock, POICategory.IndustrialFish,
            POICategory.IndustrialOil, POICategory.IndustrialOre, POICategory.IndustrialStone, POICategory.IndustrialVegetables, POICategory.IndustrialWood,
            POICategory.IndustrialWarehouse, POICategory.IndustrialFactory, POICategory.IndustrialGeneric,

            // Group E: Objects（E 組：物件）
            POICategory.TrafficLight, POICategory.LevelCrossing, POICategory.PostBox, POICategory.UtilityPylon, POICategory.UtilityPole, POICategory.Helipad,

            // Group F: Fallback value（F 組：後備值）
            POICategory.None
        };

        public static readonly Dictionary<Feature, HashSet<POICategory>> POICategoryFeatureTable = new()
        {
            { Feature.POIPrivate, new()
                {
                    POICategory.StoreBank, POICategory.StoreSoftware, POICategory.StoreTelecom, POICategory.StoreMedia, POICategory.StoreOffice,
                    POICategory.StoreHotel, POICategory.StoreGasStation,
                    POICategory.StoreRestaurant, POICategory.StoreBar, POICategory.StoreBeverage, POICategory.StoreFood, POICategory.StoreConvenienceStore,
                    POICategory.StoreBookStore, POICategory.StoreCarStore, POICategory.StoreChemicals, POICategory.StoreDrugStore, POICategory.StoreElectronics,
                    POICategory.StoreFashionStore, POICategory.StoreFurniture, POICategory.StorePlastics, POICategory.StoreRecreation, POICategory.StoreGeneric,
                    POICategory.IndustrialCoal, POICategory.IndustrialCotton, POICategory.IndustrialGrain, POICategory.IndustrialLivestock, POICategory.IndustrialFish,
                    POICategory.IndustrialOil, POICategory.IndustrialOre, POICategory.IndustrialStone, POICategory.IndustrialVegetables, POICategory.IndustrialWood,
                    POICategory.IndustrialWarehouse, POICategory.IndustrialFactory, POICategory.IndustrialGeneric
                }
            },
            { Feature.POIPublic, new()
                {
                    POICategory.Fire, POICategory.FireWatchTower, POICategory.Disaster, POICategory.Health, POICategory.MortuaryCemetery, POICategory.MortuaryCrematorium, POICategory.MortuaryGeneric,
                    POICategory.Police, POICategory.Prison, POICategory.Post, POICategory.Maintenance, POICategory.Admin,
                    POICategory.Research, POICategory.EducationUniversity, POICategory.EducationCollege, POICategory.EducationHigh, POICategory.EducationElementary, POICategory.EducationGeneric,
                    POICategory.Attraction, POICategory.Park, POICategory.Parking
                }
            },
            { Feature.POITransport, new()
                {
                    POICategory.BuildingPassengerAirplane, POICategory.BuildingCargoAirplane, POICategory.BuildingPassengerShip, POICategory.BuildingCargoShip,
                    POICategory.BuildingPassengerTrain, POICategory.BuildingCargoTrain, POICategory.BuildingSubway, POICategory.BuildingTram,
                    POICategory.BuildingBus, POICategory.BuildingTaxi, POICategory.BuildingBicycle,
                    POICategory.BuildingHelicopter, POICategory.SpaceCenter,
                    POICategory.DepotTrain, POICategory.DepotSubway, POICategory.DepotTram, POICategory.DepotBus, POICategory.DepotTaxi, POICategory.DepotGeneric,
                    POICategory.StopPassengerAirplane, POICategory.StopCargoAirplane, POICategory.StopPassengerShip, POICategory.StopCargoShip,
                    POICategory.StopPassengerTrain, POICategory.StopCargoTrain, POICategory.StopSubway, POICategory.StopTram,
                    POICategory.StopBus, POICategory.StopTaxi, POICategory.BicycleStand,
                    POICategory.StopHelicopter,
                    POICategory.TrafficLight, POICategory.LevelCrossing, POICategory.PostBox, POICategory.Helipad,
                    POICategory.TransportationGeneric
                }
            },
            { Feature.POIUtility, new()
                {
                    POICategory.PowerBattery, POICategory.PowerDam, POICategory.PowerTurbine, POICategory.PowerPlant, POICategory.PowerSubstation, POICategory.PowerGeneric,
                    POICategory.Water, POICategory.Sewage,
                    POICategory.Communication,
                    POICategory.Waste,
                    POICategory.UtilityPylon, POICategory.UtilityPole
                }
            }
        };

        /// <summary>
        /// The number of Carto's property.
        /// （Carto 的屬性數量。）
        /// </summary>
        public static int PropertyCount => Utils.CommonUtils.GetNamedFlagsCount<Property>() - 1; 

        /// <summary>
        /// The look-up table of each <see cref="Property"/>'s corresponding type.<br/>
        /// （每個 <see cref="Property"/> 的對應型別表。）
        /// </summary>
        public static readonly Dictionary<Property, Type> PropertyTypeTable = new()
        {
            { Property.Address, typeof(object[]) },
            { Property.Age, typeof(float) },
            { Property.Area, typeof(float) },
            { Property.Asset, typeof(string) },
            { Property.Brand, typeof(string) },
            { Property.Capacity, typeof(float) },
            { Property.Category, typeof(string) },
            { Property.Color, typeof(string) },
            { Property.Company, typeof(int) },
            { Property.Density, typeof(string) },
            { Property.Direction, typeof(string) },
            { Property.Discharge, typeof(float) },
            { Property.Elevation, typeof(float) },
            { Property.Employee, typeof(int) },
            { Property.Form, typeof(string) },
            { Property.Height, typeof(float) },
            { Property.Household, typeof(int) },
            { Property.Labor, typeof(int) },
            { Property.Lane, typeof(int) },
            { Property.Length, typeof(float) },
            { Property.Level, typeof(int) },
            { Property.Limit, typeof(float) },
            { Property.Load, typeof(float) },
            { Property.Model, typeof(string) },
            { Property.Name, typeof(string) },
            { Property.Object, typeof(string) },
            { Property.Passenger, typeof(int) },
            { Property.Product, typeof(string) },
            { Property.Profit, typeof(float) },
            { Property.Resident, typeof(int) },
            { Property.Route, typeof(string) },
            { Property.SexRatio, typeof(float) },
            { Property.Stop, typeof(int) },
            { Property.Storey, typeof(int) },
            { Property.Theme, typeof(string) },
            { Property.Transport, typeof(string) },
            { Property.Unlocked, typeof(bool) },
            { Property.Usage, typeof(float) },
            { Property.Value, typeof(float) },
            { Property.Vehicle, typeof(int) },
            { Property.Volume, typeof(float) },
            { Property.Wage, typeof(float) },
            { Property.Weight, typeof(int) },
            { Property.Width, typeof(float) },
            { Property.Zone, typeof(string) },
            { Property.Zoning, typeof(string) }
        };

        /// <summary>
        /// The table between the property and the custom maximum decimal length.
        /// （屬性與客製化的最長小數點位數對照表。）
        /// </summary>
        public static readonly Dictionary<Property, int> PropertyDecimalConstraintTable = new()
        {
            { Property.Age, 1 },
            { Property.Area, 2 },
            { Property.Discharge, 2 },
            { Property.Elevation, 4 },
            { Property.Height, 4 },
            { Property.Length, 4 },
            { Property.Limit, 4 },
            { Property.Load, 2 },
            { Property.Profit, 2 },
            { Property.SexRatio, 4 },
            { Property.Usage, 4 },
            { Property.Value, 2 },
            { Property.Volume, 2 },
            { Property.Wage, 2 },
            { Property.Width, 4 }
        };

        /// <summary>
        /// The array sorted by each <see cref="ZoningCategory"/>'s display order.
        /// （根據每個 <see cref="ZoningCategory"/> 顯示順序排序的陣列。）
        /// </summary>
        public static readonly ZoningCategory[] ZoningDisplayOrder = new ZoningCategory[]
        {
            ZoningCategory.Commercial, ZoningCategory.Office, ZoningCategory.Industrial, ZoningCategory.Residential, ZoningCategory.None
        };

        public static void OnReport(string file, int progress)
        {

        }

        /// <summary>
        /// Dispose of all native containers across all systems.
        /// （拋棄各系統的原生容器。）
        /// </summary>
        public static void DisposeAll()
        {
            Instance.Building.Dispose();
            Instance.Network.Dispose();
            Instance.POI.Dispose();
            Instance.Route.Dispose();
            Instance.Shared.Dispose();
        }

        /// <summary>
        /// Export in-game objects into geospatial files.
        /// （將遊戲內物體輸出為地理空間格式檔案。）
        /// </summary>
        public static void Export()
        {
            // Export options.（輸出設定。）
            Options options = Instance.Settings.GetOptions();
            options.Initialize();

            // Check user inputs.（檢查使用者輸入。）
            List<string> errorMessages = new();
            foreach (KeyValuePair<string, Error> error in options.Errors)
            {
                string errorSource = Utils.LocaleUtils.Translate(error.Key);
                switch (error.Value)
                {
                    case Error.Latitude:
                        errorMessages.Add(Utils.LocaleUtils.Translate("Carto.Common.ERROR[Latitude]").Replace("{INPUT}", errorSource));
                        break;

                    case Error.Longitude:
                        errorMessages.Add(Utils.LocaleUtils.Translate("Carto.Common.ERROR[Longitude]").Replace("{INPUT}", errorSource));
                        break;

                    case Error.Nan:
                        errorMessages.Add(Utils.LocaleUtils.Translate("Carto.Common.ERROR[Nan]").Replace("{INPUT}", errorSource));
                        break;

                    case Error.Negative:
                        errorMessages.Add(Utils.LocaleUtils.Translate("Carto.Common.ERROR[Negative]").Replace("{INPUT}", errorSource));
                        break;

                    case Error.Transform:
                        errorMessages.Add(Utils.LocaleUtils.Translate("Carto.Common.ERROR[Transform]").Replace("{INPUT}", errorSource));
                        break;

                    case Error.TransformLength:
                        errorMessages.Add(Utils.LocaleUtils.Translate("Carto.Common.ERROR[TransformLength]").Replace("{INPUT}", errorSource));
                        break;

                    case Error.UTMZone:
                        errorMessages.Add(Utils.LocaleUtils.Translate("Carto.Common.ERROR[UTMZone]").Replace("{INPUT}", errorSource));
                        break;
                    
                    default:
                        break;
                }
            }

            if (errorMessages.Count > 0)
            {
                string message = string.Join("\n", errorMessages);
                ErrorDialog dialog = new() { severity = ErrorDialog.Severity.Warning, localizedTitle = "Common.WARNING", localizedMessage = "Carto.Common.ERROR[Input]", errorDetails = message, actions = ErrorDialog.ActionBits.Continue };
                Instance.UI.appBindings.ShowErrorDialog(dialog);
                return;
            }

            // Check sharing violation.（檢查存取問題。）
            List<string> lockedFiles = Utils.IOUtils.GetLockedFiles(options);
            if (lockedFiles.Count > 0)
            {
                string message = string.Join("\n", lockedFiles);
                ErrorDialog dialog = new() { severity = ErrorDialog.Severity.Warning, localizedTitle = "Common.WARNING", localizedMessage = "Carto.Common.ERROR[ShareViolation]", errorDetails = message, actions = ErrorDialog.ActionBits.Continue };
                Instance.UI.appBindings.ShowErrorDialog(dialog);
                return;
            }

#if RELEASE
            LogExportOptions(options);
#endif

            int filesCount = 0;

            // The ignored files collection.（忽略檔案集合。）
            Dictionary<string, Error> ignores = new();

            try
            {
                RunExportPipeline(options, ignores, ref filesCount);
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                DisposeAll();
            }
            finally
            {
                PlayCompletionEffects(options, filesCount, ignores);

                // In case of any missing native container disposal during the execution, try to dispose them after all execution are completed.
                // （為避免執行中遺漏任何拋棄原生容器的程序，當所有程式執行完成後，嘗試拋棄這些容器。）.
                DisposeAll();
            }
        }

        /// <summary>
        /// Peer-mod entry point: export driven by a caller-constructed <see cref="Options"/>.
        /// （對等模組入口點：依據呼叫端建立的 <see cref="Options"/> 執行輸出。）<br/>
        /// Does not read <see cref="Instance.Settings"/>. Errors are returned via
        /// <see cref="ExportResult.ErrorMessage"/> rather than <c>ErrorDialog</c>;
        /// completion sound and dialog still honor the caller's
        /// <see cref="Options.CompletionSound"/> / <see cref="Options.CompletionDialog"/>.
        /// Synchronous: when this returns, all files have been written and all
        /// native containers disposed.
        /// （不讀取 <see cref="Instance.Settings"/>。錯誤透過
        /// <see cref="ExportResult.ErrorMessage"/> 回報而非 <c>ErrorDialog</c>；
        /// 完成音效與對話框仍依呼叫端的 <see cref="Options.CompletionSound"/>
        /// 與 <see cref="Options.CompletionDialog"/> 設定執行。
        /// 同步執行：方法回傳時所有檔案皆已寫入、所有原生容器皆已拋棄。）
        /// </summary>
        /// <param name="options">A fully-configured <see cref="Options"/> instance — the caller is responsible for the projection chain (<see cref="Options.SourceCoordinates"/>, <see cref="Options.SourceProjection"/>, <see cref="Options.SourceProjectionDefinition"/>, <see cref="Options.TargetEllipsoid"/>, <see cref="Options.TargetProjection"/>, <see cref="Options.TargetProjectionDefinition"/>) and content selection (<see cref="Options.Systems"/>, <see cref="Options.Features"/>, <see cref="Options.Properties"/>, <see cref="Options.VectorKinds"/>, <see cref="Options.RasterKinds"/>, <see cref="Options.Display"/>).（完整設定的 <see cref="Options"/>，呼叫端須自行配置投影鏈與內容選擇。）</param>
        /// <returns>An <see cref="ExportResult"/> describing outcome.（描述輸出結果的 <see cref="ExportResult"/>。）</returns>
        public static ExportResult Export(Options options)
        {
            if (options == null)
                return new ExportResult { Success = false, ErrorMessage = "options is null" };
            if (string.IsNullOrWhiteSpace(options.CustomDirectory))
                return new ExportResult { Success = false, ErrorMessage = "options.CustomDirectory is required" };

            try
            {
                Directory.CreateDirectory(options.CustomDirectory);
                options.Initialize();
            }
            catch (Exception ex)
            {
                return new ExportResult { Success = false, ErrorMessage = ex.Message };
            }

            // Locked-file check. Same logic as the button path, but reported via
            // ErrorMessage instead of ErrorDialog.
            // （存取問題檢查。與按鈕路徑相同的邏輯，但透過 ErrorMessage 回報而非 ErrorDialog。）
            List<string> lockedFiles = Utils.IOUtils.GetLockedFiles(options);
            if (lockedFiles.Count > 0)
                return new ExportResult { Success = false, ErrorMessage = $"Locked files: {string.Join(", ", lockedFiles)}" };

            // Snapshot before so the result reports exactly what this call wrote.
            // （事前快照，以便結果能精準回報此呼叫寫入的檔案。）
            string[] before = SnapshotFiles(options.CustomDirectory);

            int filesCount = 0;
            Dictionary<string, Error> ignores = new();
            string errorMessage = null;

            try
            {
                RunExportPipeline(options, ignores, ref filesCount);
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                DisposeAll();
                errorMessage = ex.Message;
            }
            finally
            {
                // Honor caller's CompletionSound / CompletionDialog flags.
                // （遵守呼叫端的 CompletionSound 與 CompletionDialog 設定。）
                PlayCompletionEffects(options, filesCount, ignores);
                DisposeAll();
            }

            string[] after = SnapshotFiles(options.CustomDirectory);
            HashSet<string> beforeSet = new(before);
            string[] written = after.Where(f => !beforeSet.Contains(f)).ToArray();

            return new ExportResult
            {
                Success = errorMessage == null,
                FilesWritten = written,
                ErrorMessage = errorMessage
            };
        }

        /// <summary>
        /// Recursive file snapshot for diffing what the pipeline wrote.
        /// （遞迴掃描檔案以比對流程寫入的差異。）
        /// </summary>
        private static string[] SnapshotFiles(string dir) =>
            Directory.Exists(dir)
                ? Directory.GetFiles(dir, "*", SearchOption.AllDirectories)
                : new string[0];

        /// <summary>
        /// Fire any caller-opted-in completion side effects (sound, dialog).
        /// （依呼叫端設定觸發完成副作用（音效、對話框）。）<br/>
        /// Shared by both <see cref="Export()"/> and <see cref="Export(Options)"/>;
        /// each flag is evaluated independently against <paramref name="options"/>.
        /// （由 <see cref="Export()"/> 與 <see cref="Export(Options)"/> 共用；
        /// 各旗標獨立依 <paramref name="options"/> 評估。）
        /// </summary>
        private static void PlayCompletionEffects(Options options, int filesCount, Dictionary<string, Error> ignores)
        {
            if (options.CompletionSound) Instance.Sound.Play(Sound.Completion);

            if (options.CompletionDialog)
            {
                string message = Utils.LocaleUtils.Translate("Carto.Common.SUCCESS[Normal]").Replace("{NUMBER}", filesCount.ToString());
                if (ignores.Count > 0)
                {
                    foreach (KeyValuePair<Error, int> kvp in ignores.GroupBy(pair => pair.Value).ToDictionary(group => group.Key, group => group.Count()))
                    {
                        string reason = Utils.LocaleUtils.Translate($"Carto.Common.IGNORE[{kvp.Key}]");
                        message = $"{message}\n{Utils.LocaleUtils.Translate("Carto.Common.SUCCESS[Ignore]").Replace("{NUMBER}", kvp.Value.ToString())}".Replace("{REASON}", reason);
                    }
                }
                MessageDialog dialog = new("Options.SECTION[Carto.Carto.Mod]", message, "Common.OK");
                Instance.UI.appBindings.ShowMessageDialog(dialog, null);
            }
        }

        /// <summary>
        /// Run the export pipeline against a prepared <see cref="Options"/> instance.
        /// （依據已準備好的 <see cref="Options"/> 執行輸出流程。）<br/>
        /// Shared by both <see cref="Export()"/> and <see cref="Export(Options)"/> —
        /// no UI side effects, no settings access. Caller is responsible for the
        /// surrounding try/catch/finally (dispose, sound, dialog).
        /// （由 <see cref="Export()"/> 與 <see cref="Export(Options)"/> 共用 —
        /// 無 UI 副作用、不存取設定。呼叫端負責外層的 try/catch/finally
        /// （拋棄、音效、對話框）。）
        /// </summary>
        /// <param name="options">The prepared export options.（已準備好的輸出設定。）</param>
        /// <param name="ignores">Sink for raster ignores keyed by label, valued by reason.（收集網格略過項目的字典：鍵為標籤，值為原因。）</param>
        /// <param name="filesCount">Running count of files written; incremented in place so partial progress survives an exception.（累計寫入的檔案數，以參考方式遞增，例外時保留部分進度。）</param>
        private static void RunExportPipeline(Options options, Dictionary<string, Error> ignores, ref int filesCount)
        {
            // Shorthanded variables to determine whther to run any system.（縮寫變數，用於決定是否執行任何系統。）
            bool useArea = options.Systems.HasFlag(System.Area);
            bool useBuilding = options.Systems.HasFlag(System.Building);
            bool useNetwork = options.Systems.HasFlag(System.Network);
            bool usePOI = options.Systems.HasFlag (System.POI);
            bool useRaster = options.Systems.HasFlag(System.Raster);
            bool useRoute = options.Systems.HasFlag(System.Route);
            bool useZoning = options.Systems.HasFlag(System.Zoning);
            bool useVector = useArea || useBuilding || useNetwork || usePOI || useRoute || useZoning;

            if (useVector)
            {
                /*
                 *  The dependency graph:（依賴性關係圖：）
                 *
                 *  SharedDataCollectionSystem Exposed Property           Systems
                 *  ....................................................................
                 *  Themes ──── ZoningTypes  ┬─────────── ZoningSystem
                 *                               └┐                  ┌ AreaSystem
                 *  Brands ────────────┴─ BuildingStats  ┼ BuildingSystem
                 *                                                     └ POISystem
                 *                                                        NetworkSystem
                 *                                                        RouteSystem
                 *
                 *  The actual requirements and execution order:（實際需求與執行順序：)
                 *
                 *  1. ZoningSystem   - Themes, ZoningTypes, ZoningTypesEntityMap, ZoningTypesNames
                 *  2. POISystem      - Brands, BuildingStats, ZoningTypes
                 *  3. BuildingSystem - Brands, BuildingStats, Themes, ZoningTypes, ZoningTypesEntityMap, ZoningTypesNames
                 *  4. AreaSystem     - BuildingStats
                 *  5. NetworkSystem  - (No dependency)
                 *  6. RouteSystem    - (No dependency)
                 */

                // Collect vector shared data.（收集向量共享資料。）
                if (useArea || useBuilding || usePOI)
                {
                    // Retrieve building statistics.（獲取建築的統計資料。）
                    Instance.Shared.GetBuildingStats(options);
                }
                else
                {
                    if (useZoning)
                    {
                        // Retrieve zoning types information.（獲取分區類別的資訊。）
                        Instance.Shared.GetZoningTypes(options);
                    }
                }

                bool areaHasBoundary = options.Has(System.Area, VectorKind.Boundary);
                bool buildingHasBoundary = options.Has(System.Building, VectorKind.Boundary);
                bool networkHasBoundary = options.Has(System.Network, VectorKind.Boundary);
                bool networkHasCenterline = options.Has(System.Network, VectorKind.Centerline);
                bool poiHasLocation = options.Has(System.POI, VectorKind.Location);
                bool routeHasCenterline = options.Has(System.Route, VectorKind.Centerline);
                bool zoningHasBoundary = options.Has(System.Zoning, VectorKind.Boundary);

                // Write vector data.（寫入向量資料。）
                switch (options.VectorFormat)
                {
                    case FileFormat.GeoJSON:
                        if (useZoning)
                        {
                            if (zoningHasBoundary)
                            {
                                GeoJson.Write(options, System.Zoning, VectorKind.Boundary, Instance.Zoning.WriteBoundaryFeatures, OnReport);
                                filesCount++;
                            }
                        }
                        if (usePOI)
                        {
                            if (poiHasLocation)
                            {
                                GeoJson.Write(options, System.POI, VectorKind.Location, Instance.POI.WriteLocationFeatures, OnReport);
                                filesCount++;
                            }
                        }
                        if (useBuilding)
                        {
                            if (buildingHasBoundary)
                            {
                                GeoJson.Write(options, System.Building, VectorKind.Boundary, Instance.Building.WriteBoundaryFeatures, OnReport);
                                filesCount++;
                            }
                        }
                        if (useArea)
                        {
                            if (areaHasBoundary)
                            {
                                GeoJson.Write(options, System.Area, VectorKind.Boundary, Instance.Area.WriteBoundaryFeatures, OnReport);
                                filesCount++;
                            }
                        }
                        if (useNetwork)
                        {
                            if (networkHasBoundary || networkHasCenterline)
                            {
                                Instance.Network.WriteFeatures(options, OnReport, out int networkFilesCount);
                                filesCount += networkFilesCount;
                            }
                        }
                        if (useRoute)
                        {
                            if (routeHasCenterline)
                            {
                                GeoJson.Write(options, System.Route, VectorKind.Centerline, Instance.Route.WriteCenterlineFeatures, OnReport);
                                filesCount++;
                            }
                        }
                        break;

                    case FileFormat.Shapefile:
                        if (useZoning)
                        {
                            if (zoningHasBoundary)
                            {
                                Shapefile.Write<ZoningCell>(options, System.Zoning, VectorKind.Boundary, Instance.Zoning.WriteBoundarySHP, Instance.Zoning.WriteBoundaryDBF, OnReport);
                                filesCount += 5;
                            }
                        }
                        if (usePOI)
                        {
                            if (poiHasLocation)
                            {
                                Shapefile.Write<Entity>(options, System.POI, VectorKind.Location, Instance.POI.WriteLocationSHP, Instance.POI.WriteLocationDBF, OnReport);
                                filesCount += 5;
                            }
                        }
                        if (useBuilding)
                        {
                            if (buildingHasBoundary)
                            {
                                Shapefile.Write<Entity>(options, System.Building, VectorKind.Boundary, Instance.Building.WriteBoundarySHP, Instance.Building.WriteBoundaryDBF, OnReport);
                                filesCount += 5;
                            }
                        }
                        if (useArea)
                        {
                            if (areaHasBoundary)
                            {
                                Shapefile.Write<Entity>(options, System.Area, VectorKind.Boundary, Instance.Area.WriteBoundarySHP, Instance.Area.WriteBoundaryDBF, OnReport);
                                filesCount += 5;
                            }
                        }
                        if (useNetwork)
                        {
                            if (networkHasBoundary || networkHasCenterline)
                            {
                                Instance.Network.WriteShapefiles(options, OnReport, out int networkFilesCount);
                                filesCount += networkFilesCount;
                            }
                        }
                        if (useRoute)
                        {
                            if (routeHasCenterline)
                            {
                                Shapefile.Write<Entity>(options, System.Route, VectorKind.Centerline, Instance.Route.WriteCenterlineSHP, Instance.Route.WriteCenterlineDBF, OnReport);
                                filesCount += 5;
                            }
                        }
                        break;
                }
            }

            if (useRaster)
            {
                bool hasWorldDepth = options.RasterKinds.HasFlag(RasterKind.WorldDepth);
                bool hasWorldElevation = options.RasterKinds.HasFlag(RasterKind.WorldElevation);
                bool hasWorldTerrain = hasWorldDepth || hasWorldElevation;

                // Write raster data.（寫入網格資料。）
                switch (options.RasterFormat)
                {
                    case FileFormat.GeoTIFF:
                        // Handle the grids using shared data first.（首先處理使用共享資料的網格。）
                        if (hasWorldTerrain)
                        {
                            Instance.Shared.GetWorldElevation(options, out Error worldHeightmapError);
                            bool worldHeightmapIntegrity = worldHeightmapError == Error.None;

                            if (worldHeightmapIntegrity)
                            {
                                if (hasWorldDepth)
                                {
                                    GeoTiff.Write(options, RasterKind.WorldDepth, Instance.Raster.WriteWorldDepth, OnReport);
                                    filesCount++;
                                }
                                if (hasWorldElevation)
                                {
                                    GeoTiff.Write(options, RasterKind.WorldElevation, Instance.Raster.WriteWorldElevation, OnReport);
                                    filesCount++;
                                }
                            }
                            else
                            {
                                if (hasWorldDepth) ignores.Add(Instance.Settings.GetOptionLabelLocaleID(Settings.GeometryWorldDepth), worldHeightmapError);
                                if (hasWorldElevation) ignores.Add(Instance.Settings.GetOptionLabelLocaleID(Settings.GeometryWorldElevation), worldHeightmapError);
                            }

                            Instance.Shared.Dispose(DisposePhase.AfterTerrainRelated);
                        }

                        // ... then handle the grids using data independent from others later.（接著處理獨立的網格。）
                        if (options.RasterKinds.HasFlag(RasterKind.Depth))
                        {
                            GeoTiff.Write(options, RasterKind.Depth, Instance.Raster.WriteDepth, OnReport);
                            filesCount++;
                        }
                        if (options.RasterKinds.HasFlag(RasterKind.Elevation))
                        {
                            GeoTiff.Write(options, RasterKind.Elevation, Instance.Raster.WriteElevation, OnReport);
                            filesCount++;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Retrieve the given <see cref="Property"/>'s type.
        /// （獲得給定 <see cref="Property"/> 的型別。）
        /// </summary>
        /// <param name="property">The property enumeration.（欄位枚舉。）</param>
        /// <param name="propertyName">The title of the field.（欄位的名稱。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <returns>The expected type of the <see cref="Property"/>.（<see cref="Property"/> 的預期型別。）</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Type GetPropertyType(Property property, string propertyName, Options options = null)
        {
            if (options != null)
            {
                if ((property == Property.Resident) && options.SeparateResident)
                {
                    return typeof(int[]);
                }
            }

            if (!PropertyTypeTable.TryGetValue(property, out Type expectedType))
            {
                throw new ArgumentException($"Unknown property `{propertyName}`. 未知的屬性 `{propertyName}`。");
            }

            return expectedType;
        }

        /// <summary>
        /// Check whether the property is a composite property or not.
        /// （確認屬性是否是一個複合屬性。）
        /// </summary>
        /// <param name="property">The property enumeration.（欄位枚舉。）</param>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <returns>True if the property is a composite property.（若屬性是複合屬性，回傳真值。）</returns>
        public static bool IsCompositeProperty(Property property, Options options = null) => GetPropertyType(property, property.ToString(), options).IsArray;

        /// <summary>
        /// Log the export options.
        /// （記錄輸出設定。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        private static void LogExportOptions(Options options)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            TextInfo text = culture.TextInfo;
            string numericAlternativeFormat = "0.###########";
            string numericFormat = "0.0##########";

            // Helper functions.
            // （輔助函式。）
            static int CompareVersion(Version provided, string baseVersion)
            {
                if (provided == null) throw new ArgumentNullException(nameof(provided));
                if (string.IsNullOrWhiteSpace(baseVersion)) throw new ArgumentException("Base version is empty. 基本版本為空值。");
                if (!Version.TryParse(baseVersion, out var version)) throw new ArgumentException("Invalid base version. 無效的基本版本。");
                return provided.CompareTo(version);
            }

            static string GetAlignedText(string left, string right, int width)
            {
                // Assumes left.Length + right.Length < width - 1
                //（假設 left.Length + right.Length < width - 1）
                return $"{left}{new string(' ', width - left.Length - right.Length)}{right}";
            }

            static string GetFeatureText(Feature condition) => condition != 0 ? condition.ToString("G") : "(omitted)";

            static string GetIndentedText(string input, int indentation) => $"{new string(' ', indentation)}{input}";

            string GetPropertyText(System system)
            {
                if (options.Properties.TryGetValue(system, out HashSet<Property> property))
                {
                    if (property.Count == 0)
                    {
                        return "(omitted)";
                    }
                    else
                    {
                        return string.Join(", ", property.ToArray());
                    }
                }

                return "(omitted)";
            }

            static string GetRasterKindText(RasterKind condition) => condition != 0 ? condition.ToString("G") : "(omitted)";

            string GetTransformText(HelmertTransform transform)
            {
                switch (transform.paramCount)
                {
                    case 3:
                        StringBuilder sbA = new();
                        for (int i = 0; i < 3; i++)
                        {
                            sbA.Append(transform[i].ToString(numericAlternativeFormat, culture));
                            sbA.Append(",");
                        }
                        return $"{sbA}0,0,0,0";

                    case 7:
                        StringBuilder sbB = new();
                        for (int i = 0; i < 7; i++)
                        {
                            sbB.Append(transform[i].ToString(numericAlternativeFormat, culture));
                            if (i < 6) sbB.Append(",");
                        }
                        return sbB.ToString();

                    default:
                        return "0,0,0,0,0,0,0";
                }
            }

            string GetVectorKindText(System system)
            {
                if (options.VectorKinds.TryGetValue(system, out VectorKind vectorKind) && (vectorKind != VectorKind.Unknown))
                {
                    return vectorKind.ToString("G");
                }

                return "(omitted)";
            }

            static string GetVersionIndicator(int result)
            {
                return result switch
                {
                    > 0 => "^ ",
                    0 => "  ",
                    < 0 => "* ",
                };
            }

            static void PrintEmptyLine()
            {
                _log.Info(string.Empty);
            }

            static void PrintDoubleLine()
            {
                _log.Info(new string('=', 38));
            }

            void PrintH1(string heading)
            {
                _log.Info(text.ToUpper(heading));
                _log.Info(new string('‾', 38));
            }

            void PrintH2(string heading)
            {
                _log.Info($"  {text.ToUpper(heading)}");
                _log.Info($"  {new string('.', 36)}");
            }

            // Start printing logs.
            // （開始記錄。）
            string directory = GetIndentedText(GetAlignedText("DIRECTORY", options.CustomDirectory == Instance.CartoDataPath ? "Default" : "Custom", 36), 2);
            if (options.CustomDirectory != Instance.CartoDataPath) directory = $"{directory} {options.CustomDirectory}";
            string namingFormat = GetIndentedText(GetAlignedText("NAMING_FORMAT", options.FileNameFormat.ToString("G"), 36), 2);
            if (options.FileNameFormat == NamingFormat.Custom) namingFormat = $"{namingFormat} {options.FileName}";
            string transformText = GetTransformText(options.TargetProjectionDefinition.transform);
            bool useArea = (options.Systems & System.Area) != 0;
            bool useBuilding = (options.Systems & System.Building) != 0;
            bool useNetwork = (options.Systems & System.Network) != 0;
            bool usePOI = (options.Systems & System.POI) != 0;
            bool useRoute = (options.Systems & System.Route) != 0;
            bool useZoning = (options.Systems & System.Zoning) != 0;
            Feature areaFeatures = options.Features & (Feature.District | Feature.MapTile | Feature.Surface);
            Feature buildingFeatures = options.Features & (Feature.Building | Feature.Extractor | Feature.Landfill);
            Feature networkFeatures = options.Features & (Feature.Cable | Feature.Pathway | Feature.Pipe | Feature.Road | Feature.Runway | Feature.Taxiway | Feature.Track | Feature.Waterway);
            Feature poiFeatures = options.Features & (Feature.POIPrivate | Feature.POIPublic | Feature.POITransport | Feature.POIUtility);
            Feature routeFeatures = options.Features & (Feature.RouteCargo | Feature.RoutePassenger);
            Feature zoningFeatures = options.Features & Feature.Zoning;
            RasterKind terrainRasters = options.RasterKinds & (RasterKind.Elevation | RasterKind.WorldElevation);
            RasterKind waterRasters = options.RasterKinds & (RasterKind.Depth | RasterKind.WorldDepth);
            bool buildingDisplay = options.Display.TryGetValue((Property.Category, System.Building), out bool bd) && bd;
            bool networkDisplay = options.Display.TryGetValue((Property.Category, System.Network), out bool nd) && nd;
            bool poiDisplay = options.Display.TryGetValue((Property.Category, System.POI), out bool pd) && pd;
            bool zoningDisplay = options.Display.TryGetValue((Property.Zoning, System.Unknown), out bool zd) && zd;

            PrintDoubleLine();
            _log.Info(GetAlignedText("Export Settings", Instance.Version, 38));
            PrintEmptyLine();

            PrintH1("ASSEMBLIES");

            foreach(Game.Modding.ModManager.ModInfo mod in Instance.Mod)
            {
                Assembly assembly = mod.asset.assembly;
                if (assembly == null) continue;
                string name = assembly.GetName().Name;
                Version version = assembly.GetName().Version;
                string assemblyItem;
                string assemblyBaseTitle = $"{name} [{version}]";

                if (name.StartsWith(Instance.Rb.Name))
                {
                    assemblyItem = $"{GetVersionIndicator(CompareVersion(version, Instance.Rb.VerifiedVersion))}{assemblyBaseTitle}";
                }
                else if (name.StartsWith(Instance.Xtm.Name))
                {
                    assemblyItem = $"{GetVersionIndicator(CompareVersion(version, Instance.Xtm.VerifiedVersion))}{assemblyBaseTitle}";
                }
                else if (name.StartsWith(Instance.Zcc.Name))
                {
                    assemblyItem = $"{GetVersionIndicator(CompareVersion(version, Instance.Zcc.VerifiedVersion))}{assemblyBaseTitle}";
                }
                else
                {
                    assemblyItem = $"  {assemblyBaseTitle}";
                }

                _log.Info(GetIndentedText(assemblyItem, 2));
            }

            PrintEmptyLine();
            _log.Info(GetIndentedText("NOTES", 2));
            _log.Info(GetIndentedText("^ = Higher than verified version", 2));
            _log.Info(GetIndentedText("* = Lower than verified version", 2));
            PrintEmptyLine();

            PrintH1("SAVES");
            _log.Info(GetIndentedText(GetAlignedText("STAGE", Instance.GameMode.ToString("G"), 36), 2));
            _log.Info(GetIndentedText(GetAlignedText("TIMESTAMP", options.Created.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", culture), 36), 2));
            PrintEmptyLine();

            PrintH1("GENERAL");
            _log.Info(directory);
            _log.Info(namingFormat);
            _log.Info(GetIndentedText(GetAlignedText("VECTOR_FORMAT", options.VectorFormat.ToString("G"), 36), 2));
            _log.Info(GetIndentedText(GetAlignedText("TIFF_FORMAT", options.GeoTiffFormat.ToString("G"), 36), 2));
            PrintEmptyLine();

            PrintH1("FEATURES & PROPERTIES");
            if (useArea)
            {
                PrintH2("AREA");
                _log.Info(GetIndentedText($"{GetAlignedText("SUBSET", " ", 10)} {GetFeatureText(areaFeatures)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("GEOMETRY", " ", 10)} {GetVectorKindText(System.Area)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("PROPERTY", " ", 10)} {GetPropertyText(System.Area)}", 4));
                PrintEmptyLine();
            }
            if (useBuilding)
            {
                PrintH2("BUILDING");
                _log.Info(GetIndentedText($"{GetAlignedText("SUBSET", " ", 10)} {GetFeatureText(buildingFeatures)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("GEOMETRY", " ", 10)} {GetVectorKindText(System.Building)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("PROPERTY", " ", 10)} {GetPropertyText(System.Building)}", 4));
                PrintEmptyLine();
            }
            if (useNetwork)
            {
                PrintH2("NETWORK");
                _log.Info(GetIndentedText($"{GetAlignedText("SUBSET", " ", 10)} {GetFeatureText(networkFeatures)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("GEOMETRY", " ", 10)} {GetVectorKindText(System.Network)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("PROPERTY", " ", 10)} {GetPropertyText(System.Network)}", 4));
                PrintEmptyLine();
            }
            if (usePOI)
            {
                PrintH2("POI");
                _log.Info(GetIndentedText($"{GetAlignedText("SUBSET", " ", 10)} {GetFeatureText(poiFeatures)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("GEOMETRY", " ", 10)} {GetVectorKindText(System.POI)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("PROPERTY", " ", 10)} {GetPropertyText(System.POI)}", 4));
                PrintEmptyLine();
            }
            if (useRoute)
            {
                PrintH2("ROUTE");
                _log.Info(GetIndentedText($"{GetAlignedText("SUBSET", " ", 10)} {GetFeatureText(routeFeatures)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("GEOMETRY", " ", 10)} {GetVectorKindText(System.Route)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("PROPERTY", " ", 10)} {GetPropertyText(System.Route)}", 4));
                PrintEmptyLine();
            }
            if (useZoning)
            {
                PrintH2("ZONING");
                _log.Info(GetIndentedText($"{GetAlignedText("SUBSET", " ", 10)} {GetFeatureText(zoningFeatures)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("GEOMETRY", " ", 10)} {GetVectorKindText(System.Zoning)}", 4));
                _log.Info(GetIndentedText($"{GetAlignedText("PROPERTY", " ", 10)} {GetPropertyText(System.Zoning)}", 4));
                PrintEmptyLine();
            }
            if (terrainRasters != 0)
            {
                PrintH2("TERRAIN");
                _log.Info(GetIndentedText($"{GetAlignedText("GEOMETRY", " ", 10)} {GetRasterKindText(terrainRasters)}", 4));
                PrintEmptyLine();
            }
            if (waterRasters != 0)
            {
                PrintH2("WATER");
                _log.Info(GetIndentedText($"{GetAlignedText("GEOMETRY", " ", 10)} {GetRasterKindText(waterRasters)}", 4));
                PrintEmptyLine();
            }

            PrintH1("PROJECTION");

            switch (options.SourceProjection)
            {
                case Geodata.CRS.TransverseMercator:
                    _log.Info(GetIndentedText(GetAlignedText("PROJECTION", "Transverse Mercator", 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("X", options.SourceCoordinates.x.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("Y", options.SourceCoordinates.y.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("ELLIPSOID", $"EPSG:{Epsg.Ellipsoid.GetCode(options.TargetEllipsoid)}", 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("SEMI_MAJOR", options.TargetProjectionDefinition.ellipsoid.a.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("INV_F", options.TargetProjectionDefinition.ellipsoid.rf.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("ORIGIN_LONG", options.TargetProjectionDefinition.origin.x.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("ORIGIN_LAT", options.TargetProjectionDefinition.origin.y.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("FALSE_EASTING", options.TargetProjectionDefinition.shift.x.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("FALSE_NORTHING", options.TargetProjectionDefinition.shift.y.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("SCALE_FACTOR", options.TargetProjectionDefinition.scaleFactor.ToString(numericFormat, culture), 36), 2));
                    if (transformText.Length <= 18)
                    {
                        _log.Info(GetIndentedText(GetAlignedText("TRANSFORM", transformText, 36), 2));
                    }
                    else
                    {
                        _log.Info(GetIndentedText($"{GetAlignedText("TRANSFORM", " ", 17)}{transformText}", 2));
                    }
                    break;

                case Geodata.CRS.UTM:
                    string hemisphere = options.SourceCoordinates.Hemisphere == Hemisphere.North ? "N" : "S";
                    _log.Info(GetIndentedText(GetAlignedText("PROJECTION", $"UTM / {options.SourceCoordinates.UTMZone}{hemisphere}", 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("X", options.SourceCoordinates.x.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("Y", options.SourceCoordinates.y.ToString(numericFormat, culture), 36), 2));
                    break;

                case Geodata.CRS.WGS84:
                    _log.Info(GetIndentedText(GetAlignedText("PROJECTION", "WGS84", 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("LONGITUDE", options.SourceCoordinates.x.ToString(numericFormat, culture), 36), 2));
                    _log.Info(GetIndentedText(GetAlignedText("LATITUDE", options.SourceCoordinates.y.ToString(numericFormat, culture), 36), 2));
                    break;
            }
            PrintEmptyLine();

            PrintH1("MISC");
            PrintH2("FILE");
            _log.Info(GetIndentedText(GetAlignedText("ELEVATION", $"{options.Elevation}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("MINIMIZED", $"{options.Minimized}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("COMPLETION_DIALOG", $"{options.CompletionDialog}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("COMPLETION_SOUND", $"{options.CompletionSound}", 34), 4));
            PrintEmptyLine();
            PrintH2("GEOMETRY");
            _log.Info(GetIndentedText(GetAlignedText("INACTIVE_ROUTE", $"{options.InactiveRoute}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("SERVICE_UPGRADE", $"{options.SeparateServiceUpgrade}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("UNZONED", $"{options.Unzoned}", 34), 4));
            PrintEmptyLine();
            PrintH2("PROPERTY");
            _log.Info(GetIndentedText(GetAlignedText("MAPTILE_STATS", $"{options.StatisticsMapTile}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("HOMELESS", $"{options.Homeless}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("CATEGORY_BUILDING_DISPLAY", $"{buildingDisplay}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("CATEGORY_NETWORK_DISPLAY", $"{networkDisplay}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("CATEGORY_POI_DISPLAY", $"{poiDisplay}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("CATEGORY_ROAD", $"{options.RoadClassification}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("PASSENGER_PET", $"{options.PetPassenger}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("RESIDENT_GENDER", $"{options.SeparateResident}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("THEME_ASSET_PACK", $"{options.AssetPack}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("WAGE_TAXABLE", $"{options.Taxable}", 34), 4));
            _log.Info(GetIndentedText(GetAlignedText("ZONING_DISPLAY", $"{zoningDisplay}", 34), 4));

            PrintDoubleLine();
        }
    }
}