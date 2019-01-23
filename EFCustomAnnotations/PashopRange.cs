using System;

namespace EFCustomAnnotations
{
    public class PashopRange : Attribute
    {
        private int min;
        private int max;

        public PashopRange(int min, int max)
        {
            min = this.min;
            max = this.max;
        }

    }
}