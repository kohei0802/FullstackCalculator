using CsvHelper.Configuration.Attributes;

namespace ClassLibrary1
{
    public class MathExpression
    {
        //public int HistoryCount { get { return histoutcunt; } , set{histoutcunt=value}}

        [Index(0)]
        public decimal? Left { get; set; } = null;
        [Index(1)]
        public string Operation { get; set; } = null;
        [Index(2)]
        public decimal? Right { get; set; } = null;
        [Index(3)]
        public decimal? Result { get; set; } = null;
        public MathExpression() { }

        public MathExpression GetCopy()
        {
            return new MathExpression()
            {
                Left = this.Left,
                Operation = this.Operation,
                Right = this.Right,
                Result = this.Result
            }; 
        }

    }


}