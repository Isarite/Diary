using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Models
{
    public class Picture
    {
        [Key] 
        public string fileName { get;set;}

    }
}