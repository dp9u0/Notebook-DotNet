using Common;

namespace Type {
    class EnumRunner : Runner {
        protected override void RunCore() {

        }

        public enum SomeEnum {
            White, // Assigned a value of 0 
            Red, // Assigned a value of 1 
            Green, // Assigned a value of 2 
            Blue, // Assigned a value of 3 
            Orange // Assigned a value of 4
        }
    }
}
