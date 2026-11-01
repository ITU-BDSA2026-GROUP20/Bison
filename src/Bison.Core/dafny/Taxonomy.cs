// Dafny program Taxonomy.dfy compiled into C#
// To recompile, you will need the libraries
//     System.Runtime.Numerics.dll System.Collections.Immutable.dll
// but the 'dotnet' tool in .NET should pick those up automatically.
// Optionally, you may want to include compiler switches like
//     /debug /nowarn:162,164,168,183,219,436,1717,1718

using System;
using System.Numerics;
using System.Collections;
[assembly: DafnyAssembly.DafnySourceAttribute(@"// dafny 4.11.0.0
// Command-line arguments: translate cs src/Bison.Core/dafny/Taxonomy.dfy
// Taxonomy.dfy


module {:extern ""Bison.Core.models""} Models {
  class {:extern} Taxon {
    function {:extern} {:axiom} IsSubTaxonOf(other: Taxon): bool
      reads {}
      decreases set _s2s_0: object? {:trigger _s2s_0 in {}} | _s2s_0 in {} :: _s2s_0, other
  }

  class {:extern} Observation {
    function {:extern} {:axiom} GetTaxon(): Taxon
      reads {}
      decreases set _s2s_0: object? {:trigger _s2s_0 in {}} | _s2s_0 in {} :: _s2s_0
  }
}

module {:extern ""Bison.Core.dafny.Taxonomy""} Taxonomy {
  function FilterByTaxon(root: Taxon, obs: seq<Observation>): (result: seq<Observation>)
    ensures forall o: Observation {:trigger o.GetTaxon()} {:trigger o in obs} {:trigger o in result} :: (o in result ==> o in obs) && (o in result ==> o.GetTaxon().IsSubTaxonOf(root))
    ensures forall o: Observation {:trigger o in result} {:trigger o.GetTaxon()} {:trigger o in obs} :: o in obs && o.GetTaxon().IsSubTaxonOf(root) ==> o in result
    decreases |obs|
  {
    if |obs| == 0 then
      []
    else if obs[0].GetTaxon().IsSubTaxonOf(root) then
      [obs[0]] + FilterByTaxon(root, obs[1..])
    else
      FilterByTaxon(root, obs[1..])
  }

  import opened Models
}
")]

namespace Dafny {
  internal class ArrayHelpers {
    public static T[] InitNewArray1<T>(T z, BigInteger size0) {
      int s0 = (int)size0;
      T[] a = new T[s0];
      for (int i0 = 0; i0 < s0; i0++) {
        a[i0] = z;
      }
      return a;
    }
  }
} // end of namespace Dafny
internal static class FuncExtensions {
  public static Func<UResult> DowncastClone<TResult, UResult>(this Func<TResult> F, Func<TResult, UResult> ResConv) {
    return () => ResConv(F());
  }
  public static Func<U, UResult> DowncastClone<T, TResult, U, UResult>(this Func<T, TResult> F, Func<U, T> ArgConv, Func<TResult, UResult> ResConv) {
    return arg => ResConv(F(ArgConv(arg)));
  }
  public static Func<U1, U2, UResult> DowncastClone<T1, T2, TResult, U1, U2, UResult>(this Func<T1, T2, TResult> F, Func<U1, T1> ArgConv1, Func<U2, T2> ArgConv2, Func<TResult, UResult> ResConv) {
    return (arg1, arg2) => ResConv(F(ArgConv1(arg1), ArgConv2(arg2)));
  }
}
// end of class FuncExtensions
namespace Bison.Core.models {



} // end of namespace Bison.Core.models
namespace Bison.Core.dafny.Taxonomy {

  public partial class __default {
    public static Dafny.ISequence<Bison.Core.models.Observation> FilterByTaxon(Bison.Core.models.Taxon root, Dafny.ISequence<Bison.Core.models.Observation> obs)
    {
      Dafny.ISequence<Bison.Core.models.Observation> _0___accumulator = Dafny.Sequence<Bison.Core.models.Observation>.FromElements();
    TAIL_CALL_START: ;
      if ((new BigInteger((obs).Count)).Sign == 0) {
        return Dafny.Sequence<Bison.Core.models.Observation>.Concat(_0___accumulator, Dafny.Sequence<Bison.Core.models.Observation>.FromElements());
      } else if ((((obs).Select(BigInteger.Zero)).GetTaxon()).IsSubTaxonOf(root)) {
        _0___accumulator = Dafny.Sequence<Bison.Core.models.Observation>.Concat(_0___accumulator, Dafny.Sequence<Bison.Core.models.Observation>.FromElements((obs).Select(BigInteger.Zero)));
        Bison.Core.models.Taxon _in0 = root;
        Dafny.ISequence<Bison.Core.models.Observation> _in1 = (obs).Drop(BigInteger.One);
        root = _in0;
        obs = _in1;
        goto TAIL_CALL_START;
      } else {
        Bison.Core.models.Taxon _in2 = root;
        Dafny.ISequence<Bison.Core.models.Observation> _in3 = (obs).Drop(BigInteger.One);
        root = _in2;
        obs = _in3;
        goto TAIL_CALL_START;
      }
    }
  }
} // end of namespace Bison.Core.dafny.Taxonomy
namespace _module {

} // end of namespace _module
