
using QLang.Tokenization;

var rawText =
"""
{{
	print "Hello world"

	}}
	print{ "Again {hello world"

""";

var parsers = TokenTypes.All.Select(s => new TokenParser(s)).ToArray();
ITokenizer tokenizer = new Tokenizer(parsers);

var tokenSequence = tokenizer.Tokenize(rawText);

foreach (var token in tokenSequence)
{
	Console.WriteLine(token);
}