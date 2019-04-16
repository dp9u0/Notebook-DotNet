using Common;

namespace Type {

    /// <summary>
    /// TypeRunner
    /// </summary>
    public class TypeRunner : Runner {
        protected override void RunCore() {
            new AllDerivedFromObjectRunner().Run();
            new PrimitiveTypeRunner().Run();
            new ValueAndReferenceTypeRunner().Run();
            new ValueTypeLayoutRunner().Run();
            new ConvertRunner().Run();
            new BoxUnboxRunner().Run();
            new DynamicRunner().Run();
        }
    }
}
