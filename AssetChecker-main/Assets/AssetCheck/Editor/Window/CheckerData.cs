using System;

namespace Assets.Checker
{
    internal class CheckerData
    {
        public string pattern { get; private set; }
        public Type checkerType;
        public string tip;
        public bool IsCheck = false;
        public CheckerData(Type checker, string tip, string pattern)
        {
            this.pattern = pattern;
            this.tip = tip;
            this.checkerType = checker;
        }
    }
}
