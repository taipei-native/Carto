using System;

namespace Carto.IO
{
    /// <summary>
    /// The coordinate reference systems.
    /// （坐標參考系統。）
    /// </summary>
    public enum CRS
    {
        /// <summary>
        /// See <see cref="Geodata.CRS.TransverseMercator"/>.
        /// （請見 <see cref="Geodata.CRS.TransverseMercator"/>。）
        /// </summary>
        TransverseMercator,

        /// <summary>
        /// See <see cref="Geodata.CRS.UTM"/>.
        /// （請見 <see cref="Geodata.CRS.UTM"/>。）
        /// </summary>
        UTM,

        /// <summary>
        /// See <see cref="Geodata.CRS.WGS84"/>.
        /// （請見 <see cref="Geodata.CRS.WGS84"/>。）
        /// </summary>
        WGS84
    }
    
    /// <summary>
    /// The display mode of the tags.
    /// （標籤的顯示模式。）
    /// </summary>
    public enum Display
    {
        /// <summary>
        /// Assign all applicable tags.（列出所有合適的標籤。）
        /// </summary>
        All,

        /// <summary>
        /// Assign one applicable tag to each feature, and if there are multiple tags applicable to the feature, only the best one will be selected.（為美個圖徵列出一個最適合的標籤。）
        /// </summary>
        Single
    }

    public enum ElevationOrigin
    {
        Game,

        SeaLevel
    }

    /// <summary>
    /// The reference ellipsoids of popular datums.
    /// （受歡迎的大地基準的參考橢球體。）
    /// </summary>
    public enum Ellipsoid
    {
        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.GSK11"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.GSK11"/>。）
        /// </summary>
        GSK11,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.Airy30"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.Airy30"/>。）
        /// </summary>
        Airy30,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.Bssl41"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.Bssl41"/>。）
        /// </summary>
        Bssl41,
        
        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.Clarke66"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.Clarke66"/>。）
        /// </summary>
        Clrk66,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.IGN80"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.IGN80"/>。）
        /// </summary>
        IGN80,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.RGS80"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.RGS80"/>。）
        /// </summary>
        RGS80,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.Everest37"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.Everest37"/>。）
        /// </summary>
        Evrst37,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.GRS80"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.GRS80"/>。）
        /// </summary>
        GRS80,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.Intl24"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.Intl24"/>。）
        /// </summary>
        Intl24,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.Krsky40"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.Krsky40"/>。）
        /// </summary>
        Krsky40,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.WGS84"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.WGS84"/>。）
        /// </summary>
        WGS84,

        /// <summary>
        /// See <see cref="Geodata.Epsg.Ellipsoid.GRS67"/>.
        /// （請見 <see cref="Geodata.Epsg.Ellipsoid.GRS67"/>。）
        /// </summary>
        GRS67,

        /// <summary>
        /// The custom ellipsoid.
        /// （自訂橢球體。）
        /// </summary>
        Custom
    }
    
    /// <summary>
    /// The classification of user's input error.
    /// （對使用者輸入錯誤的分類。）
    /// </summary>
    public enum Error
    {
        /// <summary>
        /// No error.
        /// （沒有錯誤。）
        /// </summary>
        None,

        /// <summary>
        /// General error flag. Can be caused by any reason.
        /// （可能由任何原因引起的通用錯誤旗標。）
        /// </summary>
        General,

        /// <summary>
        /// The latitude value is invalid.
        /// （無效的緯度值。）
        /// </summary>
        Latitude,

        /// <summary>
        /// The longitude value is invalid.
        /// （無效的經度值。）
        /// </summary>
        Longitude,

        /// <summary>
        /// The world heightmap is missing from the editor/save.
        /// （編輯器／存檔缺少世界高度圖。）
        /// </summary>
        MissingWorldHeightmap,

        /// <summary>
        /// The value is not a number.
        /// （輸入值不是數字。）
        /// </summary>
        Nan,

        /// <summary>
        /// The value is a negative number.
        /// （輸入值是負數。）
        /// </summary>
        Negative,

        /// <summary>
        /// The options is null.
        /// （設定為空值。）
        /// </summary>
        NullOptions,

        /// <summary>
        /// The file path is invalid or missing.
        /// （無效或缺少檔案路徑。）
        /// </summary>
        Path,

        /// <summary>
        /// Share violation on the exporting files.
        /// （存取輸出檔案遭拒。）
        /// </summary>
        ShareViolation,

        /// <summary>
        /// The Helmert Transform parameters are invalid.
        /// （無效的赫爾默特轉換參數。）
        /// </summary>
        Transform,

        /// <summary>
        /// The length of Helmert Transform parameter is invalid.
        /// （赫爾默特轉換參數的長度無效。）
        /// </summary>
        TransformLength,

        /// <summary>
        /// The zone value is invalid.
        /// （無效的分區值。）
        /// </summary>
        UTMZone
    }

    /// <summary>
    /// Carto's classification of exportable objects.
    /// （Carto 對可輸出物體的分類。）
    /// </summary>
    [Flags]
    public enum Feature
    {
        /// <summary>
        /// The fallback value for unknown or unregistered feature type.（用於未知或未註冊圖徵的後備值。）
        /// </summary>
        None = 0,

        /// <summary>
        /// The placeable structures.（建築，可放置的結構物。）
        /// </summary>
        Building = 1 << 0,

        /// <summary>
        /// The wires that carries electricity.（纜線，輸送電力的線路。）
        /// </summary>
        Cable = 1 << 1,

        /// <summary>
        /// The administrative division of an area.（行政區，對一個區域的行政劃分。）
        /// </summary>
        District = 1 << 2,

        /// <summary>
        /// The resource collection area.（開採工業，收集資源的區域。）
        /// </summary>
        Extractor = 1 << 3,

        /// <summary>
        /// The man-made barriers.（圍籬，人造的屏障。）
        /// </summary>
        Fence = 1 << 4,

        /// <summary>
        /// The place where garbages are buried.（掩埋場，垃圾被掩埋的場所。）
        /// </summary>
        Landfill = 1 << 5,

        /// <summary>
        /// The division of in-game playable area.（地圖區塊，遊戲內可遊玩區域的劃分。）
        /// </summary>
        MapTile = 1 << 6,
        
        /// <summary>
        /// The passage reserved for pedestrians.（路徑，保留給行人通行的通道。）
        /// </summary>
        Pathway = 1 << 7,

        /// <summary>
        /// The tube that carries the fresh water, sewage water or storm water.（管道，輸送自來水、汙水與雨水的線路。）
        /// </summary>
        Pipe = 1 << 8,

        /// <summary>
        /// The private properties' POI.（私人設施的興趣點。）
        /// </summary>
        POIPrivate = 1 << 9,

        /// <summary>
        /// The public services' POI.（公共設施的興趣點。）
        /// </summary>
        POIPublic = 1 << 10,

        /// <summary>
        /// The transportation facilities' POI.（交通設施的興趣點。）
        /// </summary>
        POITransport = 1 << 11,

        /// <summary>
        /// The utility service's POI.（公用設施的興趣點。）
        /// </summary>
        POIUtility = 1 << 12,

        /// <summary>
        /// The passage primarily reserved for road vehicles.（道路，主要保留給車輛通行的通道。）
        /// </summary>
        Road = 1 << 13,

        /// <summary>
        /// The public transport route that delivers cargo.（輸送貨物的公共交通路線。）
        /// </summary>
        RouteCargo = 1 << 14,

        /// <summary>
        /// The public transport route that delivers passengers.（輸送乘客的公共交通路線。）
        /// </summary>
        RoutePassenger = 1 << 15,

        /// <summary>
        /// The road reserved for aircrafts' taking off & landing purposes.（跑道，用於航空器起降的道路。）
        /// </summary>
        Runway = 1 << 16,

        /// <summary>
        /// The placeable surface textures.（表面，可放置的地表紋理。）
        /// </summary>
        Surface = 1 << 17,

        /// <summary>
        /// The road that connects aprons, hangers and terminals to the runway.（滑行道，聯絡停機坪、機棚與航廈的道路。）
        /// </summary>
        Taxiway = 1 << 18,

        /// <summary>
        /// The path trains travel along to.（軌道，列車行駛的路徑。）
        /// </summary>
        Track = 1 << 19,

        /// <summary>
        /// The path ships travel along to.（航道，船舶航行的路徑。）
        /// </summary>
        Waterway = 1 << 20,

        /// <summary>
        /// The urban planning basic division units.（分區單元，都市計畫的基本單位。）
        /// </summary>
        Zoning = 1 << 21
    }
    
    /// <summary>
    /// The data type of the fields.
    /// （欄位的型別。）
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// The boolean.
        /// （布林值。）
        /// </summary>
        Bool,

        /// <summary>
        /// The single-precision floating-point number.
        /// （單精度浮點數。）
        /// </summary>
        Float,

        /// <summary>
        /// The 32-bit integer.
        /// （32 位元整數。）
        /// </summary>
        Int,

        /// <summary>
        /// The character string.
        /// （字串。）
        /// </summary>
        String
    }

    /// <summary>
    /// The file formats Carto supports.
    /// （Carto 支援的檔案格式。）
    /// </summary>
    public enum FileFormat
    {
        /// <summary>
        /// The fallback value for unknown or unregistered file format.（用於未知或未註冊檔案格式的後備值。）
        /// </summary>
        Unknown,
        
        /// <summary>
        /// The geospatial file format based on JSON.
        /// （基於 JSON 的地理空間檔案格式。）
        /// </summary>
        GeoJSON,

        /// <summary>
        /// The geospatial file format based on Sqlite database.
        /// （基於 Sqlite 資料庫的地理空間檔案格式。）
        /// </summary>
        GeoPackage,

        /// <summary>
        /// The geospatial file format based on TIFF.
        /// （基於 TIFF 的地理空間檔案格式。）
        /// </summary>
        GeoTIFF,

        /// <summary>
        /// The geospatial file format developed by ESRI.
        /// （由 ESRI 開發的檔案格式。）
        /// </summary>
        Shapefile,

        /// <summary>
        /// The experimental file format used for Carto's web map.
        /// （用於 Carto 網路地圖的實驗性檔案格式。）
        /// </summary>
        WebMap
    }

    /// <summary>
    /// The GeoTIFF formats Carto supports.
    /// （Carto 支援的 GeoTIFF 格式。）
    /// </summary>
    public enum GeoTiffFormat
    {
        /// <summary>
        /// Single-precision floating-point numbers. Range from -3.4E+38 to 3.4E+38.<br/>
        /// （單精度浮點數，範圍由 -3.4E+38 至 3.4E+38。）
        /// </summary>
        Float32,

        /// <summary>
        /// 16-bit signed integers. Range from -32768 to 32767.<br/>
        /// （16 位元帶正負號整數，範圍由 -32768 至 32767。）
        /// </summary>
        Int16,

        /// <summary>
        /// 16-bit unsigned integers. Range from 0 to 65535.<br/>
        /// （16 位元不帶正負號整數，範圍由 0 至 65535。）
        /// </summary>
        Norm16
    }

    /// <summary>
    /// Various file naming formats.
    /// （不同的檔案命名格式。）
    /// </summary>
    public enum NamingFormat
    {
        /// <summary>
        /// Custom file naming format.
        /// （自訂檔案命名格式。）
        /// </summary>
        Custom,

        /// <summary>
        /// {Feature}
        /// </summary>
        Feature,

        /// <summary>
        /// {City}_{Feature}
        /// </summary>
        CityNameFeature,

        /// <summary>
        /// {Map}_{Feature}
        /// </summary>
        MapNameFeature
    }

    /// <summary>
    /// The orientation of the vertex ordering.
    /// （頂點排序的方向。）
    /// </summary>
    public enum Order
    {
        /// <summary>
        /// The vertices should be aligned clockwise-ly. This ordering is adopted by ESRI Shapefile.
        /// （頂點應以順時鐘排列。這是 ESRI Shapefile 採用的順序。）
        /// </summary>
        Clockwise = 0,

        /// <summary>
        /// The vertices should be aligned counterclockwise-ly. This ordering is adopted by GeoJSON and Simple Features (the basis of WKB, and GeoPackage uses WKB.)<br/>
        /// （頂點應以逆時鐘排列。這是 GeoJSON 及 Simple Features（WKB 的基礎，而 GeoPackage 使用 WKB）採用的順序。）
        /// </summary>
        Counterclockwise = 1
    }

    /// <summary>
    /// The OS platform.
    /// （作業系統平臺。）
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// The fallback value for unknown or unregistered OS.（用於未知或未註冊作業系統的後備值。）
        /// </summary>
        Unknown,

        /// <summary>
        /// The open-source OS originally developed by Linus Torvalds. As of February 2024, its PC market share is about 6%.<br/>
        /// 開源作業系統，原始由林納斯·托瓦茲開發。截至 2024 年 2 月，其個人電腦的市占率約為 6%。
        /// </summary>
        Linux,

        /// <summary>
        /// The commercial OS developed by Apple Inc.. As of February 2024, its PC market share is about 15%.<br/>
        /// 商業作業系統，由 Apple 開發。截至 2024 年 2 月，其個人電腦的市占率約為 15%。
        /// </summary>
        OSX,

        /// <summary>
        /// The commercial OS developed by Microsoft. As of February 2024, its PC market share is about 72%.<br/>
        /// 商業作業系統，由 Microsoft 開發。截至 2024 年 2 月，其個人電腦的市占率約為 72%。
        /// </summary>
        Windows
    }
    
    /// <summary>
    /// The property of the features.
    /// （圖徵的屬性。）
    /// </summary>
    public enum Property
    {
        /// <summary>
        /// The fallback value for unknown or unregistered properties.（用於未知或未註冊屬性的後備值。）
        /// </summary>
        Unknown,

        /// <summary>
        /// The in-game building identifier.（地址，遊戲內建築的編碼。）
        /// </summary>
        Address,

        /// <summary>
        /// The average length of time that residents in the area have lived in years.（年齡，區域內居民存活的平均時間長度。）
        /// </summary>
        Age,

        /// <summary>
        /// The extent of the object in square meters (m²).（面積，物件的占地面積，單位為平方公尺（m²）。）
        /// </summary>
        Area,

        /// <summary>
        /// The title of the asset.（資產，資產的名稱。）
        /// </summary>
        Asset,

        /// <summary>
        /// The name of the enterprise that rents the property.（品牌，租下建築的企業名稱。）
        /// </summary>
        Brand,

        /// <summary>
        /// The maximum amount of subtance that the utility pipes can hold.<br/>
        /// Count in cubic meters per month (m³/month) for sewage and water pipes, or count in kilowatts (kW) for power cables. <br/>
        /// （容量，管線所能乘載的物質數量上限。對於汙水與自來水管單位為立方公尺／月（m³/month），對於電纜單位為千瓦（kW）。）
        /// </summary>
        Capacity,

        /// <summary>
        /// The sub-division of the in-game objects.（分類，物件的進一步分類。）
        /// </summary>
        Category,

        /// <summary>
        /// The route or zoning color in the game.（顏色，遊戲內路線或分區顯示的顏色。）
        /// </summary>
        Color,

        /// <summary>
        /// Number of companies in the area.（公司，區域內的公司數量。）
        /// </summary>
        Company,

        /// <summary>
        /// The development intensity of the zoning type.（密度，分區的發展強度或是聚集密度。）
        /// </summary>
        Density,

        /// <summary>
        /// The direction in which fluids or vehicles move.（方向，流體或載具的移動方向。）
        /// </summary>
        Direction,

        /// <summary>
        /// The amount of substance transported in the pipe each month in cubic meters per month (m³/month).（流率，管線每月輸送的物質數量，單位為立方公尺／月（m³/month）。）
        /// </summary>
        Discharge,

        /// <summary>
        /// The average elevation of the buildings, networks or POIs in meters (m).（高程，建築、網路或 POI 的平均高程，單位為公尺（m）。）
        /// </summary>
        Elevation,

        /// <summary>
        /// The number of employees in the area.（員工，區域內的受雇員工數量。）
        /// </summary>
        Employee,

        /// <summary>
        /// The form of the network.（形式，網路的外觀形式。）
        /// </summary>
        Form,

        /// <summary>
        /// The height of the building in meters (m).（樓高，建築的高度，單位為公尺（m）。）
        /// </summary>
        Height,

        /// <summary>
        /// The number of households in the area.（家庭，區域內的家庭數量。）
        /// </summary>
        Household,

        /// <summary>
        /// The number of labor force reside in the area.（勞動力，居住於區域內的勞工數量。）
        /// </summary>
        Labor,

        /// <summary>
        /// The number of car lanes on the network.（車道，網路上的機動車道數量。）
        /// </summary>
        Lane,

        /// <summary>
        /// The length of the networks or the routes in meters (m).（長度，網路或路線的長度，單位為公尺（m）。）
        /// </summary>
        Length,

        /// <summary>
        /// The upgrade progress of the building.（等級，建築的升級進度。）
        /// </summary>
        Level,

        /// <summary>
        /// The speed limit of the networks in unknown unit.（速限，網路的最高速度限制，單位未知。）
        /// </summary>
        Limit,

        /// <summary>
        /// The amount of power transported in the cable each month in kilowatts (kW).（負載，電纜每月輸送的能量，單位為千瓦（kW）。）
        /// </summary>
        Load,

        /// <summary>
        /// The asset title of the vehicles operating on the route.（型號，在路線上營運的載具資產名稱。）
        /// </summary>
        Model,

        /// <summary>
        /// The name of the object.（名稱，物件的名稱。）
        /// </summary>
        Name,

        /// <summary>
        /// Carto's classification of in-game objects.（物體，Carto 對遊戲內物體的分類。）
        /// </summary>
        Object,

        /// <summary>
        /// The number of the passenger of the route.（乘客，路線的乘客數量。）
        /// </summary>
        Passenger,

        /// <summary>
        /// The merchandise sold by stores or factories.（產品，建築物生產或銷售的商品名稱。）
        /// </summary>
        Product,

        /// <summary>
        /// The average profit of all companies in the area in ₡ per month (₡/month).（利潤，區域內所有公司的平均利潤金額，單位為₡／月（₡/month）。）
        /// </summary>
        Profit,

        /// <summary>
        /// The number of residents in the area.（居民，區域內的居民數量。）
        /// </summary>
        Resident,

        /// <summary>
        /// The serial number of the transportation route.（路線編號，路線被賦予的流水號。）
        /// </summary>
        Route,

        /// <summary>
        /// The ratio of males to females in the area in percentage (%).（性別比，區域內男性對女性的比值，單位為百分比（%）。）
        /// </summary>
        SexRatio,

        /// <summary>
        /// The number of stops on the route.（站點，路線上的站點數量。）
        /// </summary>
        Stop,

        /// <summary>
        /// The number of above-ground levels of the building.（樓層，建築的地上樓層數量。）
        /// </summary>
        Storey,

        /// <summary>
        /// The style of the assets.（主題，資產的主題風格。）
        /// </summary>
        Theme,

        /// <summary>
        /// The transportation mode of the route.（運輸，路線的運具分類。）
        /// </summary>
        Transport,

        /// <summary>
        /// The purchase status of the map tiles.（解鎖，地圖區塊的購買狀態。）
        /// </summary>
        Unlocked,

        /// <summary>
        /// The usage rate of the route.（使用率，使用量與容量的比值。）
        /// </summary>
        Usage,

        /// <summary>
        /// The land value of the property.（價值，建築的地價。）
        /// </summary>
        Value,

        /// <summary>
        /// The number of the vehicles on the route.（車輛，路線上的車輛數量。）
        /// </summary>
        Vehicle,

        /// <summary>
        /// The traffic volume of the networks in cars per hour (car/hr).（流量，網路上每小時通過的車輛數量，單位為車／小時（car/hr））
        /// </summary>
        Volume,

        /// <summary>
        /// The average wage of all labors in the area in ₡ per month (₡/month).（薪資，區域內所有勞工的平均薪資金額，單位為₡／月（₡/month）。）
        /// </summary>
        Wage,

        /// <summary>
        /// The total weight of the cargo transported on the route.（重量，透過路線運輸的貨物總重量。）
        /// </summary>
        Weight,

        /// <summary>
        /// The width of the networks in meters (m).（寬度，網路的寬度，單位為公尺（m）。）
        /// </summary>
        Width,

        /// <summary>
        /// The name of the zoning type.（分區名稱，建築所屬的分區名稱。）
        /// </summary>
        Zone,

        /// <summary>
        /// The classification of designated development purposes.（分區，土地的發展用途。）
        /// </summary>
        Zoning
    }

    /// <summary>
    /// The subject of the raster data.
    /// （網格資料的主體。）
    /// </summary>
    [Flags]
    public enum RasterKind
    {
        /// <summary>
        /// The fallback value for unknown or unregistered raster kinds.（用於未知或未註冊網格資料的後備值。）
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The pollution spread in the air.（在空氣中飄散的汙染。）
        /// </summary>
        AirPollution = 1 << 0,

        /// <summary>
        /// The depth of the water bodies.（水體的深度。）
        /// </summary>
        Depth = 1 << 1,

        /// <summary>
        /// The elevation of the terrain.（地形的高程。）
        /// </summary>
        Elevation = 1 << 2,

        /// <summary>
        /// The deposit of the fertile land.（沃土的蘊藏量。）
        /// </summary>
        FertileDeposit = 1 << 3,

        /// <summary>
        /// The deposit of the fish.（水產的蘊藏量。）
        /// </summary>
        FishDeposit = 1 << 4,

        /// <summary>
        /// The direction in which surface water towards to.（地表水的移動方向。）
        /// </summary>
        FlowDirection = 1 << 5,

        /// <summary>
        /// The speed of the surface water.（地表水的流速。）
        /// </summary>
        FlowSpeed = 1 << 6,

        /// <summary>
        /// The pollution that spread in the soil.（在土壤中擴散的汙染。）
        /// </summary>
        GroundPollution = 1 << 7,

        /// <summary>
        /// The deposit of the ground water.（地下水的蘊藏量。）
        /// </summary>
        GroundWaterDeposit = 1 << 8,

        /// <summary>
        /// The pollution that emits in the ground water.（排放至地下水中的汙染。）
        /// </summary>
        GroundWaterPollution = 1 << 9,

        /// <summary>
        /// The value of the land.（土地的價格。）
        /// </summary>
        LandValue = 1 << 10,

        /// <summary>
        /// The pollution spread in the form of sound.（以聲音形式傳播的汙染。）
        /// </summary>
        NoisePollution = 1 << 11,

        /// <summary>
        /// The deposit of the crude oil.（原油的蘊藏量。）
        /// </summary>
        OilDeposit = 1 << 12,

        /// <summary>
        /// The deposit of the ore.（礦物的蘊藏量。）
        /// </summary>
        OreDeposit = 1 << 13,

        /// <summary>
        /// The pollution that emits in the water.（排放至水中的汙染。）
        /// </summary>
        WaterPollution = 1 << 14,

        /// <summary>
        /// The direction in which wind towards to.（風吹拂的方向。）
        /// </summary>
        WindDirection = 1 << 15,

        /// <summary>
        /// The speed of the wind.（風的速度。） 
        /// </summary>
        WindSpeed = 1 << 16,

        /// <summary>
        /// The deposit of the wood.（木材的蘊藏量。）
        /// </summary>
        WoodDeposit = 1 << 17,

        /// <summary>
        /// The depth of the water bodies in the worldmap.（世界地形圖中的水體深度。）
        /// </summary>
        WorldDepth = 1 << 18,

        /// <summary>
        /// The elevation of the terrain in the worldmap.（世界地形圖中的地形高程。）
        /// </summary>
        WorldElevation = 1 << 19
    }

    /// <summary>
    /// The classification method of the road networks.
    /// （道路網路的分類方式。）
    /// </summary>
    public enum RoadClassification
    {
        /// <summary>
        /// By speed limit.
        /// （速限。）
        /// </summary>
        Limit,
        
        // TODO: Add REGEX support.（支援正則表達式。）
        ///// <summary>
        ///// By regular expression.
        ///// （正則表達式。）
        ///// </summary>
        //Regex,

        /// <summary>
        /// By vanilla classification.
        /// （原版遊戲分類。）
        /// </summary>
        Vanilla,

        /// <summary>
        /// By road width.
        /// （路寬。）
        /// </summary>
        Width
    }

    /// <summary>
    /// The category of the geometry shapes.
    /// （幾何形狀的分類。）
    /// </summary>
    public enum Shape
    {
        /// <summary>
        /// The fallback value for unknown or unregistered shapes.（用於未知或未註冊幾何形狀的後備值。）
        /// </summary>
        Unknown,

        /// <summary>
        /// Dots.（點。）
        /// </summary>
        Point,

        /// <summary>
        /// Line segments.（線段。）
        /// </summary>
        LineString,

        /// <summary>
        /// Enclosed area on the plane.（多邊形，平面上封閉的面積。）
        /// </summary>
        Polygon,

        /// <summary>
        /// Several polygons as a whole.（數個多邊形組成的幾何形狀。）
        /// </summary>
        MultiPolygon,

        /// <summary>
        /// Grid-like data.（網格。）
        /// </summary>
        Raster
    }

    /// <summary>
    /// The systems of Carto.
    /// （Carto 的系統。）
    /// </summary>
    [Flags]
    public enum System
    {
        /// <summary>
        /// The fallback value for unknown or unregistered systems.（用於未知或未註冊系統的後備值。）
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// <see cref="Systems.AreaSystem"/>.
        /// </summary>
        Area = 1,

        /// <summary>
        /// <see cref="Systems.BuildingSystem"/>.
        /// </summary>
        Building = 2,

        /// <summary>
        /// <see cref="Systems.NetworkSystem"/>.
        /// </summary>
        Network = 4,

        /// <summary>
        /// <see cref="Systems.POISystem"/>.
        /// </summary>
        POI = 8,

        /// <summary>
        /// <see cref="Systems.RasterSystem"/>.
        /// </summary>
        Raster = 16,

        /// <summary>
        /// <see cref="Systems.RouteSystem"/>.
        /// </summary>
        Route = 32,

        /// <summary>
        /// <see cref="Systems.ZoningSystem"/>.
        /// </summary>
        Zoning = 64
    }

    /// <summary>
    /// The subject of the vector data.
    /// （向量資料的主體。）
    /// </summary>
    [Flags]
    public enum VectorKind
    {
        /// <summary>
        /// The fallback value for unknown or unregistered vector kinds.（用於未知或未註冊向量資料的後備值。）
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The outline of the object.（物體的外圍輪廓線。）
        /// </summary>
        Boundary = 1,

        /// <summary>
        /// The lines that lie in the middle of the networks or the routes.（位於網路或路線中央的線段。）
        /// </summary>
        Centerline = 2,

        /// <summary>
        /// The coverage area of the property.（建築的覆蓋面。）
        /// </summary>
        Footprint = 4,

        /// <summary>
        /// The location of objects.（物體的位置。）
        /// </summary>
        Location = 8
    }
}