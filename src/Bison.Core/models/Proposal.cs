using System;
using Bison.Core.utils;

namespace Bison.Core.models;

public class Proposal : IPrintable
{
    public Proposal(string TaxonID, Guid Id, string Timestamp)
    {
        this.TaxonID = TaxonID;
        this.Id = Id;
        this.Timestamp = Timestamp;
    }

    public string TaxonID { get; set; } = string.Empty;
    public Guid Id { get; set; } = new Guid(); // Empty by default, needs to come from Constructor
    public string Timestamp { get; set; } = string.Empty;
    
    public override string ToString()
    {
        DateTime fTimeStamp = TimestampConverter.FromUnixSeconds(Timestamp);
        return TaxonID + " @ " + fTimeStamp + " " + Id.ToString();
    }
}