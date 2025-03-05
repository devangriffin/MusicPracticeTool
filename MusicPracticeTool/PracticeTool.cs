using System.Collections.Generic;
using System.IO;

namespace MusicPracticeTool
{
    public class PracticeTool
    {
        public static int SongAmount = 0;
        public static string FileLocation = "Songs.txt";
        public static List<Song> SongList = InputData();
        public static List<Song> MemorizedList = new List<Song>();
        public static List<Song> ProgressList = new List<Song>();
        public static List<Song> PendingList = new List<Song>();

        public static void Main(string[] args)
        {
            bool isActive = true;

            SortLists(SongList, MemorizedList, ProgressList, PendingList);

            PrintAll();

            while (isActive)
            {
                int actionNum = 0;
                PrintActions();
                try { actionNum = Int32.Parse(Console.ReadLine()); }
                catch { Console.WriteLine("Incorrect Input"); }
                Console.WriteLine();

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
            using (StreamReader sr = new StreamReader(FileLocation))
            {
                List<Song> SongList = new List<Song>();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] splitLine = line.Split(" - ");
                    Song newSong = new Song(splitLine[1], splitLine[2], Int32.Parse(splitLine[3]), splitLine[4]);
                    SongList.Add(newSong);
                }

                return SongList;
            }
        }

        public static void SortLists(List<Song> SongList, List<Song> MemorizedList, List<Song> ProgressList, List<Song> PendingList)
        {
            foreach (Song s in SongList)
            {
                if (s.Level.Equals("Memorized")) { MemorizedList.Add(s); }
                else if (s.Level.Equals("In Progress")) { ProgressList.Add(s); }
                else if (s.Level.Equals("Pending")) { PendingList.Add(s); }
            }
        }

        public static void PrintList(List<Song> list, int num)
        {
            if (num > 0) { foreach (Song s in list) { Console.WriteLine(s.ToString()); } }
            else { foreach (Song s in list) { Console.WriteLine(s.ToString()); } }

            Console.WriteLine();
        }

        public static void PrintActions()
        {
            Console.WriteLine("======================================================================\n");
            Console.WriteLine("List of Actions:");
            Console.WriteLine("1: Show all the Songs");
            Console.WriteLine("2: Get a Random Song from a List");
            Console.WriteLine("3: Add a Song");
            Console.WriteLine("4: Remove a Song");
            Console.Write("Enter Number for Action: ");
        }

        public static void PrintAll()
        {
            Console.WriteLine("======================================================================\n");
            Console.WriteLine("Song Amount: " + SongAmount);
            Console.WriteLine("\nMemorized " + "(" + MemorizedList.Count + "):");
            PrintList(MemorizedList, 1);
            Console.WriteLine("In Progress " + "(" + ProgressList.Count + "):");
            PrintList(ProgressList, 1);
            Console.WriteLine("Pending " + "(" + PendingList.Count + "):");
            PrintList(PendingList, 1);
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
                Console.Write("Enter Page Number (0 if not Applicable): ");
                pageNum = Int32.Parse(Console.ReadLine());
                Console.Write("Enter List - Memorized (1) - In Progress (2) - Pending (3): ");
                numLevel = Int32.Parse(Console.ReadLine());
                Console.WriteLine();
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

            if (IsRepeat(song)) 
            {
                Console.WriteLine("ERROR: SONG HAS ALREADY BEEN ADDED");
                Console.ReadLine();
                return; 
            }
            else
            {
                Console.WriteLine("Added " + song.Title + " to the " + song.Level + " List");
                Console.ReadLine();
            }

            SongList.Add(song);
            if (song.Level.Equals("Memorized")) { MemorizedList.Add(song); }
            else if (song.Level.Equals("In Progress")) { ProgressList.Add(song); }
            else if (song.Level.Equals("Pending")) { PendingList.Add(song); }

            using (StreamWriter sw = File.AppendText(FileLocation))
            {
                string line = song.ToString();
                sw.WriteLine(line);
            }
        }

        public static bool IsRepeat(Song s)
        {
            foreach (Song song in SongList)
            {
                if (s.Title.Equals(song.Title)) { return true; }
                if (s.Location.Equals(song.Location) && (s.PageNumber.Equals(song.PageNumber)) && (s.PageNumber != 0)) { return true; }
            }

            return false;
        }

        public static void RemoveSong()
        {
            int input = 0;
            Console.Write("What is the number of the Song you want to remove?: ");
            try { input = Int32.Parse(Console.ReadLine()); }
            catch { Console.WriteLine("Incorrect Input"); }

            Song removedSong = SongList[input - 1];

            Console.Write("You Want To Delete '" + removedSong.Title + "'? (Type 'X' + to Cancel, Press ENTER to Remove): ");
            string input2 = Console.ReadLine().ToUpper();

            if (input2.Equals("X")) 
            {
                Console.WriteLine("\nCanceled Removal");
                Console.ReadLine();
                return; 
            }

            SongList.Remove(removedSong);
            if (removedSong.Level.Equals("Memorized")) { MemorizedList.Remove(removedSong); }
            else if (removedSong.Level.Equals("In Progress")) { ProgressList.Remove(removedSong); }
            else if (removedSong.Level.Equals("Pending")) { PendingList.Remove(removedSong);  }

            RenumberSongs();
            RewriteFile();   
        }

        public static void RenumberSongs()
        {
            int num = 1;
            foreach(Song song in SongList)
            {
                song.SongNumber = num++;
            }
        }

        public static void RewriteFile()
        {
            string tempFile = Path.GetTempFileName();

            using (StreamWriter sw = File.AppendText(tempFile))
            {
                foreach (Song song in SongList)
                {
                    string line = song.ToString();
                    sw.WriteLine(line);
                }
            }

            File.Delete(FileLocation);
            File.Move(tempFile, FileLocation);
        }

        public static void ActionSwitch(int actionNum)
        {
            switch (actionNum)
            {
                case 1:
                    PrintAll();
                    Console.ReadLine();
                    break;
                case 2:
                    Console.Write("Which List? Memorized (1) - In Progress (2) - Pending (3) - Full List (4): ");
                    int actionNum2;
                    try { actionNum2 = Int32.Parse(Console.ReadLine()); }
                    catch { Console.WriteLine("Incorrect Input"); break; }
                    Console.WriteLine("\n======================================================================\n");
                    switch (actionNum2)
                    {
                        case 1:
                            Console.WriteLine(GetRandomSong(MemorizedList));
                            Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine(GetRandomSong(ProgressList));
                            Console.ReadLine();
                            break;
                        case 3:
                            Console.WriteLine(GetRandomSong(PendingList));
                            Console.ReadLine();
                            break;
                        case 4:
                            Console.WriteLine(GetRandomSong(SongList));
                            Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine("Incorrect Input");
                            break;
                    }
                    break;
                case 3:
                    AddSong();
                    break;
                case 4:
                    RemoveSong();
                    break;
                default:
                    Console.WriteLine("Incorrect Input");
                    break;
            }
        }

    }
}