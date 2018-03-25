using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudCreatorConsole.Model
{
    class Attr
    {
        String visibility;
        String name;
        String sqlName;
        String type;

        public Attr()
        {

        }

        public string Visibility { get => visibility; set => visibility = value; }
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string SqlName { get => sqlName; set => sqlName = value; }
    }
}
