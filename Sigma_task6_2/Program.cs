using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sigma_task6_2
{
    class IP
    {
        public string ip { get; set; }
        public DateTime time { get; set; }
        public DayOfWeek day { get; set; }

        public IP(string ip, DateTime time, DayOfWeek day)
        {
            this.ip = ip;
            this.time = time;
            this.day = day;
        }
        public override string ToString()
        {
            string print = "";
            print += ip;
            print += " " + time.ToString(@"HH\:mm\:ss");
            print += " " + day.ToString();
            return print;
        }
    }
    class Site
    {
        IP[] IPs;
        public void ReadFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            var lineCount = File.ReadLines(path).Count();
            string line = reader.ReadLine();
            string[] lineSplit;

            IP[] fileIPs = new IP[lineCount];
            string ip;
            DateTime time;
            DayOfWeek day;
            for (int i = 0; i < lineCount; ++i)
            {
                try
                {
                    lineSplit = line.Split();
                    ip = lineSplit[0];
                    if (!DateTime.TryParse(lineSplit[1], out time))
                        throw new FormatException("Wrong file format");
                    if (!Enum.IsDefined(typeof(DayOfWeek), lineSplit[2]))
                        throw new FormatException("Wrong file format");
                    day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), lineSplit[2], true);
                    fileIPs[i] = new IP(ip, time, day);
                }
                catch(FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                line = reader.ReadLine();
            }
            IPs = fileIPs;
        }
        public string FindAmountVisitorsIPs()
        {
            string print = "Visitors for each IP:\n";
            Dictionary<string, int> listCounter = new Dictionary<string, int>();
            for(int i = 0; i < IPs.Length; ++i)
                if(!listCounter.ContainsKey(IPs[i].ip))
                    listCounter.Add(IPs[i].ip, 1);
                else listCounter[IPs[i].ip]++;

            foreach (var item in listCounter)
                print += string.Format($"{item.Key} : {item.Value}") + "\n";
            return print;
        }

        public string FindMostPopularDays()
        {
            string print = "The most popular day for each IP:\n";
            SortedDictionary<string, Dictionary<DayOfWeek, int>> findDays = new SortedDictionary<string, Dictionary<DayOfWeek, int>>();
            for (int i = 0; i < IPs.Length; ++i)
                if (!findDays.ContainsKey(IPs[i].ip))
                {
                    findDays.Add(IPs[i].ip, new Dictionary<DayOfWeek, int>());
                    findDays[IPs[i].ip].Add(IPs[i].day, 1);
                }
                else
                {
                    if(!findDays[IPs[i].ip].ContainsKey(IPs[i].day))
                        findDays[IPs[i].ip].Add(IPs[i].day, 1);
                    findDays[IPs[i].ip][IPs[i].day]++;
                }

            DayOfWeek day = DayOfWeek.Monday; 
            int count = 0;
            foreach (var item in findDays)
            {
                count = 0;
                foreach (var item1 in item.Value)
                    if(item1.Value > count)
                    {
                        day = item1.Key;
                        count = item1.Value;
                    }
                print += item.Key + " : " + day + "\n";
            }
            return print;
        }

        public string FindMostPopularTimes()
        {
            string print = "The most popular time for each IP:\n";
            Dictionary<string, Dictionary<DateTime, int>> findTimes = new Dictionary<string, Dictionary<DateTime, int>>();
            for (int i = 0; i < IPs.Length; ++i)
                if (!findTimes.ContainsKey(IPs[i].ip))
                {
                    findTimes.Add(IPs[i].ip, new Dictionary<DateTime, int>());
                    findTimes[IPs[i].ip].Add(IPs[i].time, 1);
                }
                else
                {
                    if (!findTimes[IPs[i].ip].ContainsKey(IPs[i].time))
                        findTimes[IPs[i].ip].Add(IPs[i].time, 1);
                    findTimes[IPs[i].ip][IPs[i].time]++;
                }
            foreach (var item in findTimes)
            {
                print += item.Key + " : " + item.Value.First().Key.ToString(@"HH\:mm\:ss") + 
                    "-" + item.Value.Last().Key.ToString(@"HH\:mm\:ss") + "\n";
            }
            return print;
        }
        public string FindMostPupularTime()
        {
            SortedDictionary<DateTime, int> findTime = new SortedDictionary<DateTime, int>();
            for (int i = 0; i < IPs.Length; ++i)
                if (!findTime.ContainsKey(IPs[i].time))
                    findTime.Add(IPs[i].time, 1);
                else findTime[IPs[i].time]++;
            
            return $"The most popular time is : " + findTime.First().Key.ToString(@"HH\:mm\:ss") + 
                    "-" + findTime.Last().Key.ToString(@"HH\:mm\:ss") + "\n";
        }
        public override string ToString()
        {
            string print = "";
            for (int i = 0; i < IPs.Length; ++i)
                print += IPs[i] + "\n";
            return print;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Site site = new Site();
            site.ReadFile(@"C:\Users\VETAL\Desktop\IP.txt");
            Console.WriteLine(site);
            Console.WriteLine(site.FindAmountVisitorsIPs());
            Console.WriteLine(site.FindMostPopularDays());
            Console.WriteLine(site.FindMostPopularTimes());
            Console.WriteLine(site.FindMostPupularTime());
        }
    }
}
