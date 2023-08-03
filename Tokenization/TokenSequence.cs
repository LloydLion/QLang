using System.Collections;

namespace QLang.Tokenization;

internal sealed class TokenSequence : IReadOnlyList<Token>
{
	private readonly Token[] _tokens;


	public TokenSequence(IEnumerable<Token> tokens)
	{
		_tokens = tokens.ToArray();
	}


	public Token this[int index] => _tokens[index];

	public int Count => _tokens.Length;


	public Iterator Iterate() => new(this);

	IEnumerator<Token> IEnumerable<Token>.GetEnumerator() => Iterate();

	IEnumerator IEnumerable.GetEnumerator() => Iterate();


	public sealed class Iterator : IEnumerator<Token>
	{
		private int _currentIndex = -1;
		private readonly TokenSequence _owner;


		public Iterator(TokenSequence owner)
		{
			_owner = owner;
		}


		public Token Current => _owner[_currentIndex];

		public TokenSequence Sequence => _owner;

		object IEnumerator.Current => Current;


		public Token Peek(int offset) => _owner[_currentIndex + offset];

		public void Seek(int offset) => _currentIndex += offset;

		public bool Available(int offset) => _owner.Count > _currentIndex + offset;

		public int ExceptInFuture(Predicate<Token> predicate)
		{
			int offset = 1;
			while (predicate(Peek(offset)) == false)
			{
				offset++;
				if (Available(offset) == false)
					return -1;
			}

			return offset;
		}

		public bool MoveNext()
		{
			Seek(offset: 1);
			return Available(offset: 0);
		}

		public void Dispose() { }

		public void Reset() => throw new NotSupportedException();
	}
}