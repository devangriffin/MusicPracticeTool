using System.Collections.Generic;
using System.IO;

namespace MusicPracticeTool
{
    public class Program
    {
        public static int SongAmount = 0;
        public static List<Song> SongList = InputData();
        public static List<Song> MemorizedList = new List<Song>();
        public static List<Song> ProgressList = new List<Song>();
        public static List<Song> PendingList = new List<Song>();

        public static void Main(string[] args)
        {
            bool isActive = true;

            SortLists(SongList, MemorizedList, ProgressList, PendingList);

            #region Initial Print Statements
            Console.WriteLine("Song Amount: " + SongAmount + '\n');

            Console.WriteLine("Memorized:");
            PrintList(MemorizedList, 1);
            Console.WriteLine("In Progress:");
            PrintList(ProgressList, 1);
            Console.WriteLine("Pending:");
            PrintList(PendingList, 1);

            Console.WriteLine("List of Actions:");
            Console.WriteLine("1: Get a Random Song from the Song List");
            Console.WriteLine("2: Get a Random Song from the Memorized List");
            Console.WriteLine("3: Get a Random Song from the In Progress List");
            Console.WriteLine("4: Get a Random Song from the Pending List");
            Console.WriteLine("5: Add a Song");
            Console.WriteLine("6: Remove a Song\n");
            #endregion

            while(isActive)
            {
                int actionNum = 0;
                Console.Write("Enter Number for Action: ");
                try
                {
                    actionNum = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Incorrect Input");
                }

                ActionSwitch(actionNum);
            }
        }

        /// <summary>
        /// Inputs the song data from a txt file
        /// </summary>
        /// <param name="amount">Outputs the amount of songs in the txt file</param>
        /// <returns>A list containing all the songs in the txt file</returns>
        public static List<Song> InputData()
        {
            using (StreamReader sr = new StreamReader("../../../Songs.txt"))
            {
                List<Song> SongList = new List<Song>();
                string firstLine = sr.ReadLine();
                string[] split = firstLine.Split(": ");
                int songCount = Int32.Parse(split[1]);
                SongAmount = songCount;
                sr.ReadLine();

                for (int i = 0; i < songCount; i++)
                {
                    string line = sr.ReadLine();
                    string[] splitLine = line.Split(" - ");
                    Song newSong = new Song(splitLine[1], splitLine[2], Int32.Parse(splitLine[3]), splitLine[4]);
                    SongList.Add(newSong);
                }

                return SongList;
            }
        }

        public static void SortLists(List<Song> SongList, List<Song> MemorizedList, List<Song> ProgressList, List<Song>PendingList)
        {
            foreach (Song s in SongList)
            {
                if (s.Level.Equals("Memorized")) { MemorizedList.Add(s); }
                else if (s.Level.Equals("In Progress")) { ProgressList.Add(s); }
                else if (s.Level.Equals("Pending")) { PendingList.Add(s);  }
            }
        }

        public static void PrintList(List<Song> list, int num)
        {
            if (num > 0)
            {
                foreach (Song s in list)
                {
                    Console.WriteLine(s.ToString(num++));
                }
            }
            else
            {
                foreach (Song s in list)
                {
                    Console.WriteLine(s.ToString());
                }
            }

            Console.WriteLine();
        }

        public static Song GetRandomSong(List<Song> list)
        {
            Random random = new Random();
            int num = random.Next(0, list.Count);

            return list[num];
        }

        public static void AddSong()
        {
            string title;
            string location;
            int pageNum;
            int numLevel;
            string level;

            try
            {
                Console.Write("Enter Title: ");
                title = Console.ReadLine();
                Console.Write("Enter Location: ");
                location = Console.ReadLine();
                Console.Write("Enter Page Number: ");
                pageNum = Int32.Parse(Console.ReadLine());
                Console.Write("Enter List - Memorized (1) - In Progress (2) - Pending (3): ");
                numLevel = Int32.Parse(Console.ReadLine());
                if (numLevel == 1) { level = "Memorized"; }
                else if (numLevel == 2) { level = "In Progress"; }
                else if (numLevel == 3) { level = "Pending"; }
                else { throw new Exception(); }
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorrect Input: " + e);
                return;
            }

            Song song = new Song(title, location, pageNum, level);

            if (song.Level.Equals("Memorized")) { MemorizedList.Add(song); }
            else if (song.Level.Equals("In Progress")) { ProgressList.Add(song); }
            else if (song.Level.Equals("Pending")) { PendingList.Add(song); }

            using (StreamWriter sw = File.AppendText("Songs.txt"))
            {
                sw.WriteLine(song.ToString(SongAmount++));
            }
        }

        public static void ActionSwitch(int actionNum)
        {
            switch (actionNum)
            {
                case 1:
                    Console.WriteLine(GetRandomSong(SongList) + "\n");
                    break;
                case 2:
                    Console.WriteLine(GetRandomSong(MemorizedList) + "\n");
                    break;
                case 3:
                    Console.WriteLine(GetRandomSong(ProgressList) + "\n");
                    break;
                case 4:
                    Console.WriteLine(GetRandomSong(PendingList) + "\n");
                    break;
                case 5:
                    AddSong();
                    break;
                default:
                    Console.WriteLine("Incorrect Input");
                    break;
            }
        }
        
    }
}