using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MalshinonDAL MDAL = new MalshinonDAL();
            //MDAL.InsertNewPerson("yakir", "uzan", "jossef", "reporter");
            //MDAL.GetPersonByName("yakir", "uzan");
            //MDAL.InsertNewPerson("merav", "cohen", "mc", "both");
            MDAL.GetPersonByName("merav", "cohen");
            MDAL.GetPersonBySecretCode("jossef");

        }
    }
}
