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



# Scenario
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