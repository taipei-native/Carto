using Unity.Mathematics;

namespace Carto.Geodata
{
    /// <summary>
    /// The definition of a coordinate.
    /// （坐標的定義。）
    /// </summary>
    public struct Coord
    {
        /// <summary>
        /// The hemisphere where the coordinates located in.
        /// （坐標所在的半球。）
        /// </summary>
        public Hemisphere hemisphere;

        /// <summary>
        /// The x value.
        /// （x 值。）
        /// </summary>
        public double x;

        /// <summary>
        /// The y value.
        /// （y 值。）
        /// </summary>
        public double y;

        /// <summary>
        /// The z value (height).
        /// （z（高度）值。）
        /// </summary>
        public double z;

        /// <summary>
        /// The zone number of UTM projection.
        /// （UTM 投影的區域編號。）
        /// </summary>
        public int zone;

        public Coord(double3 coordinate)
        {
            hemisphere = Hemisphere.North;
            x = coordinate.x;
            y = coordinate.y;
            z = coordinate.z;
            zone = 0;
        }

        public Coord(double3 coordinate, Coord utmReference)
        {
            hemisphere = utmReference.hemisphere;
            x = coordinate.x;
            y = coordinate.y;
            z = coordinate.z;
            zone = utmReference.zone;
        }

        public Coord((double x, double y) coordinate, double height = 0)
        {
            hemisphere = Hemisphere.North;
            x = coordinate.x;
            y = coordinate.y;
            z = height;
            zone = 0;
        }

        public Coord((double x, double y) coordinate, Coord utmReference, double height = 0)
        {
            hemisphere = utmReference.hemisphere;
            x = coordinate.x;
            y = coordinate.y;
            z = height;
            zone = utmReference.zone;
        }

        public Coord((double easting, double northing, int zone, Hemisphere hemisphere) coordinate, double height = 0)
        {
            hemisphere = coordinate.hemisphere;
            x = coordinate.easting;
            y = coordinate.northing;
            z = height;
            zone = coordinate.zone;
        }

        public Coord(CoordSafe coord)
        {
            hemisphere = coord.hemisphere == 0 ? Hemisphere.North : Hemisphere.South;
            x = coord.x;
            y = coord.y;
            z = coord.z;
            zone = coord.zone;
        }

        /// <summary>
        /// The <see cref="double3"/> representation of the coordinates.<br/>
        /// （坐標的 <see cref="double3"/> 表示法。）
        /// </summary>
        public readonly double3 Double3 => new(x, y, z);

        /// <summary>
        /// The <see cref="float3"/> representation of the coordinates.<br/>
        /// （坐標的 <see cref="float3"/> 表示法。）
        /// </summary>
        public readonly float3 Float3 => new(Double3);

        /// <summary>
        /// The tuple representation of the coordinates.<br/>
        /// （坐標的元組表示法。）
        /// </summary>
        public readonly (double x, double y) Tuple => (x, y);

        /// <summary>
        /// The tuple representation of the UTM coordinates.<br/>
        /// （UTM 坐標的元組表示法。）
        /// </summary>
        public readonly (double easting, double northing, int zone, Hemisphere hemisphere) UTMTuple => (x, y, zone, hemisphere);

        /// <summary>
        /// The safe version of the struct that can be passed into the job.
        /// （可被傳遞進工作中的安全版本。）
        /// </summary>
        /// <returns>The <see cref="CoordSafe"/> object.（<see cref="CoordSafe"/> 物件。）</returns>
        public readonly CoordSafe ToSafe() => new(this);
    }

    public struct CoordSafe
    {
        public int hemisphere;
        public double x;
        public double y;
        public double z;
        public int zone;

        public CoordSafe(Coord coord)
        {
            hemisphere = (int) coord.hemisphere;
            x = coord.x;
            y = coord.y;
            z = coord.z;
            zone = coord.zone;
        }

        public CoordSafe(double x, double y, double z, int hemisphere, int zone)
        {
            this.hemisphere = hemisphere;
            this.x = x;
            this.y = y;
            this.z = z;
            this.zone = zone;
        }

        public readonly Coord ToUnsafe() => new(this);
    }
}