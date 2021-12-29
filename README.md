# Reddit.Detective.Net

reddit.detective.net is help you to represent reddit data using graph structures using Neo4j.
This project is inspired on [reddit-detective](https://github.com/umitkaanusta/reddit-detective) this new approach use [.NET 5.0 Framework](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
and allows researchers and developers who are curious about how Redditors behave.


### Helps you to:
- Detect political disinformation campaigns
- Find trolls manipulating the discussion
- Find secret influencers and idea spreaders (it might be you!)



### Prerequisits
Using Neo4j management interface you should add an database version compatible (3.5.30) with [Neo4j Bolt driver for .NET](https://github.com/neo4j/neo4j-dotnet-driver) and fill with your ```neo4jUserName``` and ```neo4jPassword```.



After that on Credentials.cs fill you must fill the other fields about your reddit account to be used by [Reddit.NET](https://github.com/sirkris/Reddit.NET). 
```csharp
    public static class Credentials
    {
        public static string uri = "bolt://localhost:7687/db/reddit";
        public static string appId = "<APPLICATION-ID>";
        public static string appSecret = "<APPLICATION-SECRET>";
        public static string refreshToken = "<REFRESH-TOKEN>";
        public static string accessToken = "<ACCESS-TOKEN>";
        public static string userAgent = "<USER-AGENT>";

        public static string neo4jUserName = "neo4j";
        public static string neo4jPassword = "test";
    }
```

### Installation
Installation steps, without the need the build and compile the code
- Install [.NET 5.0 Framework](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
- Install [Neo4j 4.1.0](https://neo4j.com/docs/operations-manual/current/installation/);
- Neo4j uses [Cypher language](/https://neo4j.com/graphacademy/training-intro-40/enrollment/) as its query language.




### Build and Run using source code
Build Reddit.Detective.Net and all of its dependencies:
```..\reddit.detective.net\reddit.detective.net>dotnet build```

Then build Reddit.Detective.Net.Tests unittest project:
```..\reddit.detective.net\reddit.detective.net.tests>dotnet build```




### Usage
Run each unit-test project on Reddit.Detective.Net.Tests and check graph structure on Neo4j.

After run your test first time you will need to uncomment the follow code-line on ```RedditDataMapping.cs``` in order to drop indexs on database and create new ones next time you run your test.

```csharp
await RedditorController.DropIndexes(client.Driver);
```


### Example


```csharp
        [Fact]
        public void SearchRedditorsCommonSubreddits()
        {
            var service = new RedditDataService();
            var settings = ConnectionSettings.CreateBasicAuth(Credentials.uri, Credentials.neo4jUserName, Credentials.neo4jPassword);
            var api = new RedditClient(Credentials.appId, Credentials.refreshToken, Credentials.appSecret, Credentials.accessToken, Credentials.userAgent);
            //Add redditors you want to find common subreddits
            string [] redditorName = { "pmol87", "desculpe_mas" };
            //Add related subreddits name
            string subredditSearchName = "portugal";

            RedditDataMapping.GetRedditorCommonSubreddits(settings, api, service, redditorName, subredditSearchName).GetAwaiter().GetResult();
        }
```

![SearchRedditorsCommonSubreddits](https://github.com/oleitao/Reddit.Detective.Net/blob/master/reddit.detective.net.tests/Screenshots/RedditorsCommonSubredditsTest.PNG)
