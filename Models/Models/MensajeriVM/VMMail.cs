using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.MensajeriVM
{
    public class VMMail: VMMailMessage
    {
        public VMMail() { }

        public string Template { get; set; }
    }
}
