
using QLang.Tokenization;
using System.Text;

var rawText =
"""
{{
	print "Hello world"
	
	}}
	print{ "Again {hello world"

""";

var parser = new TokenParser(TokenTypes.All);
ITokenizer tokenizer = new Tokenizer(parser);

var tokenSequence = tokenizer.Tokenize(rawText);

tokenSequence.GetHashCode();
