﻿namespace T2
{
    public abstract class ISBN : IISBN
    {
        //A variable set of digits that identify the country/region in which the ISBN code was assigned. 
        //Zero ("0") means that the book belongs to an English speaking area.
        public short RegistrationGroup
        {
            get;
            set;
        }

        //A variable set of digits that identify the publisher to whom the ISBN code was originally allocated.
        public int Registrant
        {
            get;
            set;
        }

        //A variable set of digits that identify the title of the book.
        public int Publication
        {
            get;
            set;
        }

        //A one digit control number that makes it possible to validate the ISBN code. The check digit is calculated 
        //using a formula that makes use of the preceding parts (Prefix, Registration group, Registrant and Publication).
        public char CheckDigit
        {
            get;
            set;
        }

        public string Value
        {
            get
            {
                return ToString();
            }
        }

        public string ValueWithoutHyphens
        {
            get
            {
                return ToString().Replace("-", "");
            }
        }

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
}