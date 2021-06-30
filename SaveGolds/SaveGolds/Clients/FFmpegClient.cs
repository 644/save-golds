using System;
using System.Diagnostics;

namespace SaveGolds.Clients
{
    public class FFmpegClient
    {
        public string CutVid(string inputPath, string outPath, int copyTime)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\Users\blackcat\Desktop\ffmpeg-4.3.1-2021-01-01-full_build\bin\ffmpeg";
            proc.StartInfo.Arguments = "-i " + inputPath + " -c copy " + outPath;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;

            if(!proc.Start()) return "Error starting ffmpeg. Make sure the path is correct";

            proc.Close();

            return "Finished cropping";
        }
    }
}
