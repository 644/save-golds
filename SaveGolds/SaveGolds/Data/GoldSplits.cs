using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SaveGolds.Data
{
    public struct GoldSplitSet
    {
        public string GameName { get; set; }
        public string CategoryName { get; set; }
        public List<GoldSplit> GoldSplits { get; set; }

        public string FileName =>
            $"{ReplaceUnsupportedCharacters(GameName)}_{ReplaceUnsupportedCharacters(CategoryName)}.json";

        private string ReplaceUnsupportedCharacters(string s)
        {
            foreach (char c in Path.GetInvalidFileNameChars()) s = s.Replace(c.ToString(), "");
            foreach (char c in Path.GetInvalidPathChars()) s = s.Replace(c.ToString(), "");
            return s;
        }
    }

    public struct GoldSplit
    {
        public string SplitName { get; set; }
        public TimeSpan RealTime { get; set; }
        public TimeSpan GameTime { get; set; }
    }

    public static class GoldSplitSetManager
    {
        public static GoldSplitSet LoadGoldSplitSet(string configPath)
        {
            StreamReader streamReader = File.OpenText(configPath);
            string splitSetRaw = streamReader.ReadToEnd();
            return JsonSerializer.Deserialize<GoldSplitSet>(splitSetRaw);
        }

        public static string WriteGoldSplitSet(GoldSplitSet goldSplitSet)
        {
            string splitSetRaw = JsonSerializer.Serialize(goldSplitSet);
            string filePath = $"./Config/{goldSplitSet.FileName}";
            File.WriteAllText(filePath, splitSetRaw);
            return filePath; 
        }
    }
}
