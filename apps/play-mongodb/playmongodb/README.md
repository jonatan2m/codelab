# This project is a MongoDB example.

# Requirements
- DotNet Core 3.1 or higher
- MongoDB docker instance (or cloud instance, if you prefer)
- VSCode or any code editor. It's up to you.

# Running
Creating a console app.

`dotnet new console --output playmongodb`

`cd playmongodb`

Adding MongoDB Driver

`dotnet add playmongodb.csproj package MongoDB.Driver`

Create a MongoDB container instance (by docker)

`docker run -itd --rm -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=jonatan2m -e MONGO_INITDB_ROOT_PASSWORD=123456 --name play-mongo mongo`

The `-e` parameter allows you to define environment variables that will be used into the docker instance.
`MONGO_INITDB_ROOT_USERNAME` and `MONGO_INITDB_ROOT_PASSWORD` set the default root credencials.

The Connection String looks like it: `mongodb://jonatan2m:123456@localhost:27017/admin`

Running the project

`dotnet run`



# Scenario 01 - mongoDB docker instance
Just estabilish a connection with MongoDB instance and create a TODO app running on console.
TODO app consists on having Tasks and their statuses. It's possible having tasks with deadline or not.
For simplification, a task creation is driven by console, with fields separated by ";".
Examples:
## creation
> create a mongoDB tutorial;2021-02-05

## edition
> 2;editing a mongoDB tutorial task (changing title)

> 2;2021-02-01 (changing deadline)

## completion
> 2 (task id)

For edition, pass the Task Id at first position
The console will handle all options before perform the action

## Reports
- How many tasks still opened? (list all)
- How many tasks are overdue? (list all)

# Scenario 02 - Atlas MongoDB
The better starting point. [Here](https://www.mongodb.com/blog/post/atlas-plus-load-sample-data-for-easier-learning) you can access the database samples.
In the list below you can know more about the samples:
- sample_airbnb
- sample_geospatial
- sample_mflix (is a database with five collections all about movies)
- sample_supplies (is a typical sales data collection)
- *sample_training* (is a database with nine collections used in MongoDB’s private training.)
- sample_weatherdata


## Questions
- Why shall I use ObjectId to map Id properties?

- How can I see the collection metadata?

- What is the IAsyncCursor? How can I use it?
Widely used to list items

- How to know if there is any indexes on database?

- Type BsonDocument and BsonElement

> Read operations are case-sensitive