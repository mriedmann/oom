using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2
{
    public abstract class ISBN
    {
        //A variable set of digits that identify the country/region in which the ISBN code was assigned. 
        //Zero ("0") means that the book belongs to an English speaking area.
        public short RegistrationGroup { get; set; }

        //A variable set of digits that identify the publisher to whom the ISBN code was originally allocated.
        public int Registrant { get; set; }

        //A variable set of digits that identify the title of the book.
        public int Publication { get; set; }

        //A one digit control number that makes it possible to validate the ISBN code. The check digit is calculated 
        //using a formula that makes use of the preceding parts (Prefix, Registration group, Registrant and Publication).
        public char CheckDigit { get; set; }

        public string Value { get { return ToString(); } }

        public string ValueWithoutHyphens { get { return ToString().Replace("-", ""); } }

        public byte CheckDigitValue
        {
            get
            {
                if (CheckDigit == 'x' || CheckDigit == 'X')
                    return 10;
                else
                    return byte.Parse(CheckDigit.ToString());
            }
        }

        public ISBN()
        {
        }

        //ex. 978-0-14026-690-0
        

        public abstract char CalculateCheckDigit();

        
    }

    public class ISBN10 : ISBN
    {
        public ISBN10() { }

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

    public class ISBN13 : ISBN
    {
        //A three digit prefix that can be either 978 or 979. Note that the new 979 
        //prefix will not be utilized until all 978 prefixes have been assigned.
        public short Prefix { get; set; }

        public ISBN13() { }

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

            if (checkDigit == 11) return '0';
            if (checkDigit == 10) return 'X';

            return Convert.ToChar(checkDigit);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}", Prefix, RegistrationGroup, Registrant, Publication, CheckDigit);
        }
    }
}
