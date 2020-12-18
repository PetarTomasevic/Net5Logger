
# Net 5 Logging ILogger Extension
Small Library to enable ILogger to Log into database or text file

To install nuget package run

        Install-Package Net5Logger -Version 1.0.0

In **Program.cs**
 add

    using Net5Logger.DatabaseLogging;
    using Net5Logger.FileLogging;

Extend host builder with

    Host.CreateDefaultBuilder(args)
            .ConfigureLogging(x =>
            x.AddEventLog()
            .AddConsole()
            .AddDatabaseLogging("{YOUR-DB-CONNECTION-STRING}")
            .AddFileLogging(Directory.GetCurrentDirectory() + "/App_Data/")
            .AddFilter<DatabaseLoggerProvider>(logLevel => logLevel == LogLevel.Error)

If you want to log just specific type of errors you can set filtering for provider.
Example for database provider to log only errors:

    .AddFilter<DatabaseLoggerProvider>(logLevel => logLevel == LogLevel.Error)

Then use ILogger as usual and that's all. 

If you want to log to database , create table in your database:

    CREATE TABLE [dbo].[Logs](
    	[Id] [int] IDENTITY(1,1) NOT NULL,
    	[LogLevel] [nvarchar](max) NULL,
    	[EventId] [nvarchar](max) NULL,
    	[State] [nvarchar](max) NULL,
    	[Exception] [nvarchar](max) NULL,
    	[Message] [nvarchar](max) NULL,
    	[ProccesTimeMiliSeconds] [nvarchar](max) NULL,
    	[Host] [nvarchar](max) NULL
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO

Example for blazor (on index.razor:

Inject logger:

    @using Microsoft.Extensions.Logging
    @inject ILogger<Index> Logger

Set Button to call function:

    <button @onclick="@TestLogger" class="btn btn-primary">Test Logger</button>
    
Set in code:

        @code{
     void TestLogger()
        {
            try
            {
                Logger.LogError("testing Warning");
                Logger.LogInformation("testing Information");
                Logger.LogWarning("testing warning");
                Logger.LogCritical("testing critical");
                throw new System.ArgumentException("Testing Catch for library", "testing catch exception");
    
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "testing catch exception");
            }
    
        }
    }

Happy Coding!


