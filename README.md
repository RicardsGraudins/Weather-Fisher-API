# Weather Fisher API
This repository contains **additional** code and information for my fourth-year (hons) undergraduate project for the module **Mobile Application Development 3.**
The module is taught to undergraduate students at [GMIT](http://www.gmit.ie/) in the Department of Computer Science and Applied Physics for the course [B.S.c. (Hons) in Software Developement.](https://www.gmit.ie/software-development/bachelor-science-honours-software-development)

## Overview: 
As part of the Mobile Application Development 3 [project](https://github.com/RicardsGraudins/UWP-Project-3), I created an API that handles the data storage and retrieval aspect of the project. Essentially this API interacts with the [World Tides API](https://www.worldtides.info/home) where it retrieves tidal data for the 26 counties of Ireland and then using the Microsoft.Azure.Documents [library](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.documents?view=azure-dotnet), it interacts with my [CosmosDB](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction) database that is hosted on [Microsoft Azure](https://azure.microsoft.com/en-us/) where we save a document for each county. 

The API itself is also hosted on Microsoft Azure and returns a JSON whenever a request is made for a specific county. To access the API use the following URL http://worldtidesforecast20171125090607.azurewebsites.net/?county=Dublin simply replace "Dublin" with another county to get the JSON for that county. The API updates the data stored every 5 hours and since we only need the most recent forecast for each county, we delete all the documents first and then create a document for each county with the updated data which keeps the memory required to store the data to a minimum.

**Question:** Why not just use the World Tides API to get data for counties?  
**Answer:** Cost.

In order to minimize the cost of using the World Tides API I have went through the trouble of creating my own API that basically preforms the same functionality as the World Tides API. So for example, lets say we have x amount of users using the [Weather Fisher](https://github.com/RicardsGraudins/UWP-Project-3) application and each user might look at the application several times a day, everytime the user looks at the tides forecast for a county the application makes a call to the World Tides API which costs us 1 credit. Now lets say theres 1000 users and each user makes a call to the World Tides API once a day which equates to 1000 credits, instead we can use this API which only costs 26 credits (1 per county) every 5 hours that equates to 130 ish credits per day which is a significant difference. If the the amount of users using the application increased from 1000 to say 10,000 and each user makes a call to the World Tides API once a day that would be 10,000 credits however if we use this API it would still be around 130 credits per day.

## What is CosmosDB:
Initially I had intended to use DocumentDB however I decided to go with CosmosDB which builds upon and is an extension of DocumentDB. I chose CosmosDB because it is a new service offered by Azure that offers new capabilities and since it is a NoSQL database, its a lot faster to set up and modify than an SQL database.

Azure Cosmos DB was built from the ground up with global distribution and horizontal scale at its core. It offers turnkey global distribution across any number of Azure regions by transparently scaling and replicating your data wherever your users are. Elastically scale throughput and storage worldwide, and pay only for what you need. Azure Cosmos DB provides native support for NoSQL choices, offers multiple well-defined consistency models, guarantees single-digit-millisecond latencies at the 99th percentile, and guarantees high availability with multi-homing capabilities and low latencies anywhere in the world— all backed by industry-leading, comprehensive service level agreements (SLAs).

## References:
* [Azure CosmosDB](https://azure.microsoft.com/en-us/services/cosmos-db/)
* [World Tides API](https://www.worldtides.info/apidocs)
