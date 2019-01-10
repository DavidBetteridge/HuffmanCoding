namespace HuffmanCoding
{
    internal class CombinedNode : INode
    {
        public INode LHS { get; }
        public INode RHS { get; }
        public int Weight => LHS.Weight + RHS.Weight;

        public CombinedNode(INode lhs, INode rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }
    }
}