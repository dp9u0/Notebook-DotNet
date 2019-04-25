namespace ClrSample.Lib {
    class Utils {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="index"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        bool TryGetInt(out int result, int index, params string[] args) {
            result = 0;
            if (args.Length > index) {
                return int.TryParse(args[index], out result);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="index"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        bool Match(string expect, int index, params string[] args) {
            if (args.Length > index) {
                return string.Equals(expect, args[index]);
            }
            return false;
        }
    }
}
