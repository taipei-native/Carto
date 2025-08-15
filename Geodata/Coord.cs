using System;
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
        /// The type of CRS of the coordinate.（坐標的坐標參考系統類別。）
        /// </summary>
        public CRS crs;

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
        /// The hemisphere where the coordinate located in.
        /// （坐標所在的半球。）
        /// </summary>
        private readonly Hemisphere _hemisphere;

        /// <summary>
        /// The zone number of UTM projection.
        /// （UTM 投影的區域編號。）
        /// </summary>
        private readonly int _zone;

        public Coord(double x, double y, CRS crs = CRS.Game)
        {
            this.crs = crs;
            this.x = x;
            this.y = y;
            z = 0;
            _hemisphere = (crs == CRS.WGS84) && (y < 0) ? Hemisphere.South : Hemisphere.North;
            _zone = crs == CRS.WGS84 ? (int)Math.Round(math.floor((x + 180) / 6) + 1) : 0;
        }

        public Coord(double x, double y, double z, CRS crs = CRS.Game)
        {
            this.crs = crs;
            this.x = x;
            this.y = y;
            this.z = z;
            _hemisphere = Hemisphere.North;
            _zone = 0;
        }

        public Coord(double x, double y, Hemisphere hemisphere, int zone)
        {
            crs = CRS.UTM;
            this.x = x;
            this.y = y;
            z = 0;
            _hemisphere = hemisphere;
            _zone = zone;
        }

        public Coord(double x, double y, double z, Hemisphere hemisphere, int zone)
        {
            crs = CRS.UTM;
            this.x = x;
            this.y = y;
            this.z = z;
            _hemisphere = hemisphere;
            _zone = zone;
        }

        /// <summary>
        /// The hemisphere where the coordinate located in.
        /// （坐標所在的半球。）
        /// </summary>
        public readonly Hemisphere Hemisphere
        {
            get
            {
                return crs switch
                {
                    CRS.UTM => _hemisphere,
                    CRS.WGS84 => y >= 0 ? Hemisphere.North : Hemisphere.South,
                    _ => throw new NotSupportedException("The CRS other than UTM and WGS84 is not supported. 不支援 UTM 與 WGS84 以外的坐標參考系統。")
                };
            }
        }

        /// <summary>
        /// The zone number of UTM projection.
        /// （UTM 投影的區域編號。）
        /// </summary>
        public readonly int UTMZone
        {
            get
            {
                return crs switch
                {
                    CRS.UTM => _zone,
                    CRS.WGS84 => (int)Math.Round(math.floor((x + 180) / 6) + 1),
                    _ => throw new NotSupportedException("The CRS other than UTM and WGS84 is not supported. 不支援 UTM 與 WGS84 以外的坐標參考系統。")
                };
            }
        }

        /// <summary>
        /// Round the coordinate value.
        /// （四捨五入坐標值。）
        /// </summary>
        /// <returns></returns>
        public readonly Coord Round()
        {
            int place = crs == CRS.WGS84 ? 9 : 7;
            return new(Math.Round(x, place), Math.Round(y, place), z, crs);
        }

        /// <summary>
        /// Shift the coordinate.（平移坐標。）
        /// </summary>
        /// <param name="shift">The shift in x and y direction.（X 與 Y 方向的平移量。）</param>
        /// <returns>The shifted coordinate.（平移後的坐標。）</returns>
        public readonly Coord Shift(float2 shift)
        {
            return new(x + shift.x, y + shift.y, z, _hemisphere, _zone);
        }

        /// <summary>
        /// Shift the coordinate.（平移坐標。）
        /// </summary>
        /// <param name="shift">The shift in x, y, and z direction.（X、Y 與 Z 方向的平移量。）</param>
        /// <returns>The shifted coordinate.（平移後的坐標。）</returns>
        public readonly Coord Shift(float3 shift)
        {
            return new(x + shift.x, y + shift.y, z + shift.z, _hemisphere, _zone);
        }

        /// <summary>
        /// Shift the coordinate.（平移坐標。）
        /// </summary>
        /// <param name="x">The shift in x direction.（X 方向的平移量。）</param>
        /// <param name="y">The shift in y direction.（Y 方向的平移量。）</param>
        /// <param name="z">The shift in z direction.（Z 方向的平移量。）</param>
        /// <returns>The shifted coordinate.（平移後的坐標。）</returns>
        public readonly Coord Shift(double x, double y, double z)
        {
            return new(this.x + x, this.y + y, this.z + z, _hemisphere, _zone);
        }

        /// <summary>
        /// Convert the coordinate to <see cref="double3"/>.
        /// （將坐標轉換為 <see cref="double3"/>。）
        /// </summary>
        /// <returns>A double3 instance.（一個 double3 實例。）</returns>
        public readonly double3 ToDouble3()
        {
            return new(x, y, z);
        }

        public override readonly string ToString()
        {
            return $"Coord({x}, {y}, {z}) - CRS [{crs}], Hemisphere [{_hemisphere}], Zone [{_zone}]";
        }
    }
}