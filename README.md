# MMT-Tech-Test

# Notes

Web API results can be seen in /MMT.WebApi/results.json

Can be tested with this command:

`curl --location --request POST 'https://localhost:44388/Orders/FetchCustomerOrderLatestByEmailAddress?emailAddress=cat.owner@mmtdigital.co.uk'`

There are some unit tests around the code that accesses the Customer Account Details API.

I would also have:
 - implemented the "Gift" requirement but ran out of time.
 - added unit tests around the DbClient.
 - added unit tests around the OrdersController methods.
 - used stored procedures instead of hard-coded SQL.
 - formatted the JSON dates as per the spec
 - moved the GetInfo aggregation logic out of the OrdersController into a separate injectable class


Regards,

Anthony Chambers

antmx1@gmail.com
