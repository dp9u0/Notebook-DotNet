namespace Linq {
    static class DataSource {

        private static SimpleQueryProvider Provider = new SimpleQueryProvider();

        internal static SimpleQueryable<T> Create<T>() {
            return new SimpleQueryable<T>(Provider);
        }
    }
}
