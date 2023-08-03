using System.Diagnostics.CodeAnalysis;

namespace QLang.Tokenization;

internal record class TokenType
{
	public TokenType(string name, [StringSyntax(StringSyntaxAttribute.Regex)] string regularExpression, int contentGroupIndex, TokenContentParser? parser = null)
	{
		RegularExpression = '^' + regularExpression;
		ContentGroupIndex = contentGroupIndex;
		Parser = parser;
		Name = name;
	}


	public string Name { get; }

	public string RegularExpression { get; }
	
	public int ContentGroupIndex { get; }

	public TokenContentParser? Parser { get; } = null;

	public ParsingOptions Options { get; init; }


	[Flags]
	public enum ParsingOptions
	{
		ShouldIntern
	}
}