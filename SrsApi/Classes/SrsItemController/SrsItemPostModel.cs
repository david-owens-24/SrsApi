﻿using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemController
{ 
    public class SrsItemPostModel
    {
        [Required]
        public Guid SrsItemLevelUID { get; set; }

        public int Order { get; set; }
    }
}
