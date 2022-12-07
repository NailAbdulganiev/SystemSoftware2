using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSoftware1.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SystemSoftware1
{
    public class SyntacticalAnalyzer
    {
        List<Lexem> lexems;
        List<Variable> variables;
        string[] words;

        string lexemType;
        public void process(LexemProcessor lexemProcessor)
        {
            lexems = lexemProcessor.getLexems();
            variables = lexemProcessor.getVariables();
            var separators = new string[] { "lexem type: ", "lexem id: ", "value: ", "\t " };
            foreach (var lexem in lexems)
            {
                string lexem_string = lexem.ToString();
                words = lexem_string.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                switch (words[0])
                {
                    case "DataType":
                        DataType_Node(words);
                        break;
                }

                foreach (string s in words)
                {
                    Console.WriteLine(s);
                }
            }
        }

        public void DataType_Node(string[] words)
        {
            Console.WriteLine();
        }
    }
}
