using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyRestfulAPICourse.Models
{
    public class Quote
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Required]
        [StringLength(20)]
        public string Author { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // the JsonIgnore attribute will hide the UserId property from search results
        // [JsonIgnore]
        public string UserId { get; set; }

    }
}
