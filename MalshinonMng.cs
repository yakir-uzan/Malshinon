using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Malshinon
{
    internal class MalshinonMng
    {
        MalshinonDAL dal = new MalshinonDAL();

        public void MngPersonFlow()
        {
            Console.WriteLine("Enter your first Name: ");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter your last Name: ");
            string lastName = Console.ReadLine();
        }


    }
}
