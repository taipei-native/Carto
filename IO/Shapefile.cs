using Carto.Geodata;
using Carto.Utils;
using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.Mathematics;

namespace Carto.IO
{
    /// <summary>
    /// The class that provides utility functions to write ESRI Shapefile.<br/>
    /// （提供寫出 ESRI Shapefile 功能的類別。）
    /// </summary>
    public static class Shapefile
    {
        /*
            # Source: （資料來源：）

            * Environmental Systems Research Institute, Inc. (1998). ESRI Shapefile Technical Description
                https://www.esri.com/content/dam/esrisites/sitecore-archive/Files/Pdfs/library/whitepapers/pdfs/shapefile.pdf

            * Bachmann, E. (2010). Xbase Data file (*.dbf)
                https://www.clicketyclick.dk/databases/xbase/format/dbf.html

            * MapTiler. (2024). WGS / UTM zone xxX - EPSG:32xxx
                https://epsg.io/
        */

        /// <summary>
        /// Points on the X-Y plane.
        /// （XY 平面上的點。）
        /// </summary>
        public const int shapeTypePoint = 1;

        /// <summary>
        /// Line segments on the X-Y plane.
        /// （XY 平面上的線段。）
        /// </summary>
        public const int shapeTypePolyLine = 3;

        /// <summary>
        /// Polygons on the X-Y plane.
        /// （XY 平面上的多邊形。）
        /// </summary>
        public const int shapeTypePolygon = 5;

        /// <summary>
        /// Points in the X-Y-Z space.
        /// （XYZ 空間內的點。）
        /// </summary>
        public const int shapeTypePointZ = 11;

        /// <summary>
        /// Line segments in the X-Y-Z space.
        /// （XYZ 空間內的線段。）
        /// </summary>
        public const int shapeTypePolyLineZ = 13;

        /// <summary>
        /// Polygons in the X-Y-Z space.
        /// （XYZ 空間內的多邊形。）
        /// </summary>
        public const int shapeTypePolygonZ = 15;

        /// <summary>
        /// The metadata of the Shapefile.
        /// （Shapefile 的元資料。）
        /// </summary>
        public struct Parameter
        {
            /// <summary>
            /// The geometry type.
            /// （幾何種類。）
            /// </summary>
            public int shape;
        }

        /// <summary>
        /// The delegate of the WriteSHP() methods implemented in each system.
        /// （在各個系統實作的 WriteSHP() 方法的委派。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="indexPairs">The index pairs used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        public delegate void WriteSHPMethod(BinaryWriter writer, out List<(int, int)> indexPairs, out Bounds3 bounds);

        /// <summary>
        /// Retrieve the shape type code from vector kinds.
        /// （由向量類別獲得幾何種類代號。）
        /// </summary>
        /// <param name="vectorKind">The classification of exported vector objects.（對輸出向量物體的分類。）</param>
        /// <param name="includeElevation">Whether to export the elevation or not.（是否要輸出高程？）</param>
        /// <returns>The shape code used in the shapefile.（在 Shapefile 中使用的幾何代號。）</returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetShapeType(VectorKind vectorKind, bool includeElevation)
        {
            return vectorKind switch
            {
                VectorKind.Boundary => includeElevation ? shapeTypePolygonZ : shapeTypePolygon,
                VectorKind.Centerline => includeElevation ? shapeTypePolyLineZ : shapeTypePolyLine,
                VectorKind.Footprint => includeElevation ? shapeTypePolygonZ : shapeTypePolygon,
                VectorKind.Location => includeElevation ? shapeTypePointZ : shapeTypePoint,
                _ => throw new ArgumentException("Only points, line strings and (multi-) polygons can be exported as Shapefile. 只有點、線段和（複合）多邊形可以被輸出為 Shapefile。")
            };
        }

        /// <summary>
        /// Update the .shp file header.
        /// （更新 .shp 檔案的標頭。）
        /// </summary>
        /// <param name="fs">The current file stream.（目前的檔案資料流。）</param>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bounds">The bounding box of the features.（圖徵的定界框。）</param>
        /// <param name="length">The length of the file.（檔案的長度。）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        public static void UpdateSHPHeader(FileStream fs, BinaryWriter writer, Bounds3 bounds, int length, bool writeElevation)
        {
            if (BitConverter.IsLittleEndian)
            {
                fs.Seek(24, SeekOrigin.Begin);
                writer.Write(IOUtils.GetFlippedBytes(length), 0, 4);
                fs.Seek(36, SeekOrigin.Begin);
                writer.Write(BitConverter.GetBytes((double)bounds.x.min), 0, 8);
                writer.Write(BitConverter.GetBytes((double)bounds.y.min), 0, 8);
                writer.Write(BitConverter.GetBytes((double)bounds.x.max), 0, 8);
                writer.Write(BitConverter.GetBytes((double)bounds.y.max), 0, 8);
                if (writeElevation)
                {
                    writer.Write(BitConverter.GetBytes((double)bounds.z.min), 0, 8);
                    writer.Write(BitConverter.GetBytes((double)bounds.z.max), 0, 8);
                }
            }
            else
            {
                fs.Seek(24, SeekOrigin.Begin);
                writer.Write(BitConverter.GetBytes(length), 0, 4);
                fs.Seek(36, SeekOrigin.Begin);
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.min), 0, 8);
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.min), 0, 8);
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.max), 0, 8);
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.max), 0, 8);
                if (writeElevation)
                {
                    writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.min), 0, 8);
                    writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.max), 0, 8);
                }
            }
        }

        /// <summary>
        /// Write the ESRI Shapefile.
        /// （寫出 ESRI Shapefile。）
        /// </summary>
        /// <param name="options">The export options.（輸出設定。）</param>
        /// <param name="systemName">The exporting system's name.（輸出系統的名稱。）</param>
        /// <param name="vectorKind">The classification of exported vector objects.（對輸出向量物體的分類。）</param>
        /// <param name="writeSHPMethod">The WriteSHP() method implemented in each system.（各系統實作的 WriteSHP() 方法。）</param>
        /// <param name="onReportMethod">The event listener to handle the export status report.（處理回報輸出進度的事件監聽者。）</param>
        public static void Write(Options options, System systemName, VectorKind vectorKind, WriteSHPMethod writeSHPMethod, Action<string, int> onReportMethod)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (options == null) throw new ArgumentNullException("The parameters cannot be null. 參數不可為空值。");
            string filePath = options.GetFilePath(systemName, vectorKind);
            string shxPath = Path.ChangeExtension(filePath, "shx");
            Bounds3 bounds;
            int shape = GetShapeType(vectorKind, options.Elevation);
            List<(int offset, int count)> indexPairs;

            // The shapefile is a format composed by at least three sidecar files - which means multiple files have to be generated.
            // （Shapefile 是一個由至少三個檔案組成的檔案格式－這表示需要產生出複數個檔案。）

            // Unlike TIFF, which supports writing in both endian, Shapefile has strict rule when writing files.
            // （不同於 TIFF 支援以兩種端序寫入，Shapefile 對於端序有嚴格的要求。）

            using (FileStream fs = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920))
            {
                using BinaryWriter writer = new(fs);
                WriteSHPHeader(writer, shape);
                writeSHPMethod.Invoke(writer, out indexPairs, out bounds);
                (int offset, int length) = indexPairs[indexPairs.Count - 1];
                UpdateSHPHeader(fs, writer, bounds, offset + length, options.Elevation);
            }

            using (FileStream fs = new(shxPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920))
            {
                using BinaryWriter writer = new(fs);
                WriteSHX(writer, shape, bounds, indexPairs);
            }

            stopwatch.Stop();
            Instance.Log.Debug($"Write '{Path.GetFileName(filePath)}' in {CommonUtils.FormatTimeSpan(stopwatch.Elapsed)}.");
        }

        /// <summary>
        /// Write the bounding box object to the file in big endian.
        /// （以大端序寫入定界框至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        public static void WriteBoxBE(BinaryWriter writer, Bounds3 bounds)
        {
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.min));
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.min));
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.max));
            writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.max));
        }

        /// <summary>
        /// Write the bounding box object to the file in little endian.
        /// （以小端序寫入定界框至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="bounds">The bounding box.（定界框。）</param>
        public static void WriteBoxLE(BinaryWriter writer, Bounds3 bounds)
        {
            writer.Write(BitConverter.GetBytes((double)bounds.x.min));
            writer.Write(BitConverter.GetBytes((double)bounds.y.min));
            writer.Write(BitConverter.GetBytes((double)bounds.x.max));
            writer.Write(BitConverter.GetBytes((double)bounds.y.max));
        }

        /// <summary>
        /// Write the geometry to the file in little endian.
        /// （以小端序寫入幾何圖形至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="id">The unique identifier of the feature.（圖徵的獨特識別碼。）</param>
        /// <param name="shape">The geometry shape of the feature.（圖徵的幾何形狀。）</param>
        /// <param name="geometry">The geometry of the feature.（圖徵的幾何圖形。）</param>
        /// <param name="indexPair">The index pair used in .shx file.（用於 .shx 檔案的索引對。）</param>
        /// <param name="bounds">The bounding box of the feature.（圖徵的定界框。）</param>
        public static void WriteGeometryLE(BinaryWriter writer, int id, int shape, Geometry geometry, out (int offset, int length) indexPair, out Bounds3 bounds)
        {
            int numParts = geometry.GetParts(out int numPoints, out List<int> pointCounts, out bounds);
            int offset = IOUtils.GetPosition(writer) / 2;
            int length = 0;

            writer.Write(IOUtils.GetFlippedBytes(id));              // Record number.（紀錄編號。）

            switch (shape)
            {
                case shapeTypePoint:
                    length = 14;
                    break;

                case shapeTypePolyLine:
                    length = 28 + 8 * numPoints;
                    break;

                case shapeTypePolygon:
                    length = 26 + 2 * numParts + 8 * (numPoints + numParts);
                    break;

                case shapeTypePointZ:
                    length = 22;
                    break;

                case shapeTypePolyLineZ:
                    length = 36 + 12 * numPoints;
                    break;

                case shapeTypePolygonZ:
                    length = 34 + 2 * numParts + 12 * (numPoints + numParts);
                    break;
            }

            indexPair = (offset, length);
            writer.Write(IOUtils.GetFlippedBytes(length));          // Content length.（內容長度。）
            writer.Write(BitConverter.GetBytes(shape));             // Shape type.（幾何形狀。）

            switch (shape)
            {
                case shapeTypePoint:
                    WritePointLE(writer, geometry.Inclusions[0][0], false);
                    return;

                case shapeTypePointZ:
                    WritePointLE(writer, geometry.Inclusions[0][0], true);
                    return;
            }
            
            WriteBoxLE(writer, bounds);                             // Box.（定界框。）
            writer.Write(BitConverter.GetBytes(numParts));          // NumParts.（部件的數量。）
            writer.Write(BitConverter.GetBytes(numPoints));         // NumPoints.（點的數量。）

            WritePointArraysLE(writer, geometry, pointCounts, (shape == shapeTypePolygon) || (shape == shapeTypePolygonZ), out List<double> zArray);

            if ((shape == shapeTypePolyLineZ) || (shape == shapeTypePolygonZ))
            {
                WriteZArrayLE(writer, bounds, zArray);
            }
        }

        /// <summary>
        /// Write the point array to the file in little endian.
        /// （以小端序寫入點陣列至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="geometry">The value waiting to be written.（等待被寫出的數值。）</param>
        /// <param name="pointCounts">The number of points in each part.（每個部件包含的點數。）</param>
        /// <param name="isRing">Whether the array represents a ring or not.（陣列是否為一個環？）</param>
        /// <param name="zArray">The array of z values.（Z值的陣列。）</param>
        public static void WritePointArraysLE(BinaryWriter writer, Geometry geometry, List<int> pointCounts, bool isRing, out List<double> zArray)
        {
            int pointIndex = 0;
            for (int i = 0; i < pointCounts.Count; i++)
            {
                writer.Write(BitConverter.GetBytes(pointIndex));
                pointIndex += pointCounts[i];
                if (isRing) pointIndex++;
            }

            zArray = new();
            for (int i = 0; i < geometry.Inclusions.Length; i++)
            {
                for (int j = 0; j < geometry.Inclusions[i].Length; j++)
                {
                    WritePointLE(writer, geometry.Inclusions[i][j], false);
                    zArray.Add(geometry.Inclusions[i][j].z);
                }
                if (isRing)
                {
                    WritePointLE(writer, geometry.Inclusions[i][0], false);
                    zArray.Add(geometry.Inclusions[i][0].z);
                }
            }

            for (int i = 0; i < geometry.Exclusions.Length; i++)
            {
                for (int j = 0; j < geometry.Exclusions[i].Length; j++)
                {
                    for (int k = 0; k < geometry.Exclusions[i][j].Length; k++)
                    {
                        WritePointLE(writer, geometry.Exclusions[i][j][k], false);
                        zArray.Add(geometry.Exclusions[i][j][k].z);
                    }
                    if (isRing)
                    {
                        WritePointLE(writer, geometry.Exclusions[i][j][0], false);
                        zArray.Add(geometry.Exclusions[i][j][0].z);
                    }
                }
            }
        }

        /// <summary>
        /// Write the point to the file in big endian.
        /// （以大端序寫入點至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The value waiting to be written.（等待被寫入的數值。）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        public static void WritePointBE(BinaryWriter writer, float3 value, bool writeElevation)
        {
            writer.Write(IOUtils.GetFlippedBytes((double)value.x));
            writer.Write(IOUtils.GetFlippedBytes((double)value.y));
            if (writeElevation)
            {
                writer.Write(IOUtils.GetFlippedBytes((double)value.z));
                IOUtils.SkipBytes(writer, 8);
            }
        }

        /// <summary>
        /// Write the point to the file in little endian.
        /// （以小端序寫入點至檔案中。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="value">The value waiting to be written.（等待被寫入的數值。）</param>
        /// <param name="writeElevation">Whether to write the elevation or not.（是否要寫出高程？）</param>
        public static void WritePointLE(BinaryWriter writer, float3 value, bool writeElevation)
        {
            writer.Write(BitConverter.GetBytes((double)value.x));
            writer.Write(BitConverter.GetBytes((double)value.y));
            if (writeElevation)
            {
                writer.Write(BitConverter.GetBytes((double)value.z));
                IOUtils.SkipBytes(writer, 8);
            }
        }

        /// <summary>
        /// Write the file header for .shp files.
        /// （寫入 .shp 檔案的標頭。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="shape">The geometry shape of the feature.（圖徵的幾何形狀。）</param>
        public static void WriteSHPHeader(BinaryWriter writer, int shape)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(IOUtils.GetFlippedBytes(9994));
                IOUtils.SkipBytes(writer, 24);
                writer.Write(BitConverter.GetBytes(1000));
                writer.Write(BitConverter.GetBytes(shape));
            }
            else
            {
                writer.Write(BitConverter.GetBytes(9994));
                IOUtils.SkipBytes(writer, 24);
                writer.Write(IOUtils.GetFlippedBytes(1000));
                writer.Write(IOUtils.GetFlippedBytes(shape));
            }

            IOUtils.SkipBytes(writer, 64);
        }

        /// <summary>
        /// Write the .shx file.
        /// （寫入 .shx 檔案。）
        /// </summary>
        /// <param name="writer">Current file's writer.（目前檔案的寫入者。）</param>
        /// <param name="shape">The geometry shape of the feature.（圖徵的幾何形狀。）</param>
        /// <param name="bounds">The bounding box of the features.（圖徵的定界框。）</param>
        /// <param name="indexPairs">The index pairs indicating the offset and the length of each record.（顯示每個紀錄偏移與長度的索引對。）</param>
        public static void WriteSHX(BinaryWriter writer, int shape, Bounds3 bounds, List<(int offset, int length)> indexPairs)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(IOUtils.GetFlippedBytes(9994));
                IOUtils.SkipBytes(writer, 20);
                writer.Write(IOUtils.GetFlippedBytes(50 + indexPairs.Count * 4));
                writer.Write(BitConverter.GetBytes(1000));
                writer.Write(BitConverter.GetBytes(shape));
                writer.Write(BitConverter.GetBytes((double)bounds.x.min));
                writer.Write(BitConverter.GetBytes((double)bounds.y.min));
                writer.Write(BitConverter.GetBytes((double)bounds.x.max));
                writer.Write(BitConverter.GetBytes((double)bounds.y.max));
            }
            else
            {
                writer.Write(BitConverter.GetBytes(9994));
                IOUtils.SkipBytes(writer, 20);
                writer.Write(BitConverter.GetBytes(50 + indexPairs.Count * 4));
                writer.Write(IOUtils.GetFlippedBytes(1000));
                writer.Write(IOUtils.GetFlippedBytes(shape));
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.min));
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.min));
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.x.max));
                writer.Write(IOUtils.GetFlippedBytes((double)bounds.y.max));
            }

            if ((shape == shapeTypePointZ) || (shape == shapeTypePolyLineZ) || (shape == shapeTypePolygonZ))
            {
                if (BitConverter.IsLittleEndian)
                {
                    writer.Write(BitConverter.GetBytes((double)bounds.z.min));
                    writer.Write(BitConverter.GetBytes((double)bounds.z.max));
                }
                else
                {
                    writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.min));
                    writer.Write(IOUtils.GetFlippedBytes((double)bounds.z.max));
                }

                IOUtils.SkipBytes(writer, 16);
            }
            else
            {
                IOUtils.SkipBytes(writer, 32);
            }

            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < indexPairs.Count; i++)
                {
                    writer.Write(IOUtils.GetFlippedBytes(indexPairs[i].offset));
                    writer.Write(IOUtils.GetFlippedBytes(indexPairs[i].length));
                }
            }
            else
            {
                for (int i = 0; i < indexPairs.Count; i++)
                {
                    writer.Write(BitConverter.GetBytes(indexPairs[i].offset));
                    writer.Write(BitConverter.GetBytes(indexPairs[i].length));
                }
            }
        }


        public static void WriteZArrayLE(BinaryWriter writer, Bounds3 bounds, List<double> zArray)
        {
            writer.Write(BitConverter.GetBytes(bounds.z.min));
            writer.Write(BitConverter.GetBytes(bounds.z.max));
            for (int i = 0; i < zArray.Count; i++)
            {
                writer.Write(BitConverter.GetBytes(zArray[i]));
            }
        }
    }
}