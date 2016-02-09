using System;
using System.Linq;

namespace T2
{
    public class ISBN13 : ISBN
    {
        //A three digit prefix that can be either 978 or 979. Note that the new 979 
        //prefix will not be utilized until all 978 prefixes have been assigned.
        public short Prefix
        {
            get;
            set;
        }

        public ISBN13()
        {
        }

        public ISBN13(string isbnText)
        {
            string[] s = isbnText.Split('-');
            if (s.Length != 5)
                throw new ArgumentException("Invalid Format", "isbnText");
            Prefix = short.Parse(s[0]);
            RegistrationGroup = short.Parse(s[1]);
            Registrant = short.Parse(s[2]);
            Publication = int.Parse(s[3]);
            CheckDigit = s[4][0];
        }

        public override char CalculateCheckDigit()
        {
            byte[] isbnDigits = ValueWithoutHyphens.Select(c => byte.Parse(c.ToString())).ToArray();
            int m = 10, result = 0;
            for (int i = 0; i < 10; i++)
                result += isbnDigits[i] * m--;
            int checkDigit = 11 - (result % 11);
            if (checkDigit == 11)
                return '0';
            if (checkDigit == 10)
                return 'X';
            return Convert.ToChar(checkDigit);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}", Prefix, RegistrationGroup, Registrant, Publication, CheckDigit);
        }
    }
}