using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace QLang.Tokenization;

internal sealed class TokenParser : ITokenParser
{
	private readonly TokenType _targetType;


	public TokenParser(TokenType targetType)
	{
		_targetType = targetType;
	}


	public bool TryParse(ReadOnlySpan<char> raw, [NotNullWhen(true)] out Token? result, [NotNullWhen(true)] out int charAte, Token.TextPosition textPosition = default)
	{
		var matches = Regex.EnumerateMatches(raw, _targetType.RegularExpression, RegexOptions.Singleline);
		if (matches.MoveNext())
		{
			var match = matches.Current;
			var originalTextOfToken = new string(raw[..match.Length]);
			charAte = match.Length;

			if (_targetType.Options.HasFlag(TokenType.ParsingOptions.ShouldIntern))
				originalTextOfToken = string.Intern(originalTextOfToken);


			if (_targetType.ContentGroupIndex != -1)
			{
				var contentMatch = Regex.Match(originalTextOfToken, _targetType.RegularExpression);
				var group = contentMatch.Groups[_targetType.ContentGroupIndex];
				var content = originalTextOfToken.AsMemory(group.Index, group.Length);
				var parsedContent = _targetType.Parser?.Invoke(content);

				result = new Token(_targetType, originalTextOfToken, content, parsedContent) { Position = textPosition };
			}
			else
			{
				result = new Token(_targetType, originalTextOfToken, ReadOnlyMemory<char>.Empty, null) { Position = textPosition };
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