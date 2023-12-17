﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.ShowcaseAggregate
{
    public class ShowcaseBlock : BaseEntity
    {
        public string? Link { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Image { get; set; }
    }
}
