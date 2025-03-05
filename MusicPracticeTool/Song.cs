using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicPracticeTool
{
    public class Song
    {
        public string Title { get; set; }

        public string Location { get; set; }

        public string Level { get; set; }

        public int PageNumber { get; set; }

        public Song(string title, string location, int pageNumber, string level)
        {
            Title = title;
            Location = location;
            Level = level;
            PageNumber = pageNumber;
        }

        public string ToString(int num)
        {
            return new string(num + " - " + Title + " - " + Location + " - " + PageNumber + " - " + Level);
        }

        public override string ToString()
        {
            return new string(Title + " - " + Location + " - " + PageNumber + " - " + Level);
        }
    }
}
