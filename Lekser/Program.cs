using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Lekser
{
    class Program
    {
        public static string Message { get; set; }


        private static List<Token> FindToken()
        {
            int position = 0;
            List<Token> tokens = new List<Token>();

            Console.WriteLine("wprowadz tekst: ");
            Message = Console.ReadLine();

            Console.WriteLine(Message);

            while (!string.IsNullOrEmpty(Message))
            {
                if (IsWhitespace(Message))
                {
                    position += GetWhitespace(Message).Length;
                    trimMessage(GetWhitespace(Message).Length);
                }
                else if (IsBracket(Message))
                {
                    tokens.Add(
                        new Token(
                            TokenType.Bracket,
                            GetBracket(Message)
                        ));
                    position += GetBracket(Message).Length;
                    trimMessage(GetBracket(Message).Length);
                }
                else if (IsOperator(Message))
                {
                    tokens.Add(
                        new Token(
                            TokenType.Operator,
                            GetOperator(Message)
                        ));
                    position += GetOperator(Message).Length;
                    trimMessage(GetOperator(Message).Length);
                }
                else if (IsIdentifier(Message))
                {
                    tokens.Add(
                        new Token(
                            TokenType.Identifier,
                            GetIdentifier(Message)
                        ));
                    position += GetIdentifier(Message).Length;
                    trimMessage(GetIdentifier(Message).Length);
                }
                else if (IsNumber(Message))
                {
                    if (IsDouble(Message))
                    {
                        tokens.Add(
                            new Token(
                                TokenType.Double,
                                GetDouble(Message)
                            ));
                        position += GetDouble(Message).Length;
                        trimMessage(GetDouble(Message).Length);
                    }
                    else
                    {
                        tokens.Add(
                            new Token(
                                TokenType.Int,
                                GetNumber(Message)
                            ));
                        position += GetNumber(Message).Length;
                        trimMessage(GetNumber(Message).Length);
                    }
                }
                else
                {
                    Console.WriteLine($"Program finds undefined symbol :  {Message.First()} on position {position}");
                    position++;
                    trimMessage(1);
                }

            }

            return tokens;
        }

        private static void DisplayTokens(List<Token> tokens)
        {
            foreach (var token in tokens)
            {
                Console.WriteLine("Token: ");
                Console.WriteLine($"type: {token.Type}");
                Console.WriteLine($"value: {token.Value} \n");
            }
        }

        private static void trimMessage(int trimLength)
        {
            Message = Message.Substring(trimLength);
        }

        private static string GetWhitespace(string symbol)
        {
            var spacePattern = @"^\s+";
            return Regex.Match(symbol, spacePattern)
                .ToString();
        }

        private static bool IsWhitespace(string symbol)
        {
            var spacePattern = @"^\s";
            return Regex.IsMatch(symbol, spacePattern);
        }

        private static string GetBracket(string symbol)
        {
            var bracketPattern = @"^[()]";
            return Regex.Match(symbol, bracketPattern)
                .ToString();
        }

        private static bool IsBracket(string symbol)
        {
            var bracketPattern = @"^[()]";
            return Regex.IsMatch(symbol, bracketPattern);
        }

        private static string GetIdentifier(string symbol)
        {
            var identifierPattern = @"^[a-zA-z]+\w*";
            return Regex.Match(symbol, identifierPattern).ToString();
        }

        private static bool IsIdentifier(string symbol)
        {
            var identifierPattern = @"^[a-zA-z]+\w*";
            return Regex.IsMatch(symbol, identifierPattern);
        }

        private static string GetNumber(string symbol)
        {
            var numberPattern = @"^\d+";
            return Regex.Match(symbol, numberPattern).ToString();
        }

        private static bool IsNumber(string symbol)
        {
            var numberPattern = @"^\d+";
            return Regex.IsMatch(symbol, numberPattern);
        }

        private static string GetDouble(string symbol)
        {
            var doublePattern = @"^\d+\.\d+";
            return Regex.Match(symbol, doublePattern).ToString();
        }

        private static bool IsDouble(string symbol)
        {
            var doublePattern = @"^\d+\.\d+";
            return Regex.IsMatch(symbol, doublePattern);
        }

        private static string GetOperator(string symbol)
        {
            var operatorPattern = @"^[-+*\/\\=]";
            return Regex.Match(symbol, operatorPattern).ToString();
        }

        private static bool IsOperator(string symbol)
        {
            var operatorPattern = @"^[-+*\/\\=]";
            return Regex.IsMatch(symbol, operatorPattern);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                DisplayTokens(FindToken());
            }
        }
    }
}