﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericRepository.Models;

namespace News.API.Models
{
    [Table("Pictures")]
    public partial class Picture : Entity
    {
        public Picture()
        {
        }

        public int Id { get; set; }

        [StringLength(256)] public string Name { get; set; }
        [Required] [StringLength(256)] public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int? Order { get; set; }
        public int PictureType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
    }
}