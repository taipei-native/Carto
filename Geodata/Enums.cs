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
}