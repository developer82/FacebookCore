namespace FacebookCore
{
    public class FacebookCursor
    {
        public enum Direction
        {
            None,
            Next,
            Previous,
            Before,
            After
        }

        public string Before { get; internal set; }
        public string After { get; internal set; }

        public FacebookCursor()
        {
            
        }

        public FacebookCursor(string before, string after)
        {
            Before = before;
            After = after;
        }
    }
}
