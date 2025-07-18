using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace mhris.Models
{
    public class Setting
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string LoginCode { get; set; }

        public bool IsSwipedIn { get; set; }
        public bool IsSwipedBreakStart { get; set; }
        public bool IsSwipedBreakEnd { get; set; }
        public bool IsSwipedOut { get; set; }
    }
}
