using System.Collections.Generic;

namespace Carto.Geodata
{
    /// <summary>
    /// The class to store and export raster geodata.
    /// （儲存與輸出網格地理資料的類別。）
    /// </summary>
    public class RasterData : Geodata
    {
        /// <summary>
        /// Define the grids of each category.
        /// （定義各種類型的網格。）
        /// </summary>
        public Dictionary<RasterKind, float[][]> Grids { get; set; }

        public RasterData(Dictionary<RasterKind, float[][]> grids)
        {
            Grids = grids;
        }

        /// <summary>
        /// The RasterKind in the grids.
        /// （網格的類型。）
        /// </summary>
        public RasterKind Kinds
        {
            get
            {
                if (Grids == null)
                {
                    return RasterKind.Unknown;
                }

                RasterKind kinds = RasterKind.Unknown;

                foreach (RasterKind kind in Grids.Keys)
                {
                    kinds |= kind;
                }

                return kinds;
            }
        }
    }
}