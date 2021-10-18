using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sigma_task6_2
{Чому все в одному файлі
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
        {Використовуйте using, або закривайте потоки та використовуйте винятки
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
            DayOfWeek day = DayOfWeek.Monday;
            int max = 0;
            Dictionary<DayOfWeek, int> findDays = new Dictionary<DayOfWeek, int>();
            List<string> contain = new List<string>();
            for (int i = 0; i < IPs.Length; ++i)
                if (!contain.Contains(IPs[i].ip))
                {
                    contain.Add(IPs[i].ip);
                    for (int j = 0; j < IPs.Length; ++j)
                        if (IPs[i].ip == IPs[j].ip)
                            if (findDays.TryGetValue(IPs[i].day, out int key))
                                findDays[IPs[i].day] = ++key;
                            else findDays.Add(IPs[i].day, 1);
                    foreach (var item in findDays)
                        if (item.Value > max)
                        {
                            max = item.Value;
                            day = item.Key;
                        }
                    print += IPs[i].ip + " : " + day + "\n";
                }
            return print;
        }

        public string FindMostPopularTimes()
        {
            string print = "The most popular time for each IP:\n";
            List<DateTime> dates = new List<DateTime>();
            List<string> contain = new List<string>();
            bool check = false;
            for (int i = 0; i < IPs.Length; ++i)
                if (!contain.Contains(IPs[i].ip))
                {
                    contain.Add(IPs[i].ip);
                    for (int j = 0; j < IPs.Length; ++j)
                        if (IPs[i].ip == IPs[j].ip)
                            dates.Add(IPs[j].time);

                    dates.Sort();
                    for (int j = 0; j < dates.Count - 1; ++j)
                        if ((dates[dates.Count - 1] - dates[j]).TotalHours <= 1)
                        {
                            print += IPs[i].ip + " : " + dates[j].ToString(@"HH\:mm\:ss") + "-" + dates[dates.Count - 1].ToString(@"HH\:mm\:ss");
                            check = true;
                            break;
                        }
                    if(!check)
                        print += IPs[i].ip + " : " + dates[dates.Count - 1].ToString(@"HH\:mm\:ss") + "-" +
                            dates[dates.Count - 1].AddHours(1).ToString(@"HH\:mm\:ss");
                    print += "\n";
                }
            return print;
        }
        public string FindMostPupularTimeServer()
        {
            List<DateTime> dates = new List<DateTime>();
            for (int i = 0; i < IPs.Length; ++i)
                dates.Add(IPs[i].time);
            dates.Sort();
            for (int i = 0; i < dates.Count - 1; ++i)
                if ((dates[dates.Count - 1] - dates[i]).TotalHours <= 1)
                    return "The most popular time in a day IP:\n" + 
                        dates[i].ToString(@"HH\:mm\:ss") + "-" + 
                        dates[dates.Count - 1].ToString(@"HH\:mm\:ss") + "\n";        
            return "The most popular time in a day IP:\n" + 
                dates[dates.Count - 1].ToString(@"HH\:mm\:ss") + "-" +
                dates[dates.Count - 1].AddHours(1).ToString(@"HH\:mm\:ss") + "\n";
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
            site.ReadFile(@"C:\Users\VETAL\Desktop\IPP.txt");
            Console.WriteLine(site);
            Console.WriteLine(site.FindAmountVisitorsIPs());
            Console.WriteLine(site.FindMostPopularDays());
            Console.WriteLine(site.FindMostPopularTimes());
            Console.WriteLine(site.FindMostPupularTimeServer());
        }
    }
}
