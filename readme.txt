PariSharp aims to integrate PARI, a high-performance CAS written in C, into C#, which is an excellent language, but slow and lacking in mathematical functionality (even with System.Numerics).  In doing so, there are several sub-goals:
*Provide an intuitive interface to PARI, complete with IntelliSense documentation
*Make the interface object-oriented, strongly typed, and type-safe
*Replace PARI's custom exception system, which cannot be wrapped, with C#-style error-checking and exceptions
*Eliminate the need for client code to deal with pointers, and minimize the need for client code to explicitly free memory
*Bring the naming scheme for the public interface in line with C# standards

There are a few caveats:
*PariSharp will necessarily perform poorly in comparison to C programs that use the PARI library directly
*Client code is still required to explicitly initialize the GP engine before making any other calls to the PARI library, and to ensure that the PARI stack does not overflow
*Programs dependent on PariSharp cannot target 64-bit architecture, as the PARI library itself is 32-bit only
*PariSharp is not thread-safe or compatible with versions of the PARI library compiled with threading enabled, and may not ever be
*PariSharp is, as yet, not being tested with the GMP

PariSharp is being developed by a single programmer, and is very much a work in progress.  Currently, little is wrapped but prime sieves and operations on integers.

PariSharp is provided as-is, with no license (but will, of course, be utterly useless without PARI, which is licensed under the GPL).

For information on the PARI library, go to pari.math.u-bordeaux.fr.
