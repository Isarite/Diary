using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryApp.Models
{
    public class DiaryPage
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public Diary diary { get; set; }
        public IList<Marking> markings { get; set; }
        public int number { get; set; }
        public string text { get; set; }
    }
}
