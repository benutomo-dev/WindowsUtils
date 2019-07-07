using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
