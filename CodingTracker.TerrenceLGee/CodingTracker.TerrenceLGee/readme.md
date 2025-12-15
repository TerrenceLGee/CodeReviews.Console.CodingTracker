# Coding Tracker App

Console application written using C# 14/.NET 10 using JetBrains Rider on Ubuntu 25.10.

The purpose of this application is to allow you to log the time that you spend coding for each session that you code.

Created following the curriculum of [C# Academy](https://www.thecsharpacademy.com/)

# Features
- Creates a Sqlite database on application startup if the database does not already exist.
- Creates relevant tables for in the database if they do not already exist: Coder, Coding Goal, Coding Session etc.
- Multi-user so that more than one user may track their coding sessions.
- Allows a user to create a coding goal to work towards and tracks each session of that goal.
- Allows a user to add an optional comment with regard to each session.
- Allows a user to generate a report which combines the information from all the current/previous coding goals and sessions
- Implements logging to a file in the project's directory in case of errors, while also handling exceptions.
- Implements unit testing of the service layer methods.

# Challenges Faced When Implementing This Project
- Sqlite. Putting together queries to join multiple tables to return complete data.
- Dapper. As the previous project used ADO.NET, there was a learning curve learning how Dapper implemented certain functionality.
- Reporting functionality. Took several attempts to get to the current implementation.
- Separation of Concerns. Attempting to keep the functionality of various classes separate from each other.
- DRY (Don't Repeat Yourself). Attempting to reuse functionality across various classes.
- Pagination. Attempting to implement pagination so that as the number of goals/sessions/reports grows the user will not be overwhelmed with everything being displayed at once.

# What Was Learned Implementing This Project
- Like with the last project, the importance of planning out what you want to implement before writing a single line of code.
- Also, again like with the last project the importance of reading documentation as well as other resources such as Stack Overflow, Reddit, various blogs etc.
- The importance of Dictionaries to map for example the count of an object in order to prevent duplicate entries, when retrieving objects from the database.
- The value of unit testing. I learned a lot about the value of mocking and why it is important to use Interfaces/Dependency inject, as well as how unit testing can help you create more optimized code.
- The most important thing I have learned is the importance of the debugger, when the application was not working as expected debugging the code sometimes line by line allowed me to catch so many errors in logic that I had made.

# Areas To Improve Upon
- The ability to write more concise and reusable methods and classes as with the last project.
- Logic, problem-solving, writing cleaner code, making the code I write more efficient and less bloated, all things to improve upon.
- Everything. Basically I need to improve on everything, while I am generally happy with the finished project, I know that I can do much, much better I just have to keep working at it.


# Technologies Used
- [Spectre.Console](https://spectreconsole.net/)
- [Dapper](https://github.com/DapperLib/Dapper)
- [Serilog](https://serilog.net/)
- [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration)
- [XUnit](https://xunit.net/?tabs=cs)
- [Moq](https://github.com/devlooped/moq)


# Some Resources Used
- [C# Academy](https://www.thecsharpacademy.com/)
- [SQLite Documentation](https://sqlite.org/docs.html)
- [Dapper Documentation](https://www.learndapper.com/)
- [Microsoft's Excellent Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Stack Overflow](https://stackoverflow.com/questions)
- [C# Forums](https://csharpforums.net/forums/c-general-discussion.79/)
- [Excellent article on XUnit and Moq](https://dev.to/zrebhi/the-ultimate-guide-to-unit-testing-in-net-c-using-xunit-and-moq-without-the-boring-examples-28ad)
