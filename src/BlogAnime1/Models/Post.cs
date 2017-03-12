using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlogAnime1.Models
{
    public class Post
    {
        public long Id { get; set; }
        private string _key;
        public  string Key
        {
            get
            {
                if(_key==null)
                {
                    _key = Regex.Replace(Title.ToLower(),"[^a-z0-9]","-");
                }

                return _key;
            }
            set
            {
                _key = value;
            }
        }
        [Required]
        [StringLength(100,MinimumLength=5,ErrorMessage ="title must be between 5 and 100 characters")]
        public string Title
        {
            get;
            set;
        }
        [Required]
        [MinLength(100, ErrorMessage = "Blog Posts must be between at least 100 characters")]
        public string Body
        {
            get;
            set;
        }
        public DateTime Posted
        {
            get;
            set;
        }
        

    }
}
