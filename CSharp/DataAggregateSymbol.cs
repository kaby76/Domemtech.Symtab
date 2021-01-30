﻿namespace Domemtech.Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    /// <summary>
    /// A symbol representing a collection of data like a struct or class.
    ///  Each member has a slot number indexed from 0 and we track data fields
    ///  and methods with different slot sequences. A DataAggregateSymbol
    ///  can also be a member of an aggregate itself (nested structs, ...).
    /// </summary>
    public abstract class DataAggregateSymbol : SymbolWithScope, IMemberSymbol, ISymbol, IType
    {
        protected internal ParserRuleContext defNode;
        protected internal int nextFreeFieldSlot = 0; // next slot to allocate
        protected internal int typeIndex;

        public DataAggregateSymbol(string n, IList<IToken> t) : base(n, t)
        {
        }

        public virtual ParserRuleContext DefNode
        {
            set => defNode = value;
            get => defNode;
        }


        public override void define(ref ISymbol sym)
        {
            if (!(sym is IMemberSymbol))
            {
                throw new System.ArgumentException("sym is " + sym.GetType().Name + " not MemberSymbol");
            }
            base.define(ref sym);
            setSlotNumber(sym);
        }

        public override IList<ISymbol> Symbols => base.Symbols;

        public override SymtabMultiMap<string, ISymbol> Members => base.Members;

        /// <summary>
        /// Look up name within this scope only. Return any kind of MemberSymbol found
        ///  or null if nothing with this name found as MemberSymbol.
        /// </summary>
        public virtual IList<ISymbol> resolveMember(string name)
        {
            List<ISymbol> result = new List<ISymbol>();
            List<ISymbol> ss = symbols[name];
            foreach (var s in ss)
            {
                if (s is IMemberSymbol)
                {
                    result.Add(s);
                }
            }
            return result;
        }

        /// <summary>
        /// Look for a field with this name in this scope only.
        ///  Return null if no field found.
        /// </summary>
        public virtual IList<FieldSymbol> resolveField(string name)
        {
            List<FieldSymbol> result = new List<FieldSymbol>();
            IList<ISymbol> list = resolveMember(name);
            foreach (ISymbol s in list)
            {
                if (s is FieldSymbol)
                {
                    result.Add(s as FieldSymbol);
                }
            }

            return result;
        }

        /// <summary>
        /// get the number of fields defined specifically in this class </summary>
        public virtual int NumberOfDefinedFields
        {
            get
            {
                int n = 0;
                foreach (IMemberSymbol s in Symbols)
                {
                    if (s is FieldSymbol)
                    {
                        n++;
                    }
                }
                return n;
            }
        }

        /// <summary>
        /// Get the total number of fields visible to this class </summary>
        public virtual int NumberOfFields => NumberOfDefinedFields;

        /// <summary>
        /// Return the list of fields in this specific aggregate </summary>
        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: public java.util.List<? extends FieldSymbol> getDefinedFields()
        public virtual IList<FieldSymbol> DefinedFields
        {
            get
            {
                IList<FieldSymbol> fields = new List<FieldSymbol>();
                foreach (IMemberSymbol s in Symbols)
                {
                    if (s is FieldSymbol)
                    {
                        fields.Add((FieldSymbol)s);
                    }
                }
                return fields;
            }
        }

        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: public java.util.List<? extends FieldSymbol> getFields()
        public virtual IList<FieldSymbol> Fields => DefinedFields;

        public virtual void setSlotNumber(ISymbol sym)
        {
            if (sym is FieldSymbol)
            {
                FieldSymbol fsym = (FieldSymbol)sym;
                fsym.slot = nextFreeFieldSlot++;
            }
        }

        public virtual int getSlotNumber()
        {
            return -1; // class definitions do not yield either field or method slots; they are just nested
        }

        public virtual int TypeIndex
        {
            get => typeIndex;
            set => typeIndex = value;
        }

    }

}