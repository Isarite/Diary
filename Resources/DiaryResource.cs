using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryApp.Resources
{
    public class DiaryResource
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int PageCount { get; set; }
        public DateTime created { get; set; }
        public DateTime edited { get; set; }

        public DiaryResource(string Id, string Name, int PageCount, DateTime created, DateTime edited)
        {
            this.Id = Id;
            this.Name = Name;
            this.PageCount = PageCount;
            this.created = created;
            this.edited = edited;
        }
    }
}
