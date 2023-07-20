# codeTestCom

## AUTHOR: Manuel Antonio Gómez Angulo
## START DATE: 14/07/2023
## END DATE: 17/07/2023

It is about a development in C# with .net 6 in a console application
This proyect has been developed from Scratch with the template Web Api .net 6
### Architecture and technologies used
* **Windows 10**
* **Net core 6**
* **Azure Cosmos DB**
* Visual studio 2022 community Edition

### Points completed
* Rent one or several cars and calculate the price.
* Return a car and calculate surcharges (if exist)
* Have an inventory of cars
* Calculate the price for rental
* Keep the track of the customer loyalty poin

Characteristics:
* **Code in english with so love and not so bad style of code (I think)**
* Console application
* Use of TDD
* Asynchronous programming
* Dependency injection with repositorios

APIS:
Rental:
* CalculatePrice
  
GET

https://localhost:7272/api/Rental/CalculatePrice

{
  "UserId": "5334369R",
  "ContractDeliveryDate": "10/08/2023",
  "ContractReturnDate": "12/08/2023",
  "PartitionKey":"Nissan",
  "CarId": "1111AAA",
  "CarType":"Suv"
}
* CalculatePriceAndSurcharges
* 
  GET
  
https://localhost:7272/api/Rental/CalculatePriceAndSurcharges?carId=2222AAA&actualReturnDate=19/09/2023

* RentCar
  
POST

  https://localhost:7272/api/Rental/RentCar
  
  {
  "CarId": "2222AAA",
  "UserId": "5331369R",
  "ContractDeliveryDate": "10/08/2023",
  "ContractReturnDate": "09/09/2023"
}
* RentMultipleCar

  POST

  https://localhost:7272/api/Rental/RentMultipleCar
  
  {
  "rentalRq": {
    "ContractDeliveryDate": "10/08/2023",
    "ContractReturnDate": "09/09/2023",
    "UserId": "5314369R"
  },
  "carIds": [
    "1111BBB",
    "3333AAA",
    "0000BBB"
  ]
}
* ReturnCar

POST

https://localhost:7272/api/Rental/ReturnCar?carId=2222AAA&actualReturnDate=19/09/2023

User:
* GetUserByDni

GET

https://localhost:7272/api/User/GetUserByDni?dni=5331369R

* AddUser

POST

https://localhost:7272/api/User/AddUser

{
    "dni": "5344469R",
    "partitionKey": "Male",
    "name": "Pepeee",
    "surname": "Pacoee",
    "age": 40,
    "loyaltyPoints": 8,
    "sex": "Male"
}

Car
* Add Car

POST

https://localhost:7272/api/Car/AddCar

{
    "id": "6666AAA",
    "name": "Ferrari Testa Rosa",
    "brand": "Ferrari",
    "type": "Premium"
}

### Things to improve:
* **Code with the best style of code (I think)**

So **many thanks to the people** who have given me the test **to prove myself**
