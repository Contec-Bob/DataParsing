using System.IO;
using System.Net;
using System.Text.Json;
using System;

namespace DataParsing
{
    public class Eop
    {
        private const string txtPath = "https://celestrak.org/SpaceData/EOP-All.txt";
        private DateTime dateNow;
        private string savedPath;

        public Eop()
        {
            dateNow = DateTime.UtcNow;
            savedPath = @"json\" + dateNow.ToString("yyyyMMdd") + ".json";
        }

        public bool JsonIsExist()
        {
            if(dateNow != DateTime.UtcNow)
            {
                dateNow = DateTime.UtcNow;
                savedPath = @"json\" + dateNow.ToString("yyyyMMdd") + ".json";
            }

            if (File.Exists(savedPath))
                return true;
            return false;
        }

        public void JsonToString()
        {
            string savedJson = File.ReadAllText(savedPath);
            Console.WriteLine(savedJson);
        }

        public void SaveToJson()
        {
            using WebClient client = new WebClient();

            using Stream stream = client.OpenRead(txtPath);

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