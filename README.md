# Assumptions
1. Web Api receives the delivery payload from client and once it receives, api will either create new record or update to different states.
3. User or partner can get the delivery details through api by passing different delivery states.
4. User or partner can delete the delivery record by passing order number to web api.
5. Each order can have multiple delivery records.

# Technical Specification
1. Structured the solution by following clean architecture with Domain driven design, CQRS and Mediator patter.
2. For storing the data used MongoDb database.Considered dynamic data structure so used NoSql database.
3. Added logging and exception handling classes.
4. Added unit test classes for handlers.

# Important note to run Api
Please change the MongoDb database connection string from "appsettings.json" before running the Api.
