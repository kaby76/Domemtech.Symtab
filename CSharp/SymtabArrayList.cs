using System.Collections.Generic;

namespace Symtab
{
    public class SymtabArrayList<T> : List<T>
	{

		public SymtabArrayList()
		{
		}

		public SymtabArrayList(int count)
			: base(count)
		{
		}

		public override int GetHashCode()
		{
			int hash = SymtabMurmurHash.Initialize(1);
			foreach (T t in this)
				hash = SymtabMurmurHash.Update(hash, t.GetHashCode());
			hash = SymtabMurmurHash.Finish(hash, this.Count);
			return hash;
		}

		public override bool Equals(object o)
		{
			return o == this
				|| (o is List<T> list && Equals(list));
		}


		public bool Equals(List<T> o)
		{
			if (this.Count != o.Count)
				return false;
			IEnumerator<T> thisItems = this.GetEnumerator();
			IEnumerator<T> otherItems = o.GetEnumerator();
			while (thisItems.MoveNext() && otherItems.MoveNext())
			{
				if (!thisItems.Current.Equals(otherItems.Current))
					return false;
			}
			return true;

		}

	}
}
