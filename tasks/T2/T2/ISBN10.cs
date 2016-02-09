using System;
using System.Linq;

namespace T2
{
    public class ISBN10 : ISBN
    {
        public ISBN10()
        {
        }

        public ISBN10(string isbnText)
        {
            string[] s = isbnText.Split('-');
            if (s.Length != 4)
                throw new ArgumentException("Invalid Format", "isbnText");
            RegistrationGroup = short.Parse(s[0]);
            Registrant = short.Parse(s[1]);
            Publication = int.Parse(s[2]);
            CheckDigit = s[3][0];
        }

        public override char CalculateCheckDigit()
        {
            byte[] isbnDigits = ValueWithoutHyphens.Select(c => byte.Parse(c.ToString())).ToArray();
            int m = 1, result = 0;
            for (int i = 0; i < 13; i++)
                result += isbnDigits[i] * (2 * (m++ % 2) + 1);
            int checkDigit = (10 - result % 10) % 10;
            return Convert.ToChar(checkDigit);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}-{3}", RegistrationGroup, Registrant, Publication, CheckDigit);
        }
    }
}