namespace Carto.Utils
{
    using Colossal;
    using Colossal.Json;
    using Colossal.Localization;
    using Colossal.Logging;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A class to manage locale data.
    /// （一個管理語系檔案的類別。）
    /// </summary>
    public class LocaleUtils
    {
        private static readonly LocalizationManager m_Localization = Instance.Localization;
        private static readonly ILog m_Log = Instance.Log;
        
        /// <summary>
        /// The class storing the locale entry data.
        /// （儲存語系檔案條目的類別。）
        /// </summary>
        public class Locale : IDictionarySource
        {
            /// <summary>
            /// The dictionary containing the string pairs of string id and its translations.
            /// （儲存語系檔案代號與翻譯字串配對的字典。）
            /// </summary>
            private readonly Dictionary<string, string> m_Dictionary;

            /// <summary>
            /// The constructor of Carto.Utils.LocaleUtils.Locale.
            /// （Carto.Utils.LocaleUtils.Locale 的建構函式）
            /// </summary>
            /// <param name="data">The dictionary of the locale entry data.�]�y�t�ɮ׼ƾڪ��r��C�^</param>
            public Locale(Dictionary<string, string> data)
            {
                m_Dictionary = data;
            }

            /// <summary>
            /// The locale entry data.
            /// （語系檔案條目。）
            /// </summary>
            public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
            {
                return m_Dictionary;
            }

            /// <summary>
            /// The method to unload the dictionary data.
            /// （用以卸載字典的方法。）
            /// </summary>
            public void Unload() { }
        }

        /// <summary>
        /// Append a new locale entry (JSON resource) to the exisitng designated locale.
        /// （添加新的資料（JSON資源）至既有語系檔案。）
        /// </summary>
        /// <param name="language">Language code defined by Colossal Order.（由 Colossal Order 定義的語言代碼。）</param>
        /// <param name="resource">The complete name of the locale resource.（語系檔案資源的完整名稱。）</param>
        public static void AddLocaleFromEmbedded(string language, string resource)
        {
            Dictionary<string, string> localeEntry = JSON.MakeInto<Dictionary<string, string>>(JSON.Load(string.Join("\n", MiscUtils.ReadAllLines(resource))));
            AddLocaleFromSource(language, localeEntry);
        }

        /// <summary>
        /// Append a new locale entry (JSON file) to the exisitng designated locale.
        /// （添加新的資料（JSON檔案）至既有語系檔案。）
        /// </summary>
        /// <param name="language">Language code defined by Colossal Order.（由 Colossal Order 定義的語言代碼。）</param>
        /// <param name="path">The path to the locale file.（指向語系檔案的連結。）</param>
        public static void AddLocaleFromFile(string language, string path)
        {
            Dictionary<string, string> localeEntry = JSON.MakeInto<Dictionary<string, string>>(JSON.Load(string.Join("\n", File.ReadAllLines(path))));
            AddLocaleFromSource(language, localeEntry);
        }

        /// <summary>
        /// Append a new locale entry (dictionary) to the exisitng designated locale.
        /// （添加新的資料（字典）至既有語系檔案。）
        /// </summary>
        /// <param name="language">Language code defined by Colossal Order.（由 Colossal Order 定義的語言代碼。）</param>
        /// <param name="localeEntry">The locale entries.（語系檔案字典。）</param>
        public static void AddLocaleFromSource(string language, Dictionary<string, string> localeEntry)
        {
            m_Localization.AddSource(language, new Locale(localeEntry));
            m_Log.Debug($"Successfully add the {language} locale; 成功添加 {language} 語系檔案。");
        }

        /// <summary>
        /// Batch add locale entry files to the exising locales.
        /// （批次添加資料至既有語系檔案。）
        /// </summary>
        public static void BatchAddLocaleFromDirectory()
        {
            string path = MiscUtils.CombinePath(Setting.ExecuteFolder, "Locale");
            BatchAddLocaleFromDirectory(path);
        }

        /// <summary>
        /// Batch add locale entry files from given directory to the exising locales.
        /// （批次添加指定目錄內的檔案至既有語系檔案。）
        /// </summary>
        /// <param name="path">The path to the directory.（指向目錄的連結。）</param>
        public static void BatchAddLocaleFromDirectory(string path)
        {
            string[] localeArray = m_Localization.GetSupportedLocales();

            foreach (string filePath in MiscUtils.GetFiles(path))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                if ((localeArray.Contains(fileName)) & (Path.GetExtension(filePath) == ".json"))
                {
                    try
                    {
                        AddLocaleFromFile(fileName, filePath);
                    }
                    catch
                    {
                        m_Log.Warn($"Cannot add the locale ({fileName}); �L�k�K�[�y�t�ɮס]{fileName}�^");
                    }
                }
            }
        }

        /// <summary>
        /// Batch add locale entry files from embedded resources to the exising locales.
        /// （批次添加內嵌資源至既有語系檔案。）
        /// </summary>
        public static void BatchAddLocaleFromEmbedded()
        {
            string[] localeArray = m_Localization.GetSupportedLocales();
            string[] resourceArray = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            foreach (string resource in resourceArray)
            {
                string resourceName = Path.GetFileNameWithoutExtension(resource).Split('.').Last();

                if ((localeArray.Contains(resourceName)) & (Path.GetExtension(resource) == ".json"))
                {
                    try
                    {
                        AddLocaleFromEmbedded(resourceName, resource);
                    }
                    catch
                    {
                        m_Log.Warn($"Cannot add the locale ({resourceName}); �L�k�K�[�y�t�ɮס]{resourceName}�^");
                    }
                }
            }
        }

        /// <summary>
        /// Turn the locale identifier key into the translated string.
        /// （將語系檔案的識別鍵值轉換成翻譯字串。）
        /// </summary>
        /// <param name="identifier">The locale key.（語系檔案的鍵值）</param>
        public static string Translate(string identifier)
        {
            _ = TryTranslate(identifier, out string translated);
            return translated;
        }

        /// <summary>
        /// Try to turn the locale identifier key into the translated string.
        /// （嘗試將語系檔案的識別鍵值轉換成翻譯字串。）
        /// </summary>
        /// <param name="identifier">The locale key.（語系檔案的鍵值）</param>
        /// <param name="translated">The translated string.（翻譯後的字串。）</param>
        public static bool TryTranslate(string identifier, out string translated)
        {
            bool status = m_Localization.activeDictionary.TryGetValue(identifier, out string value);
            translated = status ? value : identifier;
            return status;
        }
    }
}