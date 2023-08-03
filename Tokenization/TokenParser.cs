using QLang.Utils;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace QLang.Tokenization;

internal sealed class TokenParser : ITokenParser
{
	private readonly IEnumerable<TokenType> _targetTypes;


	public TokenParser(IEnumerable<TokenType> targetTypes)
	{
		_targetTypes = targetTypes;
	}


	public bool TryParse(ReadOnlySpan<char> raw, [NotNullWhen(true)] out Token? result, [NotNullWhen(true)] out int charAte, Token.TextPosition textPosition = default)
	{
		foreach (var tokenType in _targetTypes)
		{
			if (TryParseToken(tokenType, raw, out result, out charAte, textPosition))
				return true;
			else continue;
		}

		result = null;
		charAte = 0;
		return false;
	}

	private bool TryParseToken(TokenType exceptedType, ReadOnlySpan<char> raw, [NotNullWhen(true)] out Token? result, [NotNullWhen(true)] out int charAte, Token.TextPosition textPosition = default)
	{
		var matches = Regex.EnumerateMatches(raw, exceptedType.RegularExpression, RegexOptions.Singleline);
		if (matches.MoveNext())
		{
			var match = matches.Current;
			var originalTextOfTokenSpan = raw[..match.Length];
			charAte = match.Length;


			string originalTextOfToken;
			if (exceptedType.Options.HasFlag(TokenType.ParsingOptions.ShouldIntern))
				originalTextOfToken = StringIntern.Intern(originalTextOfTokenSpan);
			else
				originalTextOfToken = new string(originalTextOfTokenSpan);


			if (exceptedType.ContentGroupIndex != -1)
			{
				var contentMatch = Regex.Match(originalTextOfToken, exceptedType.RegularExpression);
				var group = contentMatch.Groups[exceptedType.ContentGroupIndex];
				var content = originalTextOfToken.AsMemory(group.Index, group.Length);
				var parsedContent = exceptedType.Parser?.Invoke(content);

				result = new Token(exceptedType, originalTextOfToken, content, parsedContent) { Position = textPosition };
			}
			else
			{
				result = new Token(exceptedType, originalTextOfToken, ReadOnlyMemory<char>.Empty, null) { Position = textPosition };
			}

			return true;
		}
		else
		{
			result = null;
			charAte = 0;
			return false;
		}
	}
}