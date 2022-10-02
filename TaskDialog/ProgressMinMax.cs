namespace WindowsControls
{
    public struct ProgressMinMax
    {
        private uint value;

        public ushort Max => (ushort)((value >> 16) & 0xFFFF);

        public ushort Min => (ushort)(value & 0xFFFF);

        internal ProgressMinMax(uint value)
        {
            this.value = value;
        }
    }
}
