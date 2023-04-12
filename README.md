# SandTMan Function
The **SandTMan** function is an Azure Function that is triggered by an HTTP request with the authorization level of **Function**. It supports both **get** and **post** requests.

## Parameters
This function takes in two parameters:

- **req**: An object of type **HttpRequest**. This object represents the HTTP request that triggers this function.
- **log**: An object of type **ILogger**. This object represents the logger that is used to log the execution of the function.

## Constants
The following constants are defined in the function:

- **now**: A **DateTime** object representing the current date and time.
- **N**: An integer constant representing the number of previous days to include in the usage reports functionality. The default value is **30**.
- **period**: A string constant representing the time period for active users functionality. The default value is **D180**.

## Function call
The **Run** function calls the **ReportingFunction** function from the **SandTManFunction** class, passing in the **log**, **now**, **N**, and **period** parameters. The ReportingFunction function returns a string value, which is logged by the **log** object.

The function returns an **OkObjectResult** object with a string message "Done".

# SandTManFunction
This is a class containing a single static method, **ReportingFunction**, that is responsible for collecting data from different sources and pushing them to an output database.

## Methods
### ReportingFunction
`public static async Task<string> ReportingFunction(ILogger log, DateTime now, int N, string period)`

This method collects data from different sources and pushes them to an output database. It takes in the following parameters:

- **log** (**ILogger**): An instance of a logger for logging events that occur within the method.
- **now** (**DateTime**): The current date and time.
- **N** (**int**): The number of days for which to collect data for teams usage reports functionality.
- **period** (**string**): The period for which to collect data for active users functionality. It can take one of the following values: **D7**, **D30**, **D180**. By default, **D180** is used.

The method performs the following operations:

1. Teams usage reports functionality: It collects the data for the specified number of days (N) and updates the teams usage reports. It opens a connection to the output database and loops through the specified number of days, calling the **UpdateTeamsUsageReportsAsync** method of the **Teams** class for each day. The method logs the result of each call.
2. Active users functionality: It collects the data for the specified period and updates the active users data. It opens a connection to the output database, calls the **UpdateActiveUsersAsync** method of the **ActiveUsers** class, and logs the result of the call.
3. HR info functionality: It collects the HR data from the teams usage and active users databases for the specified period and updates the HR information. It opens a connection to the output database, calls the **UpdateHRInfoAsync** method of the **HR** class, and logs the result of the call.
If an error occurs during any of the above operations, the method throws an exception and logs the error message.

The method returns a string indicating that all functionality were executed successfully.

# DbHandler Class
The **DbHandler** class is used for opening connections to the output database using a connection string. The **CreateDbConnection** method opens a connection to the output database and returns the opened connection.

## CreateDbConnection Method
The **CreateDbConnection** method is used for opening a connection to the output database. The method takes a **log** parameter for logging and returns an opened **NpgsqlConnection** object.

### Parameters
**log**: The **ILogger** object for logging.
### Return Value
The method returns an opened **NpgsqlConnection** object.

### Exceptions
The method throws an **Exception** if an error occurs while opening the database connection or if the database connection is not opened after three tries.

## PushOneElementToDbTeamsDataAsync 
The method is responsible for inserting one record of **Teams** object into the **teams_data** table in the PostgreSQL database.

### Parameters
The method takes two parameters
- first is the **Teams** object containing the data to be inserted
- the second is an instance of **NpgsqlConnection** class representing the connection to the database.

### Core logic
Within the method, a new **NpgsqlCommand** object is created with a SQL query for inserting data into the **teams_data** table. The command's **Connection** property is set to the NpgsqlConnection passed to the method.

Next, the method sets the parameter values for each field of the **teams_data** table by calling **AddWithValue** method for each parameter.

Finally, the **ExecuteNonQuery** method is called on the **NpgsqlCommand** object to execute the insert command on the database, and the new record is inserted into the **teams_data** table.

This method is used in **UpdateTeamsUsageReportsAsync** method to insert **Teams** object into the PostgreSQL database.

## PushOneElementToDbActiveUsersAsync 
The method pushes one element of type **ActiveUsers** to the output database.

### Parameters
The method takes three parameters:

- **ActiveUsers activeUsersDataRecord**: an instance of the **ActiveUsers** class that contains data to be pushed to the output database.
- **NpgsqlConnection o365Conn**: a connection to the output database.
- **ILogger log**: a logger instance used to log information or errors.

### Core logic
Inside the method, a new instance of **NpgsqlCommand** is created and its connection is set to the **o365Conn** parameter. The SQL query for pushing data to the output database is set to **SqlQueryStringActiveUsersData** using the **CommandText** property of the **cmd** instance.

Then, the method uses **cmd.Parameters.AddWithValue** to add values to the SQL query. For each value, a parameter name starting with **@value** is used followed by the value of the corresponding property of the **ActiveUsers** instance.

Finally, **cmd.ExecuteNonQuery** is called to execute the SQL query and push the data to the output database.

This method is used in the **UpdateActiveUsersDataAsync** method of the **ApiHandler** class to push active users data to the output database.

## HasRecord
This method checks whether an HR record already exists in the database. 
### Parameters
It takes in an HR object **hrDataRecord** and a **NpgsqlConnection** object **HRConn** as parameters.

### Core logic
The method first creates a SQL query string using the **SqlQueryCommandStringHrRecordExist** constant, which is a pre-defined SQL query that checks whether a record with the given SSO (single sign-on) already exists in the database.

It then creates a new NpgsqlCommand object with the SQL query string and the **HRConn** connection object, and adds the **sso** parameter with the value of **hrDataRecord.sso** to the command.

The **ExecuteScalarAsync()** method of the command is then called to execute the SQL query and return the first column of the first row of the result set. In this case, since the SQL query returns a single boolean value, **ExecuteScalarAsync()** returns a single object that is cast to a boolean value.

Finally, the method returns the boolean value indicating whether a record with the given SSO already exists in the database.

## DeleteHrRecord
### Parameters
This method takes an **HR** object and a **NpgsqlConnection** object as input parameters. 
### Core logic
It deletes a record from the HR database that matches the **SSO** (Single Sign-On) of the **HR** object.

The method first creates a string variable named **deleteQuery** that contains a SQL query to delete a record from the HR database with a matching SSO value. The method then creates a **NpgsqlCommand** object, sets its command text to the **deleteQuery** string, and adds a parameter named **"sso"** with the value of **hrDataRecord.sso**. The method then calls the **ExecuteNonQueryAsync** method of the command object to execute the delete query and delete the matching record from the HR database.

## PushOneElementToDbHrDataAsync
### Parameters
The method takes an HR record and a database connection as input and pushes the record to the HR database.

### Core logic
The method first creates an instance of **NpgsqlCommand** and sets the connection to the HR database. It then sets the **CommandText** property to **SqlQueryStringHrData**, which is the SQL query string for inserting a single HR record into the database.

The method then adds the values of each property of the **hrDataRecord** object as parameters to the **NpgsqlCommand** instance using the **Parameters.AddWithValue()** method. The method then executes the SQL command by calling **ExecuteNonQuery()** on the **NpgsqlCommand** instance.

Finally, the method returns void as there is no need to return any value from this method.













# UpdateTeamsUsageReportsAsync 
The method updates the teams usage reports by fetching data from the Teams API and storing it in the output database.

## Parameters
The method takes four parameters:

- **i** - an integer representing the number of days to subtract from the current day to get the day for which the Teams usage report should be generated.
- **now** - the current date and time.
- **log** - an **ILogger** object that logs events and exceptions that occur during the method's execution.
- **conn** - an **NpgsqlConnection** object that represents the connection to the output database.
The method first calculates the date for which the Teams usage report should be generated by subtracting **i** days from the current date. It then creates an SQL query to search for a record with a matching date in the output database.

If a matching record is found, the method returns without performing any further actions for that day. If no matching record is found, the method proceeds to retrieve an access token from the Teams API and use it to make an HTTP GET request to the **getTeamsUserActivityUserDetail** endpoint to fetch the usage data for the calculated day.

The response content is then parsed using the **CsvParser** library to extract the usage data for each team. The parser removes the first line of the response, which contains the headers, and checks if the content is empty. If the content is not empty, the parser assigns the properties of the **CsvParserClassTeamsData** objects to the corresponding properties of **TeamsData** objects using LINQ.

Finally, the method pushes the **TeamsData** objects to the output database by calling the **PushOneElementToDbTeamsDataAsync** method for each object. If any exceptions occur during this process, they are logged by the **log** parameter.

The method returns a string indicating the status of the day report process.



