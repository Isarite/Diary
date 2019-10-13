using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryApp.Models
{
    public class Diary
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public string id { get; set; }
        public User user { get; set; }
        public string name { get; set; }
        public IList<DiaryPage> pages { get; set; }
        public DateTime created { get; set; }
        public DateTime edited { get; set; }
    }
}
