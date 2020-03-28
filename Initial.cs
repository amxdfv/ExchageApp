using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ExchangeApp2
{
    class Initial
    {
       public string[] GetPresets()
        {
            int counter = 0;
            string[] DBlineSet = new string[10];
            string[] ConStr = new string[2];

            System.IO.StreamReader file =
                new System.IO.StreamReader("SetSH\\set.ini");
            while ((DBlineSet[counter] = file.ReadLine()) != null)
            {
                counter++;
                if (counter >= 10) { break; }
            }
            ConStr[0] = "DATA SOURCE="+ DBlineSet[1].Remove(0, 18) +
                ";PASSWORD="+ DBlineSet[2].Remove(0, 7) +
                ";PERSIST SECURITY INFO=True;USER ID="+ DBlineSet[3].Remove(0, 6);
            ConStr[1] = "DATA SOURCE=" + DBlineSet[6].Remove(0, 18) +
                ";PASSWORD=" + DBlineSet[7].Remove(0, 7) +
                ";PERSIST SECURITY INFO=True;USER ID=" + DBlineSet[8].Remove(0, 6);
            return ConStr; 
        }

       
        
    }
}
