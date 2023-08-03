namespace QLang.Tokenization;

internal readonly record struct Token(TokenType Type, string OriginalText, ReadOnlyMemory<char> RawContent, object? Content)
{
	public TextPosition Position { get; init; }


	public override string ToString()
	{
		if (Content is not null)
			return $"[{Position} {Type.Name}| Content:{{{Content}}}]";
		else if (RawContent.IsEmpty == false)
			return $"[{Position} {Type.Name}| Content (Raw):\"{RawContent}\"]";
		else
			return $"[{Position} {Type.Name}]";
	}


	public readonly record struct TextPosition(int Line, int Character, int AbsoluteIndex)
	{
		public override string? ToString()
		{
			return $"(line: {Line}, char: {Character})";
		}
	}
}