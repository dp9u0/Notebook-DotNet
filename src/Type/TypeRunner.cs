using Common;

namespace Type {

    /// <summary>
    /// TypeRunner
    /// </summary>
    public class TypeRunner : Runner {

        protected override void RunCore() {
            RunRunner<AllDerivedFromObjectRunner>();
            RunRunner<PrimitiveTypeRunner>();
            RunRunner<ValueAndReferenceTypeRunner>();
            RunRunner<ValueTypeLayoutRunner>();
            RunRunner<ConvertRunner>();
            RunRunner<BoxUnboxRunner>();
            RunRunner<DynamicRunner>();
            RunRunner<TypeMemberRunner>();
            RunRunner<FieldRunner>();
            RunRunner<MethodRunner>();
            RunRunner<MethodRunner2>();
            RunRunner<PropertyRunner>();
            RunRunner<EventRunner>();
            RunRunner<GenericsRunner>();
            RunRunner<InterfaceRunner>();
            RunRunner<StringRunner>();
            RunRunner<FormatRunner>();
        }
    }
}
