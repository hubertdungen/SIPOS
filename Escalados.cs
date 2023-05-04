using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqList
{
    public class ListaManagerEscalados
    {

        public static List<Pessoa> escaladosList = new List<Pessoa>();

        public static List<Pessoa> LoadList()
        {
            return escaladosList;
        }

    }


}

