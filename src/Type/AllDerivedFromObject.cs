#region

using System;
using Common;

#endregion

namespace Type {
    /// <summary>
    /// </summary>
    public class AllDerivedFromObject : IRunner {
        /// <summary>
        /// </summary>
        public void Run() {
            Console.WriteLine("Nothing to run");
        }
    }

    /// <summary>
    /// interface
    /// </summary>
    interface IInterface { }

    /// <summary>
    /// class
    /// </summary>
    public class ClassType { }

    /// <summary>
    ///     Declare delegate us key word 'delegate' instead of 'extends MulticastDelegate'
    ///     <example>
    ///         Cannot declare DelegateType like this:
    ///         <code>
    ///         public class DelegateType : MulticastDelegate { 
    ///             public DelegateType(object target, string method) : base(target, method) { } 
    ///         };
    ///     </code>
    ///         Cause Compiler hardcoded:
    ///         https://stackoverflow.com/questions/2324667/why-cant-i-derive-from-system-enum-abstract-class
    ///         <code>
    ///         switch (type.SpecialType)
    ///         {
    ///             case SpecialType.System_Object:
    ///             case SpecialType.System_ValueType:
    ///             case SpecialType.System_Enum:
    ///             case SpecialType.System_Delegate:
    ///             case SpecialType.System_MulticastDelegate:
    ///             case SpecialType.System_Array:
    ///             Error(diagnostics, ErrorCode.ERR_SpecialTypeAsBound, syntax, type);
    ///             return false;
    ///         }
    ///     </code>
    ///         And When Decompiled <see cref="DelegateType" /> into IL Code
    ///         <code>
    ///         .class public auto ansi sealed Type.DelegateType
    ///         extends[netstandard] System.MulticastDelegate</code>
    ///     </example>
    /// Also See <see cref="EnumType"/>(enum) And <see cref="StructType"/>(struct)
    /// </summary>
    public delegate object DelegateType(object arg);

    /// <summary>
    /// struct
    /// </summary>
    public struct StructType { }

    /// <summary>
    /// enum
    /// </summary>
    public enum EnumType { }
}