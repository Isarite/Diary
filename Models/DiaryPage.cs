using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryApp.Models
{
    public class DiaryPage
    {
        [Key]
        public int id { get; set; }
        public IEnumerable<Marking> markings { get; set; }
        public int number { get; set; }
        public string text { get; set; }
    }
}
