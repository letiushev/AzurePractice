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

Methods
ReportingFunction
public static async Task<string> ReportingFunction(ILogger log, DateTime now, int N, string period)

This method collects data from different sources and pushes them to an output database. It takes in the following parameters:

log (ILogger): An instance of a logger for logging events that occur within the method.
now (DateTime): The current date and time.
N (int): The number of days for which to collect data for teams usage reports functionality.
period (string): The period for which to collect data for active users functionality. It can take one of the following values: D7, D30, D180. By default, D180 is used.
The method performs the following operations:

Teams usage reports functionality: It collects the data for the specified number of days (N) and updates the teams usage reports. It opens a connection to the output database and loops through the specified number of days, calling the UpdateTeamsUsageReportsAsync method of the Teams class for each day. The method logs the result of each call.
Active users functionality: It collects the data for the specified period and updates the active users data. It opens a connection to the output database, calls the UpdateActiveUsersAsync method of the ActiveUsers class, and logs the result of the call.
HR info functionality: It collects the HR data from the teams usage and active users databases for the specified period and updates the HR information. It opens a connection to the output database, calls the UpdateHRInfoAsync method of the HR class, and logs the result of the call.
If an error occurs during any of the above operations, the method throws an exception and logs the error message.

The method returns a string indicating that all functionality were executed successfully.



