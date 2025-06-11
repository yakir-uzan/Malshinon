using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon
{
   public class People
   {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LestName { get; set; }
        public string SecretCode { get; set; }
        public string Type { get; set; }

        public People(int id, string firstName, string secretCode, string lestName, string type)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LestName = lestName;
            this.SecretCode = secretCode;
            this.Type = type;
        }
   }
}
