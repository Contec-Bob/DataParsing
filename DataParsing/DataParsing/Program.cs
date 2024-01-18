using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace DataParsing
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dateNow = DateTime.UtcNow;
            string savedPath = @"json\"+dateNow.ToString("yyyyMMdd")+ ".json";

            //json 폴더의 디렉토리 읽어서 오늘 생성된 파일 있는지 확인
            //string folderPath = "json";
            //bool fileExit = false;
            //DirectoryInfo dir = new DirectoryInfo(folderPath);
            //foreach(FileInfo file in dir.GetFiles())
            //{
            //    if(file.CreationTime.ToString("yyyyMMdd") == dateNow.ToString("yyyyMMdd"))
            //        fileExit = true;
            //}

            //if(fileExit)
            //{
            //    string savedJson = File.ReadAllText(savedPath);
            //    Console.WriteLine(savedJson);
            //}

            if (File.Exists(savedPath))
            {
                string savedJson = File.ReadAllText(savedPath);
                Console.WriteLine(savedJson);
            }

            else
            {

                string path = "https://celestrak.org/SpaceData/EOP-All.txt";

                using WebClient client = new WebClient();

                using Stream stream = client.OpenRead(path);

                using StreamReader reader = new StreamReader(stream);

                string spaceData = reader.ReadToEnd();

                int index = spaceData.IndexOf(dateNow.ToString("yyyy MM dd"));
                string[] todayData = spaceData.Substring(index, 104).Split(" ", StringSplitOptions.RemoveEmptyEntries);

                /*문자열 배열
                 0 : 4자리 년도
                1 : 2자리 월
                2 : 일
                3 : MJD
                4 : x
                5 : y
                6 : UT1-UTC
                7 : LOD
                8 : dPsi
                9 : dEpsilon
                10 : dX
                11 : dY
                12 : DAT*/

                Data newData = new Data
                {
                    date = dateNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    utcToUt1Ms = double.Parse(todayData[6]),
                    xpolarS = double.Parse(todayData[4]),
                    ypolarS = double.Parse(todayData[5]),
                    lodMs = double.Parse(todayData[7])
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string savedJson = JsonSerializer.Serialize(newData, options);
                File.WriteAllText(savedPath, savedJson);
            }
        }
    }
}