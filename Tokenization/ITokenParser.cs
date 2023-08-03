using System.Diagnostics.CodeAnalysis;

namespace QLang.Tokenization;

internal interface ITokenParser
{
	public bool TryParse(ReadOnlySpan<char> raw, [NotNullWhen(true)] out Token? result, [NotNullWhen(true)] out int charAte, Token.TextPosition textPosition = default);
}