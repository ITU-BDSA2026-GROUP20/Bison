class {:extern} Taxon {
  function {:extern} IsSubtaxonOf(other: Taxon): bool
  reads {}
}

class {:extern} Observation {
  function {:extern} GetTaxon(): Taxon
  reads {}
  }

function FilterByTaxon(root: Taxon, obs: seq<Observation>): (result: seq<Observation>)
    decreases |obs|
    ensures forall o :: o in result ==> o in obs && o.GetTaxon().IsSubtaxonOf(root)
    ensures forall o :: o in obs && o.GetTaxon().IsSubtaxonOf(root) ==> o in result
    {
       if |obs| == 0 then []
       else if obs[0].GetTaxon().IsSubtaxonOf(root) then
        [obs[0]] + FilterByTaxon(root, obs[1..])
       else 
        FilterByTaxon(root, obs[1..])
    }
   
