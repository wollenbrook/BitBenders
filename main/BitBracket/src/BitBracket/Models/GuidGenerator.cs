using System;

namespace BitBracket.Models
{
    public class GuidGenerator
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}