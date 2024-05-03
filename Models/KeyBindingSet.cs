using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchPlaysAnything.Models
{
    public class KeyBindingSet
    {
        // Keys are <input, key>
        public List<Dictionary<string, string>>? Keys { get; set; }
        public string? ApplicationName { get; set; }
    }
}
