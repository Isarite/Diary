using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryApp.Models
{
    public class Diary
    {
        [Key]
        public int id { get; set; }
        public User user { get; set; }
        public string name { get; set; }
        public IEnumerable<DiaryPage> pages { get; set; }
        public DateTime created { get; set; }
        public DateTime edited { get; set; }
    }
}
