using SystemSoftware1.Constants;

namespace SystemSoftware1.Models
{
    public class Lexem
    {
        public LexemTypes Type { get; private set; }
        public int Lex { get; private set; }
        public string Value { get; private set; }

        public Lexem(LexemTypes type, int lex, string value)
        {
            Type = type;
            Lex = lex;
            Value = value;
        }

        public override string ToString()
        {
            return $"lexem type: {Type}\t lexem id: {Lex}\t value: {Value}";
        }
    }
}
