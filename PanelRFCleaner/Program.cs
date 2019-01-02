using System.IO;
using System.Text.RegularExpressions;

namespace PanelRFCleaner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (FileInfo f in root.GetFiles())
            {
                if (f.Extension.Equals(".log"))
                {
                    string file = f.FullName;
                    string newFile = 
                        Path.Combine(f.DirectoryName, Path.GetFileNameWithoutExtension(f.FullName) + ".csv");
                    int ii = 1;
                    using (StreamReader sr = new StreamReader(file))
                    {
                        using (StreamWriter sw = new StreamWriter(newFile))
                        {
                            string line = null;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (ii == 1 || (!line.Contains("False") && !line.Contains("MeasureNTC")))
                                {
                                    string newLine = line.Replace("True>", "").Replace("openRelay", "");
                                    newLine = Regex.Replace(newLine, @"\[\S+\]", "");
                                    newLine = Regex.Replace(newLine, @"Wait[0-9]+", "");
                                    newLine = newLine.Replace("\t", ",");
                                    sw.WriteLine(newLine);
                                    ii++;
                                }
                            }
                        }
                    }
                    if (ii - 1 == 1)
                    {
                        File.Delete(newFile);
                    }
                }
            }
        }
    }
}
