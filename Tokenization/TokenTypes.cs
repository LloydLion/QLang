using System.Reflection;

namespace QLang.Tokenization;

internal static class TokenTypes
{
	public static TokenType Whitespace { get; } = new TokenType(nameof(Whitespace), @"\s+", -1) { Options = TokenType.ParsingOptions.ShouldIntern };

	public static TokenType BlockStart { get; } = new TokenType(nameof(BlockStart), @"{", -1) { Options = TokenType.ParsingOptions.ShouldIntern };

	public static TokenType BlockEnd { get; } = new TokenType(nameof(BlockEnd), @"}", -1) { Options = TokenType.ParsingOptions.ShouldIntern };

	public static TokenType PrintOperator { get; } = new TokenType(nameof(PrintOperator), @"print", -1) { Options = TokenType.ParsingOptions.ShouldIntern };

	public static TokenType StringLiteral { get; } = new TokenType(nameof(StringLiteral), "\"([^\"]*)\"", 1);

	public static IReadOnlyCollection<TokenType> All { get; } =
		typeof(TokenTypes).GetProperties(BindingFlags.Public | BindingFlags.Static).Where(s => s.PropertyType == typeof(TokenType)).Select(s => (TokenType)s.GetValue(null)!).ToArray();
}