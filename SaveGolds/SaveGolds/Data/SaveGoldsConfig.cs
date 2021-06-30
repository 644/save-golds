using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SaveGolds.Data
{
    public struct SaveGoldsConfig
	{
        public string RecordingPath { get; set; }
        public string RecordingFormat { get; set; }
        public bool MakeBackups { get; set; }
        public Dictionary<string, string> SplitConfigs { get; set; }
    }

    public class SaveGoldsConfigManager
    {
        private SaveGoldsConfig Config;

        public SaveGoldsConfigManager()
        {
            if(!File.Exists("./Config/config.json"))
            {
                Config = new SaveGoldsConfig {
                    RecordingPath = "",
                    RecordingFormat = "mkv",
                    MakeBackups = false,
                    SplitConfigs = new Dictionary<string, string>()
                };

                if (!Directory.Exists("./Config")) 
                {
                    Directory.CreateDirectory("./Config");
                }
                WriteToConfigFile();
            }
            else
            {
                using (StreamReader streamReader = File.OpenText("./Config/config.json"))
                {
                    string configRaw = streamReader.ReadToEnd();
                    Config = JsonSerializer.Deserialize<SaveGoldsConfig>(configRaw);
                }
            }
        }

        public void RegisterSplits(string splitFilePath)
        {
            GoldSplitSet goldSplitSet = SplitFileParser.ParseSplitFile(splitFilePath);
            string gameConfigPath = GoldSplitSetManager.WriteGoldSplitSet(goldSplitSet);
            string gameKey = $"{goldSplitSet.GameName} - {goldSplitSet.CategoryName}";
            if (Config.SplitConfigs.ContainsKey(gameKey))
            {
                throw new GameExistsException(gameKey);
            }
            Config.SplitConfigs.Add(gameKey, gameConfigPath);
            WriteToConfigFile();
        }

        public void DeleteRegisteredSplits(string gameKey)
        {
            if (Config.SplitConfigs.TryGetValue(gameKey, out string deletedSplitsPath))
            {
                Config.SplitConfigs.Remove(gameKey);
                File.Delete(deletedSplitsPath);
                WriteToConfigFile();
            }
        }

        public List<string> GetRegisteredSplits() =>
            Config.SplitConfigs.Keys.ToList();

        public string GetSplitConfigPath(string game) =>
            Config.SplitConfigs[game];

        private void WriteToConfigFile() =>
            File.WriteAllText("./Config/config.json", JsonSerializer.Serialize(Config));
    }

    public class GameExistsException : Exception
    {
        public GameExistsException(string gameKey) :
            base(
            $"{gameKey} already has a configuration. Please delete that configuration first if you'd like to register a different split file."
            )
        { }
    }
}
