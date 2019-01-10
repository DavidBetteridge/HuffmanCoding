namespace HuffmanCoding
{
    internal class LeafNode : INode
    {
        public int Weight { get; }
        public char Key { get; }

        public LeafNode(char key, int Weight)
        {
            this.Key = key;
            this.Weight = Weight;
        }

        public override string ToString() => $"{Key} {Weight}";
    }
}
