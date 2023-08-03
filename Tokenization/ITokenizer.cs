namespace QLang.Tokenization;

internal interface ITokenizer
{
	public TokenSequence Tokenize(string rawText);
}