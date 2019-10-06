using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryApp.Models
{
    public class Marking
    {
        [Key]
        public int id { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public string color { get; set; }
    }
}
