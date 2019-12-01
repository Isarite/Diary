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
        public DateTime created { get; set; }
        public DateTime edited { get; set; }

        public DiaryResource(string id, string name, DateTime created, DateTime edited)
        {
            this.Id = id;
            this.Name = name;
            this.created = created;
            this.edited = edited;
        }
    }
}
