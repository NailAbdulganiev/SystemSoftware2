using Microsoft.VisualBasic;
using System;
using SystemSoftware1.Constants;
using SystemSoftware1.Models;

namespace SystemSoftware1
{
    public class LexemProcessor
    {
        private string buffer = "";
        private string seekingBuffer = "";
        private int pointer = 0;
        private LexemProcessorStates _state = LexemProcessorStates.Idle;
        private StringReader? reader;
        private char[] charBuffer = new char[1];
        private List<Lexem> lexems = new List<Lexem>();
        private List<Variable> _variablesTable = new List<Variable>();
        private int _variablesCounter = 0;



        public Tuple<IList<Lexem>, IList<Variable>> ProcessFile(string fileName)
        {
            using (reader = new StringReader(File.ReadAllText(fileName)))
            {
                while (_state != LexemProcessorStates.Final)
                {
                    switch (_state)
                    {
                        case LexemProcessorStates.Idle:
                            {
                                if (reader.Peek() == -1)
                                {
                                    _state = LexemProcessorStates.Final;
                                    break;
                                }

                                if (IsEmptyOrNextLine(charBuffer[0]))
                                {
                                    GetNextChar();
                                }
                                else if (char.IsLetter(charBuffer[0]))
                                {
                                    ClearBuffer();
                                    AddToBuffer(charBuffer[0]);
                                    _state = LexemProcessorStates.ReadingIdentifier;
                                    GetNextChar();
                                }
                                else if (char.IsDigit(charBuffer[0]))
                                {
                                    ClearBuffer();
                                    AddToBuffer(charBuffer[0]);
                                    _state = LexemProcessorStates.ReadingNum;
                                    GetNextChar();
                                }
                                else
                                {
                                    _state = LexemProcessorStates.Delimeter;
                                    AddToBuffer(charBuffer[0]);
                                    GetNextChar();
                                }
                                break;
                            }
                        case LexemProcessorStates.ReadingIdentifier:
                            {
                                if (char.IsLetterOrDigit(charBuffer[0]))
                                {
                                    AddToBuffer(charBuffer[0]);
                                    GetNextChar();
                                }
                                else
                                {
                                    var lexemRef = SearchInLexemDictionary();
                                    var typeRef = SearchInTypesDictionary();
                                    if (lexemRef.Item1 != -1)
                                    {
                                        AddLexem(LexemTypes.Indentifier, lexemRef.Item1, lexemRef.Item2);
                                        ClearBuffer();
                                    }
                                    else if (typeRef.Item1 != -1)
                                    {
                                        AddLexem(LexemTypes.DataType, typeRef.Item1, typeRef.Item2);
                                        ClearBuffer();
                                    }
                                    else
                                    {
                                        var variable = _variablesTable.Any(v => v.Name.Equals(buffer));
                                        if (!variable)
                                        {
                                            var variableType = lexems.LastOrDefault(c => c.Type == LexemTypes.DataType);
                                            if (variableType == null)
                                            {
                                                _state = LexemProcessorStates.Error;
                                                break;
                                            }
                                            _variablesTable.Add(new Variable(_variablesCounter++, variableType.Value, buffer));
                                            AddLexem(LexemTypes.Variable, _variablesTable.Count - 1, $"variable '{buffer}' of type <{variableType.Value}>");
                                            ClearBuffer();
                                        }
                                        else
                                        {
                                            AddLexem(LexemTypes.Variable, _variablesTable.FindIndex(c => c.Name == buffer), $"variable '{buffer}'");
                                            ClearBuffer();
                                        }
                                    }
                                    _state = LexemProcessorStates.Idle;
                                }
                                break;
                            }
                        case LexemProcessorStates.ReadingNum:
                            {
                                if (char.IsDigit(charBuffer[0]))
                                {
                                    AddToBuffer(charBuffer[0]);
                                    GetNextChar();
                                }
                                else
                                {
                                    AddLexem(LexemTypes.Constant, int.Parse(buffer), $"integer with value = {buffer}");
                                    ClearBuffer();
                                    _state = LexemProcessorStates.Idle;
                                }
                                break;
                            }
                        case LexemProcessorStates.Delimeter:
                            {
                                var searchResult = SearchInDelimeterDictionary();
                                var searchOperatorsResult = SearchInOperationsDictionary();


                                if (searchResult.Item1 != -1)
                                {
                                    AddLexem(LexemTypes.Delimeter, searchResult.Item1, searchResult.Item2);
                                    _state = LexemProcessorStates.Idle;
                                    ClearBuffer();
                                }
                                else if (searchOperatorsResult.Item1 != -1)
                                {
                                    seekingBuffer = new string(new char[] { buffer[0], charBuffer[0] });
                                    var seekOperatorsResult = SeekInOperationsDictionary();
                                    if (seekOperatorsResult.Item1 != -1)
                                    {
                                        AddLexem(LexemTypes.Operation, seekOperatorsResult.Item1, seekOperatorsResult.Item2);
                                        _state = LexemProcessorStates.Idle;
                                        ClearBuffer();
                                        GetNextChar();
                                    }
                                    else
                                    {
                                        AddLexem(LexemTypes.Operation, searchOperatorsResult.Item1, searchOperatorsResult.Item2);
                                        _state = LexemProcessorStates.Idle;
                                        ClearBuffer();
                                    }
                                }
                                else
                                {
                                    AddLexem(LexemTypes.ParsingError, -1, $"Error at {pointer}: Could not parse {buffer}!");
                                    _state = LexemProcessorStates.Error;
                                }
                                break;
                            }
                        case LexemProcessorStates.Error:
                            {
                                _state = LexemProcessorStates.Final;
                                break;
                            }
                        case LexemProcessorStates.Final:
                            {
                                return new Tuple<IList<Lexem>, IList<Variable>>(lexems, _variablesTable);
                            }
                    }
                }

                return new Tuple<IList<Lexem>, IList<Variable>>(lexems, _variablesTable);
            }
        }

        private void GetNextChar()
        {
            reader.Read(charBuffer, 0, 1);
            pointer++;
        }

        private bool IsEmptyOrNextLine(char input)
        {
            return input == ' '
                || input == '\n'
                || input == '\t'
                || input == '\0'
                || input == '\r';
        }
        public List<Lexem> getLexems()
        {
            return lexems;
        }
        public List<Variable> getVariables()
        {
            return _variablesTable;
        }

        private void ClearBuffer()
        {
            buffer = "";
            seekingBuffer = "";
        }

        private void AddToBuffer(char input)
        {
            buffer += input;
        }

        private void AddLexem(LexemTypes type, int value, string lex)
        {
            lexems.Add(new Lexem(type, value, lex));
        }

        private (int, string) SearchInLexemDictionary()
        {
            var result = Array.FindIndex(Constants.Constants.Keywords, l => l.Equals(buffer));

            if (result != -1)
            {
                return (result, buffer);
            }

            return (-1, buffer);
        }

        private (int, string) SearchInDelimeterDictionary()
        {
            var result = Array.FindIndex(Constants.Constants.KeySymbols, l => l.Equals(buffer));

            if (result != -1)
            {
                return (result, buffer);
            }

            return (-1, buffer);
        }

        private (int, string) SearchInTypesDictionary()
        {
            var searchResult = Constants.Constants.Types.ContainsKey(buffer);

            if (searchResult)
            {
                return Constants.Constants.Types[buffer];
            }

            return (-1, buffer);
        }

        private (int, string) SearchInOperationsDictionary()
        {
            var searchResult = Constants.Constants.Operators.ContainsKey(buffer);

            if (searchResult)
            {
                return Constants.Constants.Operators[buffer];
            }

            return (-1, buffer);
        }

        private (int, string) SeekInOperationsDictionary()
        {
            var searchResult = Constants.Constants.Operators.ContainsKey(seekingBuffer);

            if (searchResult)
            {
                return Constants.Constants.Operators[seekingBuffer];
            }

            return (-1, buffer);
        }
    }
}

