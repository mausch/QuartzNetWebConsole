namespace QuartzNetWebConsole.Utils {
    public sealed class RelativeUri {
        public readonly string PathAndQuery;

        public RelativeUri(string pathAndQuery) {
            PathAndQuery = pathAndQuery;
        }

        public string Query {
            get {
                var parts = PathAndQuery.Split('?');
                if (parts.Length < 2)
                    return "";
                return parts[1];
            }
        }

        public string Path {
            get {
                return PathAndQuery.Split('?')[0];
            }
        }

        public override string ToString() {
            return PathAndQuery;
        }
    }
}