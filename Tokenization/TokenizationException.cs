namespace QLang.Tokenization;

internal class TokenizationException : Exception
{
	public TokenizationException(string? message, Exception? innerException = null) : base(message, innerException)
	{

	}
}