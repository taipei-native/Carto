namespace Carto.Geodata
{
    /// <summary>
    /// The container of parsed projection parameters.<br/>
    /// （解析後的投影參數容器。）
    /// </summary>
    public readonly struct ParsedParams
    {
        public ParsedParams(Coord _center, CRS _projection, ProjectionDefinition _projectionDefinition)
        {
            center = _center;
            projection = _projection;
            projectionDefinition = _projectionDefinition;
        }
        
        /// <summary>
        /// The projection center.
        /// （投影中心。）
        /// </summary>
        public readonly Coord center;

        /// <summary>
        /// The projection method.
        /// （投影方式。）
        /// </summary>
        public readonly CRS projection;

        /// <summary>
        /// The definition of the projection.
        /// （投影法的定義。）
        /// </summary>
        public readonly ProjectionDefinition projectionDefinition;
    }
}