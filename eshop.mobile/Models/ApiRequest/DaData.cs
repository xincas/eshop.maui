namespace Eshop.Mobile.Models.ApiRequest;

public class DaDataRequest
{
    public string query { get; set; }
    public int count { get; set; }
}

public class DaDataResponse
{
    public IEnumerable<Suggestion> suggestions { get; set; }

    public class Suggestion
    {
        public string value { get; set; }
    }
}