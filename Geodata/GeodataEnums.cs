namespace Carto.Geodata
{
    /// <summary>
    /// The coordinate reference systems.
    /// （坐標參考系統。）
    /// </summary>
    public enum CRS
    {
        /// <summary>
        /// The fallback value for unkonwn or unregistered CRS.（用於未知或未註冊坐標參考系統的後備值。）
        /// </summary>
        Unknown,

        /// <summary>
        /// The in-game coordinate system. The length unit is meter, and the datum is a flat plane.<br/>
        /// （遊戲內的坐標系統。長度單位是公尺，大地基準是一個平面。）
        /// </summary>
        Game,

        /// <summary>
        /// A variant of Mercator projection also known as Web Mercator (EPSG: 3857), widely used in web applications.<br/>
        /// （偽麥卡托投影（或網路麥卡托投影，EPSG: 3857），經常被用於網路應用程式中。）<br/>
        /// See also: <seealso href="https://en.wikipedia.org/wiki/Web_Mercator_projection">Web Mercator projection</seealso>
        /// </summary>
        PseudoMercator,

        /// <summary>
        /// The generic Transverse Mercator projection, whose cylinder's axis lies on the meridian instead of the Equator when comparing to Mercator.<br/>
        /// （橫麥卡托投影，與麥卡托投影相比，其圓柱的投影軸線在經線而非赤道上。）<br/>
        /// See also: <seealso href="https://en.wikipedia.org/wiki/Transverse_Mercator_projection">Transverse Mercator projection</seealso>
        /// </summary>
        TransverseMercator,

        /// <summary>
        /// The Universal Transverse Mercator (EPSG: 32xxx), a popular variant of the transverse mercator projection.<br/>
        /// （通用橫麥卡托投影（EPSG: 32xxx），橫麥卡托投影的受歡迎變種之一。）<br/>
        /// See also: <seealso href="https://en.wikipedia.org/wiki/Universal_Transverse_Mercator_coordinate_system">UTM coordinate system</seealso>
        /// </summary>
        UTM,

        /// <summary>
        /// The World Geodetic System 84 (EPSG: 4326), one of the most popular geodetic coordinate system of the globe.<br/>
        /// （世界大地測量系統（EPSG: 4326），最受歡迎的地球大地坐標系統之一。）<br/>
        /// See also: <seealso href="https://en.wikipedia.org/wiki/World_Geodetic_System">World Geodetic System</seealso>
        /// </summary>
        WGS84
    }

    /// <summary>
    /// The reference ellipsoids of popular datums.
    /// （受歡迎的大地基準的參考橢球體。）
    /// </summary>
    public enum Ellipsoid
    {
        /// <summary>
        /// The fallback value for unknown or unregistered ellipsoid.（用於未知或未註冊橢球體的後備值。）
        /// </summary>
        Unknown,

        /// <summary>
        /// User-defined custom ellipsoid.（使用者定義的客製橢球體。）
        /// </summary>
        Custom,

        /// <summary>
        /// The Geodetic Reference System 1980 ellipsoid.（1980年大地參考系統橢球體。）
        /// </summary>
        GRS80,

        /// <summary>
        /// The World Geodetic System 1984 ellipsoid.（1984年世界大地測量系統橢球體。）
        /// </summary>
        WGS84
    }

    /// <summary>
    /// North and south hemisphere of the Earth.
    /// （地球的南北半球。）
    /// </summary>
    public enum Hemisphere
    {
        /// <summary>
        /// The northern hemisphere.（北半球。）
        /// </summary>
        North,

        /// <summary>
        /// The southern hemisphere.（南半球。）
        /// </summary>
        South
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
        /// The number of residents in the area.（居民，區域內的居民數量。）
        /// </summary>
        Resident,

        /// <summary>
        /// The number of above-ground levels of the building.（樓層，建築的地上樓層數量。）
        /// </summary>
        Story,

        /// <summary>
        /// The number of stops on the route.（站點，路線上的站點數量。）
        /// </summary>
        Stop,

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
        /// The average economy conditions of all households in the building.（財富，建築內所有家庭的平均經濟情況。）
        /// </summary>
        Wealth,

        /// <summary>
        /// The width of the networks in meters (m).（寬度，網路的寬度，單位為公尺（m）。）
        /// </summary>
        Width,

        /// <summary>
        /// The classification of designated development purposes.（分區，土地的發展用途。）
        /// </summary>
        Zoning
    }

    /// <summary>
    /// The subject of the raster data.
    /// （網格資料的主體。）
    /// </summary>
    public enum RasterKind
    {
        /// <summary>
        /// The fallback value for unknown or unregistered raster kinds.（用於未知或未註冊網格資料的後備值。）
        /// </summary>
        Unknown,
        
        /// <summary>
        /// The pollution spread in the air.（在空氣中飄散的汙染。）
        /// </summary>
        AirPollution,

        /// <summary>
        /// The depth of the water bodies.（水體的深度。）
        /// </summary>
        Depth,

        /// <summary>
        /// The elevation of the terrain.（地形的高程。）
        /// </summary>
        Elevation,

        /// <summary>
        /// The deposit of the fertile land.（沃土的蘊藏量。）
        /// </summary>
        FertileDeposit,

        /// <summary>
        /// The direction in which surface water towards to.（地表水的移動方向。）
        /// </summary>
        FlowDirection,

        /// <summary>
        /// The speed of the surface water.（地表水的流速。）
        /// </summary>
        FlowSpeed,

        /// <summary>
        /// The pollution that spread in the soil.（在土壤中擴散的汙染。）
        /// </summary>
        GroundPollution,

        /// <summary>
        /// The deposit of the ground water.（地下水的蘊藏量。）
        /// </summary>
        GroundWaterDeposit,

        /// <summary>
        /// The pollution that emits in the ground water.（排放至地下水中的汙染。）
        /// </summary>
        GroundWaterPollution,

        /// <summary>
        /// The value of the land.（土地的價格。）
        /// </summary>
        LandValue,

        /// <summary>
        /// The pollution spread in the form of sound.（以聲音形式傳播的汙染。）
        /// </summary>
        NoisePollution,

        /// <summary>
        /// The deposit of the crude oil.（原油的蘊藏量。）
        /// </summary>
        OilDeposit,

        /// <summary>
        /// The deposit of the ore.（礦物的蘊藏量。）
        /// </summary>
        OreDeposit,

        /// <summary>
        /// The pollution that emits in the water.（排放至水中的汙染。）
        /// </summary>
        WaterPollution,

        /// <summary>
        /// The direction in which wind towards to.（風吹拂的方向。）
        /// </summary>
        WindDirection,

        /// <summary>
        /// The speed of the wind.（風的速度。） 
        /// </summary>
        WindSpeed,

        /// <summary>
        /// The deposit of the wood.（木材的蘊藏量。）
        /// </summary>
        WoodDeposit,

        /// <summary>
        /// The depth of the water bodies in the worldmap.（世界地形圖中的水體深度。）
        /// </summary>
        WorldDepth,

        /// <summary>
        /// The elevation of the terrain in the worldmap.（世界地形圖中的地形高程。）
        /// </summary>
        WorldElevation
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
    /// The subject of the vector data.
    /// （向量資料的主體。）
    /// </summary>
    public enum VectorKind
    {
        /// <summary>
        /// The fallback value for unknown or unregistered vector kinds.（用於未知或未註冊向量資料的後備值。）
        /// </summary>
        Unknown,

        /// <summary>
        /// The outline of the object.（物體的外圍輪廓線。）
        /// </summary>
        Boundary,

        /// <summary>
        /// The lines that lie in the middle of the networks or the routes.（位於網路或路線中央的線段。）
        /// </summary>
        Centerline,

        /// <summary>
        /// The coverage area of the property.（建築的覆蓋面。）
        /// </summary>
        Footprint,

        /// <summary>
        /// The location of POIs.（興趣點的位置。）
        /// </summary>
        Location
    }
}