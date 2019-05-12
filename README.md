# Checkout

.net core 2.2 based web API for managing customers orders for imaginary e-commerce site

## CheckoutAPI Project  
### Assumptions made and explanation of choices made:  
Model objects are Customer <- Basket <- BasketProduct -> Product  
Would use an automapper like Hibernate in a real world solution to communicate with the database.  
There is always a Basket object linked to a Customer. The assumption being that when an order is complete the basket will get migrated to an order / purchase model or a new basket will get created as part of the order completion process.  
BasketProduct is used to define the quantity of a product in a basket.  
Also added a docker file to containerize the CheckoutAPI project so it could be hosted in AWS EC2.  

### Endpoints:  
#### Customer  
+ Create `POST api/customer/`  
+ Get `GET api/customer/{id}/`  

#### Product  
+ Create ` POST api/product/`  
+ Get all `GET api/product/`  
+ Get specific `GET api/product/{id}/`  

#### Basket  
+ Get `GET api/basket/{id}`  
+ Get products `GET api/basket/{id}/products`  
+ Add products `POST api/basket/{id}/products/`  
+ Update products `PUT api/basket/{id}/products/`  
+ Remove product `DELETE api/basket/{basketId}/products/{basketProductId}/`  
+ Empty `DELETE api/basket/{id}/products`  

## Checkout Project (Library)  
Provide methods to make use of all the API endpoints.  
Also provides model object for results and calls that get serialize and deserialize as required.  

## Tutorials Followed:   
https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.2&tabs=visual-studio-mac  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2  
https://docs.microsoft.com/en-us/dotnet/core/docker/build-container  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.2  
https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient  
https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data  
