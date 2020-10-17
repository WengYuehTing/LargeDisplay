using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BodySee.Tools
{
    class YTFileReader
    {
        public static void Read(string path)
        {
            StreamReader reader = new StreamReader(path);
            string line;
            while((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        public static List<string> GetFileContent(string path)
        {
            StreamReader reader = new StreamReader(path);
            List<string> list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                list.Add(line);
            }
            return list;
        }


    }
}
