using System;

namespace SystemSoftware1.Constants
{
    public enum LexemTypes : int
    {
        ParsingError = -1,
        DataType = 0,
        Variable = 1,
        Delimeter = 2,
        Indentifier = 3,
        Constant = 4,
        Operation = 5,
    }
}
