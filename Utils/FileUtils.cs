namespace Carto.Utils
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// A class to provide easy interface to write data.
    /// （一個提供寫出資料的簡易介面的類別。）
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// The constructor of Carto.Utils.FileUtils.
        /// （Carto.Utils.FileUtils 的建構函式。）
        /// </summary>
        public FileUtils(string path)
        {
            Length = 0;
            Path = path;
        }

        /// <summary>
        /// The buffer size of the StreamWriter.
        /// （StreamWriter 的緩衝區大小。）
        /// </summary>
        private const int BufferSize = 65536;

        /// <summary>
        /// The length of the file in bytes.
        /// （檔案長度；須注意不可混用二進位資料與文字，以免長度計算出錯。）
        /// </summary>
        public int Length;

        /// <summary>
        /// The path to the file.
        /// （檔案的路徑。）
        /// </summary>
        private readonly string Path;

        /// <summary>
        /// Append the byte array into the file.
        /// （將位元組陣列添加在檔案結尾。）
        /// </summary>
        /// <param name="bytes">The byte array requiring being appended to the file. （要求被添加於檔案結尾的位元組陣列。）</param>
        public void Append(byte[] bytes)
        {
            using (FileStream fs = new FileStream(Path, FileMode.Append, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            Length += bytes.Length;
        }

        /// <summary>
        /// Append the byte array into the file.
        /// （將位元組陣列添加在檔案結尾。）
        /// </summary>
        /// <param name="bytes">The byte array requiring being appended to the file. （要求被添加於檔案結尾的位元組陣列。）</param>
        /// <param name="index">The index of the first appended byte, （第一個被添加的位元組的索引。）</param>
        public void Append(byte[] bytes, out int index)
        {
            index = Length;
            
            using (FileStream fs = new FileStream(Path, FileMode.Append, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            Length += bytes.Length;
        }

        /// <summary>
        /// Append the text into the file.
        /// （將文字添加在檔案結尾。）
        /// </summary>
        /// <param name="text">The text requiring being appended to the file. （要求被添加於檔案結尾的文字。）</param>
        /// <param name="encoding">The encoding style of the text; default to be UTF-8.（文字的編碼方式；預設為UTF-8。） </param>
        public void Append(string text, Encoding encoding = null)
        {
            Encoding _enc = (encoding is null) ? Encoding.UTF8 : encoding;
            File.AppendAllText(Path, text, _enc);
            Length += text.Length;
        }

        /// <summary>
        /// Append the text into the file and add a newline character at the end of the text.
        /// （將文字添加在檔案結尾，並且在文字的結尾加入換行字元。）
        /// </summary>
        /// <param name="text">The text requiring being appended to the file. （要求被添加於檔案結尾的文字。）</param>
        /// <param name="encoding">The encoding style of the text; default to be UTF-8.（文字的編碼方式；預設為UTF-8。） </param>
        public void AppendLine(string text, Encoding encoding = null)
        {
            Encoding _enc = (encoding is null) ? Encoding.UTF8 : encoding;
            File.AppendAllText(Path, $"{text}\n", _enc);
            Length += text.Length;
        }

        /// <summary>
        /// Clear the contents of the file.
        /// （清空檔案的內容。）
        /// </summary>
        public void Clear()
        {
            Length = 0;
            File.WriteAllText(Path, string.Empty);
        }

        /// <summary>
        /// Recalculate the length of the bytes in the file.
        /// （重新計算檔案中的位元組長度。）
        /// </summary>
        /// <returns>The byte length. （位元組的長度。）</returns>
        public int GetByteLength()
        {
            Length = 0;

            using (Stream stream = File.OpenRead(Path))
            {
                int bytesRead;
                while ((bytesRead = stream.Read(new byte[BufferSize], 0, BufferSize)) > 0) {}
                Length = bytesRead;
            }

            return Length;
        }

        /// <summary>
        /// Recalculate the length of the text in the file.
        /// （重新計算檔案中的文字長度。）
        /// </summary>
        /// <param name="encoding">The encoding style of the text; default to be UTF-8.（文字的編碼方式；預設為UTF-8。）</param>
        /// <returns>The text length. （文字的長度。）</returns>
        public int GetTextLength(Encoding encoding = null)
        {
            Length = 0;
            Encoding _enc = (encoding is null) ? Encoding.UTF8 : encoding;

            using (StreamReader sr = new StreamReader(Path, _enc))
            {
                string line = sr.ReadLine();
                Length += line.Length;
            }

            return Length;
        }

        /// <summary>
        /// Replace the data in the file from the given index.
        /// （替換掉二進位檔案內指定位置的資料。）
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void ReplaceBytes(int index, object item)
        {
            byte[] buffer;

            switch (item)
            {
                case double @double:
                    buffer = BitConverter.GetBytes(@double);
                    break;

                case float @float:
                    buffer = BitConverter.GetBytes(@float);
                    break;

                case int @int:
                    buffer = BitConverter.GetBytes(@int);
                    break;

                case long @long:
                    buffer = BitConverter.GetBytes(@long);
                    break;

                case short @short:
                    buffer = BitConverter.GetBytes(@short);
                    break;

                case string @string:
                    buffer = Encoding.UTF8.GetBytes(@string);
                    break;

                default:
                    buffer = BitConverter.GetBytes(0);
                    break;
            }

            if ((index < Length - 1) && (index + buffer.Length <= Length - 1))
            {
                using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite))
                {
                    fs.Seek(index, SeekOrigin.Begin);
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            else
            {
                throw new ArgumentException($"The index {index} is out of range (0 ~ {Length - 1}) or the item `{item}` has too many bytes. 索引值{index}超出範圍（0 ～ {Length - 1}）或替換項目 `{item}` 位元組數量過多。");
            }
        }
    }
}