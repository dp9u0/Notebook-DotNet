﻿
using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;

namespace AppDomainSample {
    class AppDomainRunner {

        public static void Marshalling() {
            // Get a reference to the AppDomain that the calling thread is executing in
            AppDomain adCallingThreadDomain = Thread.GetDomain();
            // Every AppDomain is assigned a friendly string name (helpful for debugging)
            // Get this AppDomain's friendly string name and display it
            String callingDomainName = adCallingThreadDomain.FriendlyName;
            Console.WriteLine("Default AppDomain's friendly name={0}", callingDomainName);
            // Get and display the assembly in our AppDomain that contains the 'Main' method
            String exeAssembly = Assembly.GetEntryAssembly().FullName;
            Console.WriteLine("Main assembly={0}", exeAssembly);


            // Define a local variable that can refer to an AppDomain
            AppDomain ad2 = null;

            // *** DEMO 1: Cross-AppDomain Communication Using Marshal-by-Reference ***
            Console.WriteLine("{0}Demo #1", Environment.NewLine);
            // Create new AppDomain (security and configuration match current AppDomain)
            ad2 = AppDomain.CreateDomain("AD #2 Demo #1", null, adCallingThreadDomain.SetupInformation);
            // Load our assembly into the new AppDomain, construct an object, marshal 
            // it back to our AD (we really get a reference to a proxy)
            MarshalByRefType mbrt = (MarshalByRefType)
            ad2.CreateInstanceAndUnwrap(typeof(MarshalByRefType).Assembly.FullName, typeof(MarshalByRefType).FullName);

            Console.WriteLine("Type={0}", mbrt.GetType()); // The CLR lies about the type
                                                           // Prove that we got a reference to a proxy object
            Console.WriteLine("Is proxy={0}", RemotingServices.IsTransparentProxy(mbrt));
            // This looks like we're calling a method on MarshalByRefType but we're not.
            // We're calling a method on the proxy type. The proxy transitions the thread
            // to the AppDomain owning the object and calls this method on the real object.
            mbrt.SomeMethod();
            AppDomain.Unload(ad2);// Unload the new AppDomain
            // mbrt refers to a valid proxy object; the proxy object refers to an invalid AppDomain
            try {
                // We're calling a method on the proxy type. The AD is invalid, exception is thrown
                mbrt.SomeMethod();
                Console.WriteLine("Successful call.");
            } catch (AppDomainUnloadedException) {
                Console.WriteLine("Failed call.");
            }

            // *** DEMO 2: Cross-AppDomain Communication Using Marshal-by-Value ***
            Console.WriteLine("{0}Demo #2", Environment.NewLine);
            // Create new AppDomain (security and configuration match current AppDomain)
            ad2 = AppDomain.CreateDomain("AD #2 Demo #2", null, null);

            // Load our assembly into the new AppDomain, construct an object, marshal 
            // it back to our AD (we really get a reference to a proxy)
            mbrt = (MarshalByRefType)
            ad2.CreateInstanceAndUnwrap(typeof(MarshalByRefType).Assembly.FullName, typeof(MarshalByRefType).FullName);

            // The object's method returns a COPY of the returned object; 
            // the object is marshaled by value (not by reference).
            MarshalByValType mbvt = mbrt.MethodWithReturn();
            // Prove that we did NOT get a reference to a proxy object
            Console.WriteLine("Is proxy={0}", RemotingServices.IsTransparentProxy(mbvt));
            // This looks like we're calling a method on MarshalByValType and we are.
            Console.WriteLine("Returned object created " + mbvt.SomeMethod());

            // Unload the new AppDomain
            AppDomain.Unload(ad2);
            // mbvt refers to valid object; unloading the AppDomain has no impact.
            try {
                // We're calling a method on an object; no exception is thrown
                Console.WriteLine("Returned object created " + mbvt.SomeMethod());
                Console.WriteLine("Successful call.");
            } catch (AppDomainUnloadedException) {
                Console.WriteLine("Failed call.");
            }


            // DEMO 3: Cross-AppDomain Communication Using non-marshalable type ***
            Console.WriteLine("{0}Demo #3", Environment.NewLine);
            // Create new AppDomain (security and configuration match current AppDomain)
            ad2 = AppDomain.CreateDomain("AD #2 Demo #3");// Load our assembly into the new AppDomain, construct an object, marshal 
                                                          // it back to our AD (we really get a reference to a proxy)
            mbrt = (MarshalByRefType)
            ad2.CreateInstanceAndUnwrap(typeof(MarshalByRefType).Assembly.FullName, typeof(MarshalByRefType).FullName);
            // The object's method returns a non-marshalable object; exception
            NonMarshalableType nmt = mbrt.MethodArgAndReturn(callingDomainName);
            // We won't get here...
        }
    }     // Instances can be marshaled-by-reference across AppDomain boundaries

    public sealed class MarshalByRefType : MarshalByRefObject {
        public MarshalByRefType() {
            Console.WriteLine("{0} ctor running in {1}",
            this.GetType().ToString(), Thread.GetDomain().FriendlyName);
        }
        public void SomeMethod() {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
        }
        public MarshalByValType MethodWithReturn() {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
            MarshalByValType t = new MarshalByValType();
            return t;
        }
        public NonMarshalableType MethodArgAndReturn(String callingDomainName) {
            // NOTE: callingDomainName is [Serializable]
            Console.WriteLine("Calling from '{0}' to '{1}'.",
            callingDomainName, Thread.GetDomain().FriendlyName);
            NonMarshalableType t = new NonMarshalableType();
            return t;
        }
    }
    // Instances can be marshaled-by-value across AppDomain boundaries
    [Serializable]
    public sealed class MarshalByValType : object {
        private DateTime m_creationTime = DateTime.Now; // NOTE: DateTime is [Serializable]
        public MarshalByValType() {
            Console.WriteLine("{0} ctor running in {1}, Created on {2:D}",
            this.GetType().ToString(),
            Thread.GetDomain().FriendlyName,
            m_creationTime);
        }

        public String SomeMethod() {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
            return m_creationTime.ToLongDateString();
        }
    }

    public sealed class NonMarshalableType : object {
        public NonMarshalableType() {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
        }
    }
}
