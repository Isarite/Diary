using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryApp.Models
{
    public class User:IdentityUser
    {
        public IEnumerable<Diary> diaries { get; set; }
    }
}
