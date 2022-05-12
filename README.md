# Payment Gateway (CKO)
To build a payment gateway, an API based application that will allow a merchant to offer a way for their shoppers to pay for their product.
## Requirements :memo:
- [x] Process a payment through the payment gateway and receive either a successful or unsuccessful response.
- [x] Retrieve the details of a previously made payment.
## Assumptions
1. Mock Acquiring bank should either `Approve` Or `Decline` the payment
2. The MerchantId is determined based on the API-Key passed into the request
3. CVV codes could be 3 or 4 digits
4. Card Numbers should be masked (8 digits should be masked) before Logging and fetching the previous Payments
5. PaymentId is generated by merchant not gateway (to prevent duplicate payment)
## High Level Design (UML - Sequence Diagram)

![PaymentGateway_UML_SEQUENCE](https://github.com/DKB1990/PaymentGateway/blob/main/images/PaymentGateway_UML.png?raw=true)

## Architecture
CQRS is an architectural pattern that separates the models for reading and writing data. The basic idea is that you can divide a system's operations into two sharply separated categories ([CQRS pattern MSDN](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)):

`Queries:` These queries return a result and do not change the state of the system, and they are free of side effects.<br/>
`Commands:` These commands change the state of a system.

![CQRS pattern](https://github.com/DKB1990/PaymentGateway/blob/main/images/CQRS.png?raw=true)

## Project Structure

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
1. Unit Tesiting and Integration Testing using X-Unit
2. Masking Card Number before storing logs as per PCI-DSS policy
3. Model validation with FluentValidation
4. Implemented the API Document by adding Swagger UI (`<url>/doc/`)
5. Implementation of CQRS pattern for separation of operations (Read, Write/Update/Delete) 

## Build & Installation
#### Pre-requisites
1. .NET Core 6.0
2. Nuget Package: `FluentValidation`, `X-Unit`, `Swashbuckle.AspNetCore`, `AutoMapper`, `MediatR`, `Newtonsoft.JSON`

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
`Not Found (404)`: Payment with id could not be found

## Future Improvements :wrench:
1. Implementing Event Based pattern for Acquiring Bank
2. Adding more Unit-Test cases on Command, Query models
3. Connecting the logger with AppDynamics Or RedShift to make some ML models for future predictions
4. Implementing Retry mechanism to the immediate failed Or TimedOut scenarios
5. Adding more validations (To make system PCI DSS complaint)
6. Replacing In-Memory database with T-SQL database


