using System;
using System.Collections.Generic;
using System.Text;

namespace DavBuzzer.Models
{
    public class Hint : BaseModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int Round { get; set; }
        public int NumberCharacterToHide { get; set; }
    }
}
