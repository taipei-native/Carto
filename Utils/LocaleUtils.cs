using Colossal;
using Colossal.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Carto.Utils
{
    /// <summary>
    /// The class that provides utility functions to localize the user interface.
    /// （提供翻譯使用者介面相關功能的類別。）
    /// </summary>
    public static class LocaleUtils
    {
        /// <summary>
        /// The class that stores the locale entries.
        /// （儲存語系檔案條目的類別。）
        /// </summary>
        public class Locale : IDictionarySource
        {
            /// <summary>
            /// The dictionary that contains the string pairs of the locale id and its translation.
            /// （含有語系檔案代號及其翻譯的字串對的字典。）
            /// </summary>
            private readonly Dictionary<string, string> _entries;

            public Locale(Dictionary<string, string> entries) => _entries = entries;

            /// <summary>
            /// Retrieve the locale data entries.
            /// （獲得語系檔案條目。）
            /// </summary>
            /// <param name="errors">The error encountered during reading the entries.（讀取條目時遭遇的錯誤。）</param>
            /// <param name="indexCounts">Unknown. Probably related to the index in the provided dictionary.（未知。或許和提供字典的索引值有關。）</param>
            /// <returns>The locale entries.（語系檔案條目。）</returns>
            public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) => _entries;

            /// <summary>
            /// Unload the locale source. As of the version 1.2.3f1, CO doesn't implement this method in the template, <see cref="CSVFileSource"/> or <see cref="MemorySource"/>.<br/>
            /// （解除載入語系檔案。截至 1.2.3f1 版本，CO 並沒有在模板中、<see cref="CSVFileSource"/> 或 <see cref="MemorySource"/> 實作這個方法。）
            /// </summary>
            public void Unload() { }
        }

        /// <summary>
        /// The custom JSON converter to read abbreviated locales.
        /// （用於讀取已縮寫語系檔案的客製化 JSON 轉換器。）
        /// </summary>
        public class LocaleConverter : JsonConverter<Dictionary<string, string>>
        {
            /// <summary>
            /// The common prefix of the locales ids.
            /// （語系檔案識別碼的共同前綴。）
            /// </summary>
            private readonly string _prefix;

            public LocaleConverter(string prefix) => _prefix = prefix;

            /// <summary>
            /// Read the JSON file.
            /// （讀取 JSON 檔案。）
            /// </summary>
            /// <param name="reader">The current file's reader.（目前檔案的讀取者。）</param>
            /// <param name="objectType">The type to convert into.（欲轉換的型別。）</param>
            /// <param name="existingValue">The existing object to write into.（可寫入的既有物件。）</param>
            /// <param name="hasExistingValue">Whether <paramref name="existingValue"/> exists or not.（<paramref name="existingValue"/> 是否存在？）</param>
            /// <param name="serializer">The custom JSON serializer.（客製化的 JSON 序列化類別。）</param>
            /// <returns>The deserialized dictionary.（反序列化的字典。）</returns>
            public override Dictionary<string, string> ReadJson(JsonReader reader, Type objectType, Dictionary<string, string> existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                Dictionary<string, string> locales = new();
                JObject jObject = JObject.Load(reader);
                IEnumerator<JProperty> enumerator = jObject.Properties().GetEnumerator();
                while (enumerator.MoveNext())
                {
                    JProperty property = enumerator.Current;
                    locales.Add(ExpandLocaleId(property.Name), property.Value?.ToString() ?? string.Empty);
                }

                return locales;
            }

            /// <summary>
            /// Write the JSON file.
            /// （寫入 JSON 檔案。）
            /// </summary>
            /// <param name="writer">The current file's writer.（目前檔案的寫入者。）</param>
            /// <param name="value">The dictionary to serialize.（欲序列化的字典。）</param>
            /// <param name="serializer">The custom JSON serializer.（客製化的 JSON 序列化類別。）</param>
            /// <exception cref="NotImplementedException"></exception>
            public override void WriteJson(JsonWriter writer, Dictionary<string, string> value, JsonSerializer serializer) => throw new NotImplementedException();

            /// <summary>
            /// Expand the abbreviated locale ids.
            /// （擴展被縮寫的語系檔案條目識別碼。）
            /// </summary>
            /// <param name="localeID">The input id.（輸入的識別碼。）</param>
            /// <returns>The expanded id.（擴展的識別碼。）</returns>
            private string ExpandLocaleId(string localeID)
            {
                bool isDescription = localeID.StartsWith("Options.Description", StringComparison.Ordinal);
                bool isOption = localeID.StartsWith("Options.Option", StringComparison.Ordinal);
                bool isDropdown = localeID.StartsWith("Options.Dropdown", StringComparison.Ordinal);
                bool isGroup = localeID.StartsWith("Options.Group", StringComparison.Ordinal);
                bool isSection = localeID.StartsWith("Options.Section", StringComparison.Ordinal);
                bool isTab = localeID.StartsWith("Options.Tab", StringComparison.Ordinal);
                bool isWarning = localeID.StartsWith("Options.Warning", StringComparison.Ordinal);
                Match match = Regex.Match(localeID, @"(?<=Options\.(?:Description|Dropdown|Group|Option|Section|Tab|Warning)\.).+");
                if (!match.Success) return localeID;

                string suffix = match.Value;
                string content;

                if (suffix == nameof(Carto))
                {
                    content = _prefix;
                }
                else if (isDescription || isOption || isWarning)
                {
                    content = $"{_prefix}.{nameof(Settings)}.{suffix}";
                }
                else if (isDropdown)
                {
                    Match enumMatch = Regex.Match(localeID, @"(?<=Options\.Dropdown\.).+(?=\[)");
                    Match enumValueMatch = Regex.Match(localeID, @"(?<=\[).+(?=\])");
                    if (enumMatch.Success && enumValueMatch.Success)
                    {
                        string enumName = enumMatch.Value.ToUpperInvariant();
                        string enumValueName = enumValueMatch.Value;
                        return $"Options.{_prefix}.{enumName}[{enumValueName}]";
                    }

                    content = $"{_prefix}.{suffix}";
                }
                else
                {
                    content = $"{_prefix}.{suffix}";
                }

                if (isDescription) return $"Options.OPTION_DESCRIPTION[{content}]";
                if (isGroup) return $"Options.GROUP[{content}]";
                if (isOption) return $"Options.OPTION[{content}]";
                if (isSection) return $"Options.SECTION[{content}]";
                if (isTab) return $"Options.TAB[{content}]";
                if (isWarning) return $"Options.WARNING[{content}]";
                return localeID ;
            }
        }

        /// <summary>
        /// Load the available locales.
        /// （載入可使用的語系檔案。）
        /// </summary>
        public static void Load()
        {
            string[] resourceNames = Instance.Assembly.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; i++)
            {
                string languageID = Path.GetFileNameWithoutExtension(resourceNames[i]).Split('.').Last();
                string resourceName = resourceNames[i];
                if (Path.GetExtension(resourceName) == ".json")
                {
                    _ = TryLoadLocaleFromResource(languageID, resourceName);
                }
            }
        }

        /// <summary>
        /// Retrieve the translation of the provided <paramref name="localeID"/>.
        /// （獲得 <paramref name="localeID"/> 的翻譯。）
        /// </summary>
        /// <param name="localeID">The unique identifier of the locale entry.（語系檔案條目的唯一識別碼。）</param>
        /// <returns>The translation of the locale key.（語系檔案鍵值的翻譯。）</returns>
        public static string Translate(string localeID)
        {
            _ = TryTranslate(localeID, out string localeString);
            return localeString;
        }

        /// <summary>
        /// Try to load the locale from the designated embedded JSON resource.
        /// （嘗試從指定的嵌入 JSON 資源中載入語系檔案。）
        /// </summary>
        /// <param name="languageID">The unique identifier of the language.（語言的唯一代碼。）</param>
        /// <param name="resourceName">The title of the embedded JSON file.（嵌入 JSON 資源的名稱。）</param>
        /// <returns>Whether the attempt successes or not.（嘗試是否成功。）</returns>
        private static bool TryLoadLocaleFromResource(string languageID, string resourceName)
        {
            if (!Instance.Localization.SupportsLocale(languageID)) return false;
            Instance.Localization.AddSource(languageID, new Locale(IOUtils.GetJsonResource(resourceName, new LocaleConverter("Carto.Carto.Mod"))));
            Instance.Log.Debug($"Successfully add the {languageID} locale. 成功添加 {languageID} 語系檔案。");
            return true;
        }

        /// <summary>
        /// Check whether there's a locale entry named after the provided <paramref name="localeID"/>.
        /// （確認是否有語系檔案條目以 <paramref name="localeID"/> 為名。）
        /// </summary>
        /// <param name="localeID">The unique identifier of the locale entry.（語系檔案條目的唯一識別碼。）</param>
        /// <param name="localeString">The translation of the locale key.（語系檔案鍵值的翻譯。）</param>
        /// <returns>Whether there's an entry named after the provided localeID.（是否有）</returns>
        public static bool TryTranslate(string localeID, out string localeString)
        {
            bool status = Instance.Localization.activeDictionary.TryGetValue(localeID, out localeString);
            if (!status) localeString = localeID;   // The fallback value.（後備值。）
            return status;
        }
    }
}