﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandService.Models
{
    public class Command
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string HowTo { get; set; }
        [Required]
        public string CommandLine { get; set; }
        [Required]    
        public int PlatformId { get; set; }
        //[ForeignKey("PlatformId")]
        public Platform Platform { get; set; }

    }
}
