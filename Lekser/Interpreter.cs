using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lekser
{
    public class Interpreter
    {
        // (20 - (30 + 20)) = 30 
        public bool IsValidExpression(List<Token> tokens)
        {
            if (!equalsOperatorMatch(tokens))
            {
                throw new Exception("Exception: Equals operator error !");
            }

            if (!BracketsMatch(tokens))
            {
                throw new Exception("Exception: Brackets don't match !");
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                if (!checkToken(tokens, i))
                {
                    return true;
                }
            }

            return true;
        }

        bool checkToken(List<Token> tokens, int index)
        {
            if (tokens[index].Type != TokenType.Operator)
                return true;

            if (tokens[index].Type == TokenType.Bracket)
            {
                if (index == 0) return true;
                checkToken(tokens, index - 1);
            }

            if (index == 0)
            {
                throw new Exception($"Exception: Expression starts with not expected operator at index: {index}!");
            }

            if (!(isNumber(tokens[index - 1]) || isIdentifier(tokens[index - 1])))
            {
                if (tokens[index - 1].Type == TokenType.Bracket)
                {
                    checkToken(tokens, index - 1);
                }
                else
                {
                    throw new Exception($"Exception: Not expected token at index: {index}!");
                }
            }
            else
            {
                if (tokens[index].Value == "=")
                {
                    return true;
                }

                if (index - 1 == 0)
                {
                    return true;
                }

                if (tokens[index - 2].Type == TokenType.Int ||
                    tokens[index - 2].Type == TokenType.Double ||
                    tokens[index - 2].Type == TokenType.Identifier)
                {
                    throw new Exception($"Exception: Missing operator between elements at index: {index}!");
                }
            }

            return true;
        }

        private bool isNumber(Token token)
        {
            return token.Type == TokenType.Int || token.Type == TokenType.Double;
        }

        private bool isIdentifier(Token token)
        {
            return token.Type == TokenType.Identifier;
        }

        private bool equalsOperatorMatch(List<Token> tokens)
        {
            var equalOperatorCount =
                tokens.FindAll(x => x.Type == TokenType.Operator && x.Value == "=").Count;

            return equalOperatorCount <= 1;
        }

        private bool BracketsMatch(List<Token> tokens)
        {
            var brackets = tokens.FindAll(x => x.Type == TokenType.Bracket);

            if (brackets.Count == 0) return true;

            int leftBracketsCount =
                tokens.FindAll(x =>
                        x.Type == TokenType.Bracket && x.Value == "("
                    )
                    .Count;

            int rightBracketsCount = tokens.FindAll(x =>
                    x.Type == TokenType.Bracket && x.Value == ")"
                )
                .Count;


            if (HaveEmptyBrackets(tokens))
            {
                throw new Exception("Exception: Empty brackets in expression!");
            }

            if (IsLeftBracketFirst(brackets))
            {
                return leftBracketsCount == rightBracketsCount;
            }

            return false;
        }

        private bool IsLeftBracketFirst(List<Token> tokens)
        {
            return IsLeftBracket(tokens.First());
        }

        private bool IsLeftBracket(Token token)
        {
            return token.Value == "(";
        }

        private bool HaveEmptyBrackets(List<Token> tokens)
        {
            return Regex.IsMatch(TokensToString(tokens), "\\(\\s*\\)");
        }


        private String TokensToString(List<Token> tokens)
        {
            var valueList = tokens.Select(x => x.Value);
            return string.Join("", valueList.ToArray());
        }
    }
}