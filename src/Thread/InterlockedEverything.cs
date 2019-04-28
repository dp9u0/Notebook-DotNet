using System;
using System.Threading;

namespace ThreadSample {

    public static class InterlockedEverything {

        delegate Int32 Morpher<TResult, TArgument>(Int32 startValue, TArgument argument,
     out TResult morphResult);

        static TResult Morph<TResult, TArgument>(ref Int32 target, TArgument argument,
         Morpher<TResult, TArgument> morpher) {
            TResult morphResult;
            Int32 currentVal = target, startVal, desiredVal;
            do {
                startVal = currentVal;
                desiredVal = morpher(startVal, argument, out morphResult);
                currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);
            } while (startVal != currentVal);
            return morphResult;
        }
    }
}
