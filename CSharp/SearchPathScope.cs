using System.Collections.Generic;

namespace Domemtech.Symtab
{
    public class SearchPathScope : BaseScope
    {
        private readonly List<IScope> ordered_scopes = new List<IScope>();

        public SearchPathScope(IScope enclosingScope) : base(enclosingScope)
        {
        }
    }
}
