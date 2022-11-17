using System;

namespace CosmosChangeFeedExample
{
    public class DbItem
    {
        public string id { get; set; }
        public string appId { get; set; }
        public bool useGoodPartitionKey { get; set; }
        public string partitionKey { get { return useGoodPartitionKey ? Guid.NewGuid().ToString() : "12345"; } }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
        public Address address { get; set; }
        public OwnedItem[] ownedItems { get; set; }
        public DateTime creationTime { get; set; }
    }

    public class Address
    {
        public string streetAddress { get; set; }
        public string suburb { get; set; }
        public string state { get; set; }
        public int postcode { get; set; }
    }

    public class OwnedItem
    {
        public string itemDescription { get; set; }
        public decimal cost { get; set; }
        public string category { get; set; }
    }

    public static class SampleDocumentCreator
    {
        private readonly static string[] FirstNames = new string[]
        {
            "Mary", "Bob", "Jane", "Jess", "Paul", "Pennywise", "Donald", "Freddy", "Angus", "Aristotle", "PeeWee", "Michele"
        };
        private readonly static string[] LastNames = new string[]
        {
            "Magnolia","Jones","Smith","ChumbaWumba","Duck","Krueger","Dumbledore","Cucumber","Unicorn","Herman","RandomPerson","Beetlejuice"
        };
        public readonly static string[] States = new string[]
        {
            "NSW","TAS","VIC","SA","WA","QLD","ACT"
        };
        public readonly static string[] Streets = new string[]
        {
            "Brown st", "Tarmac rd", "Two way st","Pothole rd","MyTaxesPaidForThis st","Some stupid name cr","Hells highway","Ridiculous rd","Whatever way"
        };
        public readonly static string[] Suburbs = new string[]
        {
            "Sydney", "Suburbia", "FreeVille","Woop Woop","Back of Bourke","Outback","The Bush","The Footpath","Somewhere"
        };
        public readonly static string[] ItemCategories = new string[]
        {
            "Bling", "OldClutter", "CoolStuff"
        };
        public readonly static string[] ItemDescription = new string[]
        {
            "Diamond Ring", "Computer", "Model car", "Phone", "Rubber ducky", "Furry thing", "Clown nose"
        };
    }
}