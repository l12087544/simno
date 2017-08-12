using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run
{
    public class Resource
    {
        public bool? FIsSeparator { get; internal set; }
        public string FMenu { get; set; }
        public int FMenuLevel { get; internal set; }
        public int? FMenuType { get; set; }
        public string FParentResourceId { get; internal set; }
        public string FResourceId { get; internal set; }
    }
}
