using SystemSoftware1;

LexemProcessor processor = new LexemProcessor();
var result = processor.ProcessFile("./InputData/input.txt");
//Console.WriteLine("Lexems: ");
//foreach (var lexem in result.Item1)
//{
//    Console.WriteLine(lexem.ToString());
//}
//Console.WriteLine("Variables: ");
//foreach (var variable in result.Item2)
//{
//    Console.WriteLine(variable.ToString());
//}
//Console.Read();
SyntacticalAnalyzer syntacticalAnalyzer = new SyntacticalAnalyzer();
syntacticalAnalyzer.process(processor);