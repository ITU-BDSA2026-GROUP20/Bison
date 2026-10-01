using System;
using System.ComponentModel.DataAnnotations.Schema;
using Bison.Core.utils;

namespace Bison.Core.models;

public class Reading : IPrintable
{
    public Reading() { }

    public Reading(int UserId, string Observation, string Timestamp)
    {
        this.UserId = UserId;
        this.Observation = Observation;
        this.Timestamp = Timestamp;
    }

    public int UserId { get; set; }
    public User? User{ get; set; }
    public string Observation { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
    public Guid Id { get; set; } = Guid.NewGuid();

    public override string ToString()                                                                                                                                                                      
    {                                                                                                                                                                                                      
        DateTime fTimeStamp = TimestampConverter.FromUnixSeconds(Timestamp);                                                                                                                               
        return (User?.Username ?? $"user#{UserId}") + " @ " + fTimeStamp + " " + Observation + " " + Id.ToString();                                                                                    
    } 
}