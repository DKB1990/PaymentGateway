# Payment Gateway (CKO)
An API based application that will allow a merchant to offer a way for their shoppers to pay for their product.
## Requirements :memo:
- [x] Process a payment through the payment gateway and receive either a successful or unsuccessful response.
- [x] Retrieve the details of a previously made payment.
## Assumptions
1. Mock Acquiring bank should either `Approve` Or `Decline` the payment
2. The MerchantId is fixed and generated by system internally
3. CVV codes could be 3 or 4 digits
4. Card Numbers should be masked (8 digits should be masked) before Logging and fetching the previous Payments
5. PaymentId is generated by merchant not gateway (to prevent duplicate payment)
## High Level Design (UML - Sequence Diagram)

<img alt="PaymentGateway_UML_SEQUENCE" src= "https://github.com/DKB1990/PaymentGateway/blob/main/images/PaymentGateway_UML.png">

## Architecture
Command Query Segregation Responsilibity (CQRS) is an architectural pattern that separates the models for reading and writing data. The basic idea is that you can divide a system's operations into two sharply separated categories ([CQRS pattern MSDN](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)):

`Queries:` These queries return a result and do not change the state of the system, and they are free of side effects.<br/>
`Commands:` These commands change the state of a system.

<img alt="CQRS pattern" src= "https://github.com/DKB1990/PaymentGateway/blob/main/images/CQRS.png">

## Project Structure
The inspiration to create this project structure came from various sources, `ThoughtWorks Blog`, [Structure a .NET solution](https://www.youtube.com/watch?v=YiVqwoFMieg), [Common Web application Architectures](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures). And Below is the final project structure/architecture.

```code
#--src
  #--PaymentGateway.API
      |--CQRS
      |--Mappers
      |--Controllers
  #--PaymentGateway.Domain
      |--Models
      |--Validations
      |--Enums
      |--Services
      |--Interfaces
  #--PaymentGatewayInfrastructure
      |--Repositories
  #--PaymentGateway.SimulatorBank
#--tests
  #--PaymentGateway.API.UnitTests
  #--PaymentGateway.API.IntegrationTests
  #--PaymentGateway.Domain.UnitTests
```

## Extra Mile Bonus Points :rocket:
1. I have added Unit Tesiting and Integration Testing using X-Unit
2. Implemented logging mechanism by using `Microsoft.Extensions.Logging`
3. To adhere to PCI-DSS policy, I'm masking the Card Number before logging the request
4. Built a pipeline to validate model classes with FluentValidation
5. Implemented the API Document by adding Swagger UI (`<url>/doc/`)
6. Implementation of CQRS pattern for separation of operations (Read, Write/Update/Delete). inspiration: ([CQRS pattern MSDN](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs))

## Build & Installation
#### Pre-requisites
1. .NET Core 6.0, VS 2019 or above
2. Nuget Package: `FluentValidation`, `X-Unit`, `Swashbuckle.AspNetCore`, `AutoMapper`, `MediatR`, `Newtonsoft.JSON`

#### How to Run Project
1. PaymentGateway.API is Set is Start-up project. You may open the solution in VS2019 or above and run the solution
2. Alternatively, goto the PaymentGateway.API project folder and run `Dotnet Run` in terminal within the same folder 
3. Then, Run the Solution and Open Swagger UI: /<projectURL>/docs/
5. Take the below sample JSON request formats to invoke the APIs
6. You may also use POSTMAN (using CURL commands) to invoke APIs

## Sample Request/Response JSON format
`HTTP/1.1` `POST`: /Payment/
```JSON
"request": {
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 999.99,
  "currencyCode": "AED",
  "description": "E-COM payment DUBAI-UAE",
  "cardDetails": {
    "number": "5331445434345556",
    "cvv": "989",
    "expiryDate": {
      "year": 2024,
      "month": 10
    },
    "beneficiaryName": "Dheeraj Bansal"
  }
}

"response (201)": {
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "statusCode": "PENDING"
}
```
`POST - curl`

```Curl
curl -X 'POST' \
  'https://localhost:5001/Payments' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 10,
  "currencyCode": "AED",
  "description": "Apple MAC book Order",
  "cardDetails": {
    "number": "1111222233334444",
    "cvv": "123",
    "expiryDate": {
      "year": 2022,
      "month": 3
    },
    "beneficiaryName": "Dheeraj Bansal"
  }
}'
```
`Response (400 - Bad Request)`
* Invalid CardNumber
* Invalid CVV
* Invalid Expiry Date
* Invalid CurrencyCode
* Invalid Amount

`HTTP/1.1` `GET`: /Payment/{id}

```JSON
"request": {
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

"response (200)"{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 999.99,
  "statusCode": "APPROVED | DECLINED | PENDING",
  "description": "E-COM payment DUBAI-UAE",
  "currencyCode": "AED",
  "declinedReasonCode": "InSufficient Funds", 
  "cardExpiryDate": "10/2024",
  "cardNumber": "5331########5556",
  "beneficiaryName": "Dheeraj Bansal",
  "requestedDateTime": "2022-05-11T18:28:51.297Z",
  "isPaymentDeclined": "true | false"
}
```
`GET - curl`

```curl
curl -X 'GET' \
  'https://localhost:5001/Payments/3fa85f64-5717-4562-b3fc-2c963f66afa6' \
  -H 'accept: application/json'
```
`Not Found (404)`: Payment with id could not be found

## Future Improvements :wrench:
1. Implementing Event Based pattern for Acquiring Bank
2. Adding more Unit-Test cases on Command, Query models
3. Connecting the logger with AppDynamics Or RedShift to make some ML models for future predictions
4. Implementing Retry mechanism to the immediate failed Or TimedOut scenarios
5. Adding more validations (To make system PCI DSS complaint)
6. Replacing In-Memory database with T-SQL database


