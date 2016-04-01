using System;

namespace Consolidate
{
    public class Consolidator
    {
        private static readonly Lazy<Consolidator> Lazy = new Lazy<Consolidator>(() => new Consolidator());

        public static Consolidator Instance => Lazy.Value;

        private Consolidator()
        { }

        public void Execute()
        {
            
        }
    }
}
