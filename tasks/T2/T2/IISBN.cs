namespace T2
{
    public interface IISBN
    {
        char CheckDigit { get; set; }
        byte CheckDigitValue { get; }
        int Publication { get; set; }
        int Registrant { get; set; }
        short RegistrationGroup { get; set; }
        string Value { get; }
        string ValueWithoutHyphens { get; }

        char CalculateCheckDigit();
    }
}