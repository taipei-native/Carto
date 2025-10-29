using System;

namespace Carto.Domain
{
    /// <summary>
    /// The building types in the game.
    /// （遊戲內的建築類別。）
    /// </summary>
    [Flags]
    public enum BuildingCategory : long
    {
        /// <summary>
        /// The fallback value for unknown or unregistered building type.（用於未知或未註冊建築類別的後備值。）
        /// </summary>
        None = 0,

        // Building Status（建築狀態）
        /// <summary>
        /// The abandoned building.（廢棄的建築。）
        /// </summary>
        Abandoned = 1 << 0,

        /// <summary>
        /// The building in the wrong zoning and would be demolished soon.（因分區錯誤，將被拆除的建築。）
        /// </summary>
        Condemned = 1 << 1,

        /// <summary>
        /// The under construction building.（建造中的建築。）
        /// </summary>
        Construction = 1 << 2,

        /// <summary>
        /// The destroyed building.（被摧毀的建築。）
        /// </summary>
        Destroyed = 1 << 3,

        // Building properties（建築屬性）
        /// <summary>
        /// The building upgrades that require main buildings.（需要主建築的建築升級。）
        /// </summary>
        Extension = 1 << 4,

        /// <summary>
        /// The buildings that automatically spawn on special industries.（自動生成在特化工業上的建築。）
        /// </summary>
        Extractor = 1 << 5,

        /// <summary>
        /// The private buildings that spawn on the dedicated zonings.（生成在特定分區的私人建築。）
        /// </summary>
        Property = 1 << 6,

        /// <summary>
        /// Public service buildings.（公共服務建築。）
        /// </summary>
        Public = 1 << 7,

        // Building tags（建築標籤）
        Admin = 1 << 8,
        Communication = 1 << 9,
        Decoration = 1 << 10,
        Disaster = 1 << 11,
        Education = 1 << 12,
        Fire = 1 << 13,
        Health = 1 << 14,
        Maintenance = 1 << 15,
        Mortuary = 1 << 16,
        Park = 1 << 17,
        Parking = 1 << 18,
        Police = 1 << 19,
        Post = 1 << 20,
        Power = 1 << 21,
        Research = 1 << 22,
        Sewage = 1 << 23,
        Transportation = 1 << 24,
        Waste = 1 << 25,
        Water = 1 << 26,

        // Extractor tags（特殊工業標籤）
        Farmland = 1 << 27,
        Fishery = 1 << 28,
        Forestry = 1 << 29,
        Landfill = 1 << 30,
        OilField = 1L << 31,
        Quarry = 1L << 32,
        Ranch = 1L << 33
    }

    /// <summary>
    /// The direction of the network.
    /// （網路的方向。）
    /// </summary>
    [Flags]
    public enum Direction
    {
        /// <summary>
        /// The fallback value for unknown or unregistered direction.（用於未知或未註冊方向的後備值。）
        /// </summary>
        None = 0,

        /// <summary>
        /// The network can be traveled from the start node.
        /// （網路由起點節點出發。）
        /// </summary>
        Forward = 1,

        /// <summary>
        /// The network can be traveled from the end node.
        /// （網路由迄點節點出發。）
        /// </summary>
        Backward = 2,

        /// <summary>
        /// The network can be traveled both ways.
        /// （網路由起點或訖點出發。）
        /// </summary>
        Both = 3
    }

    /// <summary>
    /// The specific phase to dipose native containers.
    /// （丟棄原生容器的特定階段。）
    /// </summary>
    public enum DisposePhase
    {
        /// <summary>
        /// Dispose containers no longer needed after <see cref="Systems.AreaSystem"/>'s operation.<br/>
        /// （丟棄在 <see cref="Systems.AreaSystem"/> 操作後不再需要的容器。）
        /// </summary>
        AfterAreaSystem,

        /// <summary>
        /// Dispose containers no longer needed after <see cref="Systems.BuildingSystem"/>'s operation.<br/>
        /// （丟棄在 <see cref="Systems.BuildingSystem"/> 操作後不再需要的容器。）
        /// </summary>
        AfterBuildingSystem,

        /// <summary>
        /// Dispose containers no longer needed after <see cref="Systems.SharedDataCollectionSystem.GetBuildingStats"/>'s operation.<br/>
        /// （丟棄在 <see cref="Systems.SharedDataCollectionSystem.GetBuildingStats"/> 操作後不再需要的容器。）
        /// </summary>
        AfterBuildingStats,

        /// <summary>
        /// Dispose containers no longer needed after terrain related operations in <see cref="Systems.RasterSystem"/>.<br/>
        /// （丟棄在 <see cref="Systems.RasterSystem"/> 與地形相關操作執行後不再需要的容器。）
        /// </summary>
        AfterTerrainRelated,

        /// <summary>
        /// Dispose containers no longer needed after <see cref="Systems.ZoningSystem"/>'s operation.<br/>
        /// （丟棄在 <see cref="Systems.ZoningSystem"/> 操作後不再需要的容器。）
        /// </summary>
        AfterZoningSystem
    }

    /// <summary>
    /// The form of the network.
    /// （網路的形式。）
    /// </summary>
    public enum Form
    {
        /// <summary>
        /// The ground level network.
        /// （地面網路。）
        /// </summary>
        Normal,

        /// <summary>
        /// The bridge.
        /// （橋梁。）
        /// </summary>
        Elevated,

        /// <summary>
        /// The underground tunnel.
        /// （地下隧道。）
        /// </summary>
        Tunnel
    }

    /// <summary>
    /// The network types in the game.
    /// （遊戲內的網路類別。）
    /// </summary>
    [Flags]
    public enum NetworkCategory
    {
        /// <summary>
        /// The fallback value for unknown or unregistered network type.（用於未知或未註冊網路類別的後備值。）
        /// </summary>
        None = 0,

        /// <summary>
        /// The small road.（小路。）
        /// </summary>
        Small = 1 << 0,

        /// <summary>
        /// The medium road.（中路。）
        /// </summary>
        Medium = 1 << 1,

        /// <summary>
        /// The large road.（大路。）
        /// </summary>
        Large = 1 << 2,

        /// <summary>
        /// The highway.（公路／高速公路。）
        /// </summary>
        Highway = 1 << 3,
        
        /// <summary>
        /// The general category for roads.（道路的概括分類。）
        /// </summary>
        Car = Small | Medium | Large,

        /// <summary>
        /// The road with bus lane.（有公車專用道的道路。）
        /// </summary>
        Bus = 1 << 4,

        /// <summary>
        /// The tram tracks.（電車軌道。）
        /// </summary>
        Tram = 1 << 5,

        /// <summary>
        /// The subway tracks.（捷運軌道。）
        /// </summary>
        Subway = 1 << 6,

        /// <summary>
        /// The train tracks.（火車軌道。）
        /// </summary>
        Train = 1 << 7,

        /// <summary>
        /// The passage reserved for pedestrians.（路徑，保留給行人通行的通道。）
        /// </summary>
        Pathway = 1 << 8,

        /// <summary>
        /// The road that connects aprons, hangers and terminals to the runway.（滑行道。）
        /// </summary>
        Taxiway = 1 << 9,

        /// <summary>
        /// The road reserved for aircrafts' taking off & landing purposes.（跑道。）
        /// </summary>
        Runway = 1 << 10,

        /// <summary>
        /// The path ships travel along to.（航道。）
        /// </summary>
        Waterway = 1 << 11,

        /// <summary>
        /// The wires that carries low voltage electricity.（低壓電纜線。）
        /// </summary>
        LowCable = 1 << 12,

        /// <summary>
        /// The wires that carries low voltage electricity.（高壓電纜線。）
        /// </summary>
        HighCable = 1 << 13,

        /// <summary>
        /// The tube that carries fresh water.（自來水道。）
        /// </summary>
        WaterPipe = 1 << 14,

        /// <summary>
        /// The tube that carries sewage water.（汙水下水道。）
        /// </summary>
        SewagePipe = 1 << 15,

        /// <summary>
        /// The tube that carries storm water.（雨水下水道。）
        /// </summary>
        StormPipe = 1 << 16,

        /// <summary>
        /// The man-made barriers.（圍籬。）
        /// </summary>
        Fence = 1 << 17,

        /// <summary>
        /// The networks made by Road Builder.（由 Road Builder 製作的網路。）
        /// </summary>
        RoadBuilder = 1 << 18,
    }

    /// <summary>
    /// The POI types in the game.
    /// （遊戲內的興趣點類別。）
    /// </summary>
    public enum POICategory
    {
        /// <summary>
        /// The fallback value for unknown or unregistered feature type.（用於未知或未註冊圖徵的後備值。）
        /// </summary>
        None = 0,

        // Public Service Facilities（公眾服務設施）
        Admin,
        Communication,
        Disaster,
        EducationCollege,
        EducationElementary,
        EducationGeneric,
        EducationHigh,
        EducationUniversity,
        Fire,
        FireWatchTower,
        Health,
        Maintenance,
        MortuaryCemetery,
        MortuaryCrematorium,
        MortuaryGeneric,
        Police,
        Post,
        PostBox,
        PowerBattery,
        PowerDam,
        PowerGeneric,
        PowerPlant,
        PowerSubstation,
        PowerTurbine,
        Prison,
        Research,
        Sewage,
        UtilityPole,
        UtilityPylon,
        Waste,
        Water,

        // Public Transportation Facilities（大眾運輸設施）
        Helipad,
        LevelCrossing,
        Parking,
        SpaceCenter,
        TrafficLight,
        BuildingBus,
        BuildingCargoAirplane,
        BuildingCargoShip,
        BuildingCargoTrain,
        BuildingFerry,
        BuildingHelicopter,
        BuildingPassengerAirplane,
        BuildingPassengerShip,
        BuildingPassengerTrain,
        BuildingSubway,
        BuildingTaxi,
        BuildingTram,
        DepotBus,
        DepotFerry,
        DepotGeneric,
        DepotSubway,
        DepotTaxi,
        DepotTrain,
        DepotTram,
        StopBus,
        StopCargoAirplane,
        StopCargoShip,
        StopCargoTrain,
        StopFerry,
        StopHelicopter,
        StopPassengerAirplane,
        StopPassengerShip,
        StopPassengerTrain,
        StopSubway,
        StopTaxi,
        StopTram,
        TransportationGeneric,

        // Commercial Facilities & Offices（商辦設施）
        /// <summary>
        /// The office that sells the products not listed below.（販售未在下表列出的產品的辦公場所。）
        /// </summary>
        StoreOffice,

        /// <summary>
        /// The commercial store that sells the products not listed below.（販售未在下表列出的產品的商店。）
        /// </summary>
        StoreGeneric,

        /// <summary>
        /// The office that sells Financial.（販售金融的辦公場所。）
        /// </summary>
        StoreBank,

        /// <summary>
        /// The store that sells Entertainment.（販售娛樂的商店。）
        /// </summary>
        StoreBar,

        /// <summary>
        /// The store that sells Paper.（販售紙張的商店。）
        /// </summary>
        StoreBookStore,

        /// <summary>
        /// The store that sells Beverage.（販售飲料的商店。）
        /// </summary>
        StoreBeverage,

        /// <summary>
        /// The store that sells Vehicles.（販售車輛的商店。）
        /// </summary>
        StoreCarStore,

        /// <summary>
        /// The store that sells Chemicals.（販售化學品的商店。）
        /// </summary>
        StoreChemicals,

        /// <summary>
        /// The store that sells Convenience Food.（販售即時食品的商店。）
        /// </summary>
        StoreConvenienceStore,

        /// <summary>
        /// The store that sells Pharmaceuticals.（販售藥品的商店。）
        /// </summary>
        StoreDrugStore,

        /// <summary>
        /// The store that sells Electronics.（販售電子產品的商店。）
        /// </summary>
        StoreElectronics,

        /// <summary>
        /// The store that sells Textiles.（販售紡織品的商店。）
        /// </summary>
        StoreFashionStore,

        /// <summary>
        /// The store that sells Food.（販售食物的商店。）
        /// </summary>
        StoreFood,

        /// <summary>
        /// The store that sells Furnitures.（販售家具的商店。）
        /// </summary>
        StoreFurniture,

        /// <summary>
        /// The store that sells Petrochemicals.（販售石化產品的商店。）
        /// </summary>
        StoreGasStation,

        /// <summary>
        /// The store that sells Lodging.（販售旅宿的商店。）
        /// </summary>
        StoreHotel,

        /// <summary>
        /// The office that sells Media.（販售媒體的辦公場所。）
        /// </summary>
        StoreMedia,

        /// <summary>
        /// The store that sells Plastics.（販售塑膠的商店。）
        /// </summary>
        StorePlastics,

        /// <summary>
        /// The store that sells Recreation.（販售休閒的商店。）
        /// </summary>
        StoreRecreation,

        /// <summary>
        /// The store that sells Meals.（販售膳食的商店。）
        /// </summary>
        StoreRestaurant,

        /// <summary>
        /// The office that sells Software.（販售軟體的辦公場所。）
        /// </summary>
        StoreSoftware,

        /// <summary>
        /// The office that sells Telecom.（販售電信的辦公場所。）
        /// </summary>
        StoreTelecom,

        // Industrial Facilities（工業設施）
        IndustrialCoal,
        IndustrialCotton,
        IndustrialFactory,
        IndustrialFish,
        IndustrialGeneric,
        IndustrialGrain,
        IndustrialLivestock,
        IndustrialOil,
        IndustrialOre,
        IndustrialStone,
        IndustrialVegetables,
        IndustrialWarehouse,
        IndustrialWood,

        // Other Facilities（其他設施）
        /// <summary>
        /// The tourist attractions & landmarks.（旅遊景點與地標。）
        /// </summary>
        Attraction,
        Park
    }

    /// <summary>
    /// The UI sound effect types.
    /// （UI 音效的種類。）
    /// </summary>
    public enum Sound
    {
        /// <summary>
        /// The completion sound.
        /// （完成音效。）
        /// </summary>
        Completion
    }

    /// <summary>
    /// The transport types in the game.
    /// （遊戲內運輸的種類。）
    /// </summary>
    [Flags]
    public enum TransportCategory
    {
        None = 0,
        Airplane = 1 << 0,
        Bus = 1 << 1,
        Ferry = 1 << 2,
        Helicopter = 1 << 3,
        Ship = 1 << 4,
        Subway = 1 << 5,
        Taxi = 1 << 6,
        Train = 1 << 7,
        Tram = 1 << 8
    }

    /// <summary>
    /// The basic zoning types in the game.
    /// （遊戲內的基本分區類別。）
    /// </summary>
    [Flags]
    public enum ZoningCategory
    {
        /// <summary>
        /// The fallback value for unknown or unregistered feature type.（用於未知或未註冊圖徵的後備值。）
        /// </summary>
        None = 0,

        /// <summary>
        /// The zoning type that provides residences.（提供居所的分區類別。）
        /// </summary>
        Residential = 1,

        /// <summary>
        /// The zoning type that provides tertiary sector services and hires people with medium education.（提供第三級服務、雇傭受中等教育的分區類別。）
        /// </summary>
        Commercial = 2,

        /// <summary>
        /// The zoning type that provides primary and secondary services and hires people with poor education.（提供第一級與第二級服務、雇傭受初級教育的分區類別。）
        /// </summary>
        Industrial = 4,

        /// <summary>
        /// The zoning type that provides tertiary sector services and hires people with well education.（提供第三級服務、雇傭受高等教育的分區類別。）
        /// </summary>
        Office = 8
    }

    /// <summary>
    /// The development strength of the zoning.
    /// （分區的發展強度。）
    /// </summary>
    public enum ZoningDensity
    {
        /// <summary>
        /// The density is not specified.
        /// （密度未指定。）
        /// </summary>
        Generic,

        /// <summary>
        /// Low density development.
        /// （低強度開發。）
        /// </summary>
        Low,

        /// <summary>
        /// Medium density development.
        /// （中強度開發。）
        /// </summary>
        Medium,

        /// <summary>
        /// High density development.
        /// （高強度開發。）
        /// </summary>
        High
    }
}