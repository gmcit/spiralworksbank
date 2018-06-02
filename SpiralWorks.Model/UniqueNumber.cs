﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpiralWorks.Model
{
    [Table("UniqueNumber")]
    public class UniqueNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int UniqueNumberId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string AccountNumber
        {
            get => this.UniqueNumberId.ToString($"D{(UniqueNumberId.ToString().Length + 5)}");
            private set { }
        }
    }
}
