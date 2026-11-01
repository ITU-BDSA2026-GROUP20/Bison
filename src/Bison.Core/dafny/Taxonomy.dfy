module {:extern "Bison.Core.models"} Models {
  class {:extern} Taxon {
    function {:extern} {:axiom} IsSubTaxonOf(other: Taxon): bool
    reads {}
  }

  class {:extern} Observation {
    function {:extern} {:axiom} GetTaxon(): Taxon
    reads {}
  }
}

module {:extern "Bison.Core.dafny.Taxonomy"} Taxonomy {
  import opened Models

  function FilterByTaxon(root: Taxon, obs: seq<Observation>): (result: seq<Observation>)
    decreases |obs|
    ensures forall o :: o in result ==> o in obs && o.GetTaxon().IsSubTaxonOf(root)
    ensures forall o :: o in obs && o.GetTaxon().IsSubTaxonOf(root) ==> o in result
    {
      if |obs| == 0 then []
      else if obs[0].GetTaxon().IsSubTaxonOf(root) then
      [obs[0]] + FilterByTaxon(root, obs[1..])
      else 
      FilterByTaxon(root, obs[1..])
    }
}