﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Salesperson
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0} \t Name: {1}", Id, Name);
        }
    }
}