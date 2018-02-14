---
services: ECommerce demo 
platforms: DotNetCore
author: David Chen
---

# Simple ECommerce demo

`This sample contains a solution file that contains five projects: 
Commonshare: it is common cross shared library;
ECommerce.Core: it is a core business layer ibrary;
ECommerce.Code.Data: It is a data service layer ibrary;
ECommerce.WebAPI: It is an ASPNETCORE restful API;
ECommerce.Web: It is an ASPNETCORE angular 4+ single page application.
`


The sample covers the full statck devloper as the following: 

* Entity Framework Core usage;
* Use JWT tokens with Bearer authentication;
* Authorization;
* Swagger support;
* Data format can be xml or json;
* API can also support Cors policy settings;
* Use dependent injection;
* Apply SOLID principles;
* Repository Patten;
* Angular 4+;
* And more


## How To Run This Sample


## Using the development environment to run the sample


### Step 1: Clone or download this repository

From your shell or command line:

```
git clone 
```

### Step 2: Check nodejs, node are installed correctly

### Step 3 Open the `Ecommerce.sln.sln` in Visual Studio 2017 run as administrator

3.1 Check SQL work fine (optional)
Currently I use (localdb)\\mssqllocaldb. Search and replace;

3.2 Run npm install (optional)
Open a command window as administrator;
go to ECommerce.WebAPI;
Run npm install

3.3 Run the ECommerce.WebAPI;
Right click ECommerce.WebAPI project and debug as a new instance;
Wait a couple of seconds, until a swagger UI popup;
Check your database server=(localdb)\\mssqllocaldb to see created;

3.4 Run the ECommerce.Web
Right click ECommerce.Web project and debug as a new instance;
For login use UserName = "david.chen@startinnovations.com" and Password = "P@ssw0rd!"

