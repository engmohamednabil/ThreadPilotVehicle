# ThreadPilot 

## Explains your architecture and design decisions

Each solution follows a multi-layer architecture and can function as two microservices with certain considerations.
The first solution, called Person, has one endpoint that takes person identification as input and returns their insurance details with associated costs.

The second solution, called Vehicle, is called by the Person service using a vehicle registration number as input to return the vehicle details.

The layers are as follows:
Core Layer: Contains four class library projects:
Entities: Represent the objects for Person and Vehicle.

Interfaces: Define all contracts to enable dependency injection.

Models: Include response models mapped to Entities, hiding sensitive data.

Services: Contain the business logic, called directly from the controller.

Infrastructure Layer:
Integration: Manages integrations with external services, such as calling the Vehicle service from the Person service.

Presentation Layer: The .NET Core API that clients use, with Swagger enabled for both services.

Test Layer: A unit testing project using NUnit.

The Path to Microservices:
Single Responsibility: Each service is limited to a single responsibility, such as handling Person or Vehicle data, and performs it effectively.

Loose Coupling: A message broker can facilitate asynchronous communication between services, reducing dependency on direct responses.

Independent Deployment: Each service can have its own database, enabling independent deployment from other external services.

Containerization: Services can be containerized for scalability, depending on business needs, though this introduces additional complexities.


## Describes how to run and test the solution locally

- Running and testing the solution will depend on the developer preferances, if Visual Studio or VSCode are installed or if the developer prefers working with command line.
- As explained in the previous section, there are two solutions, Person.sln and Vehicle.sln
- For simplicity, I will explain a set of few commands that are needed to run the solution and check if its unit tests are successfully executes
- Assuming that .NET SDK is installed in your machine (to verify run dotnet --version)
- Start by running cmd from windows or the relevant command line program in your machine, navigate to \ThreadPilotVehicle\Vehicle (it is where the Vehicle.sln resides)
- Execute the following commands in order
	* dotnet restore
	* dotnet build
	* dotnet test
	* dotnet run
- The result from the last command (dotnet run) will return a Url for Vehicle endpoint (copy that Url and check the next step carefully)
- Note: there is only one key inside Person service appsettings.json file which is called "VehicleApiURL" "http://localhost:5159/Vehicle" you need to make sure this Url matches the one you gets from running Vehicle endpoint
- Now move the the second solution, navigate to \ThreadPilotPerson\Person (it is where the Person.sln resides)
- Execute the following commands in order
	* dotnet restore
	* dotnet build
	* dotnet test
	* dotnet run
- Note: the command (dotnet test) runs all unit tests are shows the states of each one


Running the two solutions from Visual Studio or VSCode is even simpler:
------------------------------------------------------------------------
- Start by navigating to \ThreadPilotVehicle\Vehicle (where the Vehicle.sln resides)
- Set the Vehicle project inside Presentation folder as (Startup project)
- Run the solution using ctrl + F5 or from VSCode
- Take the Url and paste it in your prefered browser, is should look like http://localhost:xxxx/swagger/index.html 
where xxxx is the port that Visual Studio or VSCode assigns
- Copy that Url (without the section /swagger/index.html ) and check the next step carefully
- Navigate to \ThreadPilotPerson\Person (where the Person.sln resides)
- Set the Person project inside Presentation folder as (Startup project)
- Make sure that the appsettings.json file inside the path \ThreadPilotPerson\Person has the correct Url value for the ley called VehicleApiURL, that Url is what came from running Vehicle service
- Run the solution using ctrl + F5 or from VSCode 
- Another Url for running Person service will come and it should look like http://localhost:yyyy/swagger/index.html
- Use swagger to test the integration between both services

## Discusses your approach to error handling, extensibility, and (if applicable) security

Error Handling:
---------------
The following are best practices for graceful error handling in a solution: centralizing error handling, using standard HTTP status codes, logging, and differentiating between exception types.
Centralization of Error Handling: A middleware, ExceptionHandlingMiddleware, is added to the solution and injected into the execution pipeline using app.UseMiddleware<ExceptionHandlingMiddleware>();.

Using Standard HTTP Status Codes: For each exception, the StatusCodes enum is used to set the appropriate Response.StatusCode.

Logging: When an exception is caught, it is logged before being handled, making it easier for developers to track issues by reviewing the logs.

Differentiating Exception Types: In the middleware, a switch-case statement handles each exception type appropriately, setting the correct message and StatusCode.

Extensibility:
--------------
The solution is extensible by using interfaces instead of concrete objects. Dependencies are injected via interfaces, simplifying the process of faking or mocking services or external endpoint calls in the unit testing project.

[SetUp]
public void Setup()
{
    _vehicleService = new FakeVehicleService(); // Uses a fake service instead of the actual VehicleService
    _personService = new PersonService(_vehicleService);
    // ...
}

Security:
---------
Security in the solution can be handled in multiple ways:
Sensitive information, such as a person's identification number, is hidden in responses by excluding these fields.
Another Idea that I may consider is:
An API key which can be added to both services. For example, adding an API key to the Person service allows the Vehicle service to validate it before responding, ensuring secure service-to-service communication.


## Use of patterns that make the solution extensible and maintainable

The following patterns and principles were implemented to meet the extensibility requirement:
Dependency Injection: All dependency injection is performed through constructor injection using interfaces.

Use of Interfaces: Each service has a corresponding contract that defines the methods to be implemented to comply with the contract.

Single Responsibility Principle: Each method or class in the solution focuses on a single responsibility.

Mocking/Abstraction: In the unit test project, faking a service instead of calling an external service enhances the solution's extensibility and flexibility.


## Mocking/abstraction for legacy systems or external dependencies

The Moq library was used to mock the ILogger and inject a fake service, which is called instead of the actual external Vehicle service.

This ensures that unit tests can run independently of external services.

An integration test project could be added to perform actual tests between the Person service and the Vehicle service.


## Handling of edge cases (e.g., missing vehicles, no insurances, multiple insurances)

I have added unit tests to handle some of the edge cases like no insurances, multiple insurances, person not found and vehicle not found


## Discussion of API versioning

API versioning enables developers to introduce new features without breaking existing applications.

Versions can be specified in the URL, such as https://localhost:xxxx/api/v1/Person, or passed as a header, e.g., 'X-Api-Version: 1'.

The following packages support versioning in .NET applications: Asp.Versioning.Mvc and Asp.Versioning.Mvc.ApiExplorer.

By using annotations on controller actions, developers can specify the appropriate version for each action (method).


## Brief section on how you would approach onboarding or enabling other developers to work with your solution

I would begin onboarding by discussing the business value of implementing the solution.

Once the development team is aligned, the next step is to review the solution architecture.

It is crucial for the entire development team to understand the high-level overview before diving into the details.

I would start by running the solution to demonstrate the end result.

Moving into the technical details, I would explain how the Core layer is constructed, followed by the Presentation layer.

I would conclude with a walkthrough of the Integration layer (external service calls) and the unit testing project.

I would encourage everyone to question every design decision, enabling us to create a revamping plan with a list of potential improvements for the solution.

Finally, for hands-on experience, I would ensure each developer reads the "How to Run and Test the Solution Locally" section in the Readme file and successfully runs the solution locally.

This results in a development team that understands the business value, technical aspects, and is ready to take on assigned tasks.


## Personal Reflection

Similar Projects or Experiences:
--------------------------------
A recent project similar to this, but with different business logic, was at Tele2.

My team was responsible for developing a service that other APIs could call to retrieve information about roles and permissions.

The business value of the service was to provide a unified API that all Tele2 brands could use to verify the caller's authorization.

The service validated a list of roles and permissions against a user, with user identification passed from the calling API as a claim in a JWT token.

The response indicated whether the user was authorized to call the originating API.

Although the business logic was straightforward, the functional and non-functional requirements were extensive and required careful handling.

The service included several functionalities, such as integration with the ELK stack and an audit trail layer.

Challenges and Interesting Aspects of the Assignment:
-----------------------------------------------------
The assignment was interesting because, despite the simple business logic, the requirements covers nearly all aspects of the software development lifecycle.

It involves development, testing, error handling, extensibility, documentation, deployment, and team orientation.

Improvements or Extensions with More Time:
-------------------------------------------
Based on my experience with a similar project, the following functionalities could be added to the solution:

Avoid string comparisons, for instance, use an enum for Insurance Type.

Store data for Person, Vehicle, and insurances in a database.

Add an audit trail layer to log details about who is calling the service, the information requested, and the timestamp of each call.

Integrate Serilog and the ELK stack for logging and visualization.

Implement resilience and fault tolerance using a library like Polly.

Use AutoMapper to map Entities to Response Models.

Include integration tests.

Set up CI/CD pipelines in Azure DevOps, including build pipelines (using GitHub repository code as input) and release pipelines.

Explain ELK stack

API security best practices

