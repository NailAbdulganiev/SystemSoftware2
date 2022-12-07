using System;

namespace SystemSoftware1.Constants
{
    public class Constants
    {
        public static readonly Dictionary<string, (int, string)> Types = new Dictionary<string, (int, string)>()
        {
            { "int", (0, "int") },
            { "uint", (1, "uint") },
            { "long", (2, "long") },
            { "ulong", (3, "ulong") },
            { "string", (4, "string") },
        };

        public static readonly Dictionary<string, (int, string)> Operators = new Dictionary<string, (int, string)>()
        {
            {"=", (0, "=")},
            {"+", (1, "+")},
            {"-", (2, "-")},
            {"*", (3, "*")},
            {"/", (4, "/")},
            {"+=", (5, "+=")},
            {"-=", (6, "-=")},
            {"==", (7, "==")},
            {">", (8, ">")},
            {"<", (9, "<")},
            {"++", (10, "++")},
            {"--", (11, "--")},
            {"%", (12, "%")},
        };

        public static readonly string[] Keywords = { "class", "public", "private", "for", "return", "if", "else", "while" };

        public static readonly string[] KeySymbols = { ".", ";", ",", "(", ")", "[", "]", "{", "}" };
    }
}
