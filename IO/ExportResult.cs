namespace Carto.IO
{
    /// <summary>
    /// The result returned by <see cref="IO.Export(Options)"/>.
    /// （<see cref="IO.Export(Options)"/> 的回傳結果。）<br/>
    /// Errors are reported via <see cref="ErrorMessage"/> rather than modal dialogs
    /// (unless the caller's <see cref="Options.CompletionDialog"/> opts back in). <br/>
    /// （錯誤透過 <see cref="ErrorMessage"/> 回報，除非呼叫端的
    /// <see cref="Options.CompletionDialog"/> 主動啟用，否則不會彈出對話框。）
    /// </summary>
    public class ExportResult
    {
        /// <summary>
        /// Whether the export completed without throwing.
        /// （輸出是否未拋出例外而完成。）
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Absolute paths of files written during the export.
        /// （輸出過程中寫入的檔案絕對路徑。）<br/>
        /// Empty array when nothing was written.
        /// （未寫入任何檔案時為空陣列。）
        /// </summary>
        public string[] FilesWritten { get; set; } = new string[0];

        /// <summary>
        /// Diagnostic message on failure. Null when <see cref="Success"/> is true.
        /// （失敗時的診斷訊息。<see cref="Success"/> 為真值時，此屬性為 null。）
        /// </summary>
        public string ErrorMessage { get; set; }

        public Error ErrorType { get; set; }
    }
}
