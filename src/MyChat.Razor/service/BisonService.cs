using MyChat.Razor;

public record ObservationViewModel(string Author, string Message, string Timestamp);

public class ObservationService : IObservationService
{

    //Note to self: IMPLEMENT THIS
    public List<ObservationViewModel> GetObservations()
    {
        //Need to return
        return null;
    }


    //Note to self: IMPLEMENT THIS
    public List<ObservationViewModel> GetObservationsFromAuthor(string author)
    {
        // filter by the provided author name
       // return observations.Where(x => x.Author == author).ToList();
       return null;
    }

    public List<ObservationViewModel> GetObservationsFromAuthorAndPage(string author, int page)
    {
       return null;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
