namespace Carto.Geodata
{
    public static class Epsg
    {
        public static class Crs
        {
            /// <summary>
            /// urn:ogc:def:crs:EPSG::4326
            /// </summary>
            public const int WGS84 = 4326;
        }
        
        public static class Ellipsoid
        {
            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7008
            /// </summary>
            public const int Clarke66 = 7008;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7015
            /// </summary>
            public const int Everest37 = 7015;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7019
            /// </summary>
            public const int GRS80 = 7019;

            /// <summary>
            /// urn:ogc:def:ellipsoid:EPSG::7030
            /// </summary>
            public const int WGS84 = 7030;

            public static short GetCode(IO.Ellipsoid ellipsoid)
            {
                return ellipsoid switch
                {
                    IO.Ellipsoid.Clrk66 => Clarke66,
                    IO.Ellipsoid.Evrst37 => Everest37,
                    IO.Ellipsoid.GRS80 => GRS80,
                    IO.Ellipsoid.WGS84 => WGS84,
                    _ => UserDefined
                };
            }

            public static string GetName(IO.Ellipsoid ellipsoid)
            {
                return ellipsoid switch
                {
                    IO.Ellipsoid.Clrk66 => "Clarke 1866",
                    IO.Ellipsoid.Evrst37 => "Everest 1830 (1937 Adjustment)",
                    IO.Ellipsoid.GRS80 => "GRS 1980",
                    IO.Ellipsoid.WGS84 => "WGS 84",
                    _ => "User Defined Ellipsoid"
                };
            }
        }

        public static class Meridian
        {
            /// <summary>
            /// urn:ogc:def:meridian:EPSG::8901
            /// </summary>
            public const int Greenwich = 8901;
        }

        public static class Uom
        {
            /// <summary>
            /// urn:ogc:def:uom:EPSG::9102
            /// </summary>
            public const int Degree = 9102;

            /// <summary>
            /// urn:ogc:def:uom:EPSG::9001
            /// </summary>
            public const int Metre = 9001;
        }

        public const int UserDefined = 32767;
    }
}