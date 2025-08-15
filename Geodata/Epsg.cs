namespace Carto.Geodata
{
    /// <summary>
    /// The class that stores the EPSG code for several common datums and projections.<br/>
    /// （儲存數種常見大地基準與投影 EPSG 代號的類別。）
    /// </summary>
    public static class Epsg
    {
        /// <summary>
        /// The coordinate reference system.
        /// （坐標參考系統。）
        /// </summary>
        public static class Crs
        {
            /// <summary>
            /// urn:ogc:def:crs:EPSG::4326 "WGS 84"<br/>
            /// The World Geodetic System 1984.<br/>
            /// （1984 年世界大地系統。）
            /// </summary>
            public const int WGS84 = 4326;
        }
        
        /// <summary>
        /// The reference ellipsoid.
        /// （參考橢球體。）
        /// </summary>
        public static class Ellipsoid
        {
            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7001 "Airy 1830"<br/>
            /// The 1830 Airy ellipsoid is the primary ellipsoid in British Isles.<br/>
            /// （1830 年的艾里橢球體是不列顛群島的主要橢球體。）
            /// </summary>
            public const int Airy30 = 7001;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7004 "Bessel 1841"<br/>
            /// The 1841 Bessel ellipsoid was one of the most widely used ellipsoid before the Hayford ellipsoid (EPSG: 7022).<br/>
            /// （1841 年的貝塞爾橢球體，曾是在海福德橢球體（EPSG: 7022）問世前最常用的橢球體之一。）
            /// </summary>
            public const int Bssl41 = 7004;
            
            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7008 "Clarke 1866"<br/>
            /// The 1866 Clarke ellipsoid is famous for being the reference ellipsoid of NAD27 datum.<br/>
            /// （1866 年的克拉克橢球體，因被 NAD27 使用而著名。）
            /// </summary>
            public const int Clarke66 = 7008;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7015 "Everest 1830 (1937 Adjustment)"<br/>
            /// The 1937 readjustment of the Everest ellipsoid introduced in 1830.<br/>
            /// （1830 年的埃佛勒斯（Everest）橢球體的 1937 年修正版本。）
            /// </summary>
            public const int Everest37 = 7015;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7036 "GRS 1967"<br/>
            /// The ellipsoid for the Geodetic Reference System 1967, which is the international standard between 1967 and 1979.<br/>
            /// （1967 年世界大地參考系統的參考橢球體，在 1967 年至 1979 年間曾為國際標準。）
            /// </summary>
            public const int GRS67 = 7036;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7019 "GRS 1980"<br/>
            /// The ellipsoid for the Geodetic Reference System 1980, which is the international standard since 1979.<br/>
            /// （1980 年世界大地參考系統的參考橢球體，為自 1979 年以來的國際標準。）
            /// </summary>
            public const int GRS80 = 7019;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::1025 "GSK-2011"<br/>
            /// The ellipsoid for PZ-90, which is the ellipsoid for GLONASS.<br/>
            /// （GLONASS 之大地基準 PZ-90 的參考橢球體。）
            /// </summary>
            public const int GSK11 = 1025;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7011 "Clarke 1880 (IGN)"<br/>
            /// The 1866 Clarke ellipsoid modified by Institut Géographique National in 1880.<br/>
            /// （1866 年的克拉克橢球體，於 1880 年被國家地理研究院修改的版本。）
            /// </summary>
            public const int IGN80 = 7011;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7022 "International 1924"<br/>
            /// The Hayford ellipsoid was the international standard between 1924 and 1967.<br/>
            /// （海福德（Hayford）橢球體在 1924 年至 1967 年間曾為國際標準。）
            /// </summary>
            public const int Intl24 = 7022;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7024 "Krassowsky 1940"<br/>
            /// The ellipsoid that was first adopted by USSR in 1942.<br/>
            /// （最早於 1942 年由蘇聯採用的橢球體。）
            /// </summary>
            public const int Krsky40 = 7024;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7012 "Clarke 1880 (RGS)"<br/>
            /// The 1866 Clarke ellipsoid modified by Royal Geographical Society in 1880.<br/>
            /// （1866 年的克拉克橢球體，於 1880 年被皇家地理學會修改的版本。）
            /// </summary>
            public const int RGS80 = 7012;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7030 "WGS84"<br/>
            /// The ellipsoid for the World Geodetic System 1984.<br/>
            /// （1984 年世界大地系統的參考橢球體。）
            /// </summary>
            public const int WGS84 = 7030;

            /// <summary>
            /// Retrieve the registered EPSG id of a given ellipsoid.
            /// （獲得指定橢球體登記的 EPSG 代號。）
            /// </summary>
            /// <param name="ellipsoid">Input ellipsoid.（輸入的橢球體。）</param>
            /// <returns>The EPSG code.（EPSG 代號。）</returns>
            public static short GetCode(IO.Ellipsoid ellipsoid)
            {
                return ellipsoid switch
                {
                    IO.Ellipsoid.Airy30 => Airy30,
                    IO.Ellipsoid.Bssl41 => Bssl41,
                    IO.Ellipsoid.Clrk66 => Clarke66,
                    IO.Ellipsoid.Evrst37 => Everest37,
                    IO.Ellipsoid.GRS67 => GRS67,
                    IO.Ellipsoid.GRS80 => GRS80,
                    IO.Ellipsoid.GSK11 => GSK11,
                    IO.Ellipsoid.IGN80 => IGN80,
                    IO.Ellipsoid.Intl24 => Intl24,
                    IO.Ellipsoid.Krsky40 => Krsky40,
                    IO.Ellipsoid.RGS80 => RGS80,
                    IO.Ellipsoid.WGS84 => WGS84,
                    _ => UserDefined
                };
            }

            /// <summary>
            /// Retrieve the name of a given ellipsoid.
            /// （獲得指定橢球體的名稱。）
            /// </summary>
            /// <param name="ellipsoid">Input ellipsoid.（輸入的橢球體。）</param>
            /// <returns>The string representing the ellipsoid.（代表橢球體的字串。）</returns>
            public static string GetName(IO.Ellipsoid ellipsoid)
            {
                return ellipsoid switch
                {
                    IO.Ellipsoid.Airy30 => "Airy 1830",
                    IO.Ellipsoid.Bssl41 => "Bessel 1841",
                    IO.Ellipsoid.Clrk66 => "Clarke 1866",
                    IO.Ellipsoid.Evrst37 => "Everest 1830 (1937 Adjustment)",
                    IO.Ellipsoid.GRS67 => "GRS 1967",
                    IO.Ellipsoid.GRS80 => "GRS 1980",
                    IO.Ellipsoid.GSK11 => "GSK-2011",
                    IO.Ellipsoid.IGN80 => "Clarke 1880 (IGN)",
                    IO.Ellipsoid.Intl24 => "International 1924",
                    IO.Ellipsoid.Krsky40 => "Krassowsky 1940",
                    IO.Ellipsoid.RGS80 => "Clarke 1880 (RGS)",
                    IO.Ellipsoid.WGS84 => "WGS 84",
                    _ => "User Defined Ellipsoid"
                };
            }
        }

        /// <summary>
        /// The prime meridian.
        /// （本初經線。）
        /// </summary>
        public static class Meridian
        {
            /// <summary>
            /// urn:ogc:def:meridian:EPSG::8901 "Greenwich"<br/>
            /// The Greenwich prime meridian, which is the international standards since 1884.<br/>
            /// （格林威治子午線，自 1884 年以來的國際標準。）
            /// </summary>
            public const int Greenwich = 8901;
        }

        /// <summary>
        /// The unit of measure.
        /// （測量單位。）
        /// </summary>
        public static class Uom
        {
            /// <summary>
            /// urn:ogc:def:uom:EPSG::9102 "degree"<br/>
            /// A degree is π/180 radians.<br/>
            /// （度，相當於 π/180 弧度。）
            /// </summary>
            public const int Degree = 9102;

            /// <summary>
            /// urn:ogc:def:uom:EPSG::9001 "metre"<br/>
            /// The SI base unit for length.<br/>
            /// （公尺，國際單位制的長度基本單位。）
            /// </summary>
            public const int Metre = 9001;
        }

        /// <summary>
        /// The reserved code for user defined content.
        /// （為使用者自訂內容預留的代號。）
        /// </summary>
        public const int UserDefined = 32767;
    }
}