using System;

namespace App
{
    public class EscapeException : Exception
    {
        public EscapeException() : base("Escape key was pressed.") { }
    }
}
