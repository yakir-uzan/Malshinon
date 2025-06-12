using System;
using System.Collections.Generic;

namespace Malshinon
{
    public class MalshinonHelper
    { 
        // פונקציית עזר שמקבלת שם מלא ומחזירה סיקרט קוד
        public string CreateSecretCode(string firstName, string lastName)
        {
            Random rnd = new Random();
            string secretCode = $"{firstName[1]}{lastName[1]}" + rnd.Next(10, 7500);
            return secretCode;
        }

        //פונקציה שמחפשת את שם המטרה בתןך הדיווח
        public string SearchTarget(string reportTxt)
        {
            Console.WriteLine("Please enter the target name in the first 2 words of the ceremony with the first and last name starting with capital letters!");

            string[] splitReport = reportTxt.Split();

            List<string> fullName = new List<string>();

            foreach (string word in splitReport)
            {
                if (!string.IsNullOrWhiteSpace(word) && char.IsUpper(word[0]))
                {
                    fullName.Add(word);
                    if (fullName.Count == 2)
                        break;
                }
            }

            if (fullName.Count < 2)
            {
                Console.WriteLine("Target name not found correctly. Please enter first and last name with capital letters.");
                return null;
            }
            string fullNameStr = $"{fullName[0]} {fullName[1]}";
            return fullNameStr;
        }
    
    }

}

