using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAnime1.Models
{
    public class Specials
    {
        public string Min(DateTime date)
        {
            return date.ToString("d");
        }
    }
}
