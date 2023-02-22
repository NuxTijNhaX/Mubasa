# Mubasa
![GitHub top language](https://img.shields.io/github/languages/top/NuxTijNhaX/mubasa)
![GitHub issues](https://img.shields.io/github/issues/NuxTijNhaX/Mubasa)
![GitHub closed pull requests](https://img.shields.io/github/issues-pr-closed/NuxTijNhaX/Mubasa)
![GitHub](https://img.shields.io/github/license/NuxTijNhaX/Mubasa)
> Mubasa stands for Mua Bán Sách which is an e-commerce website for selling books, built on ASP.NET Core.
> Live demo [_here_](https://mubasa.bsite.net/).

## Table of Contents
- [Mubasa](#mubasa)
  - [Table of Contents](#table-of-contents)
  - [Technologies Used](#technologies-used)
  - [Features](#features)
  - [Screenshots](#screenshots)
  - [Setup](#setup)
    - [Docker](#docker)
    - [Visual Studio and SQL Server](#visual-studio-and-sql-server)
      - [Prerequisites](#prerequisites)
      - [Steps to run](#steps-to-run)
  - [Room for Improvement](#room-for-improvement)
  - [Acknowledgements](#acknowledgements)
  - [Contact](#contact)
  - [License](#license)
<!-- * [License](#license) -->

## Technologies Used
- ASP.NET Core MVC - v6.0
- Entity Framework Core - v6.0
- ASP.NET Identity Core - v6.0


## Features
- For Customer:
  - Browse, search products
  - Manage shopping cart
  - Manage order history
  - Pay with MoMo e-wallet
  - Manage personal information
  - Login/register with Facebook, Google
- For Admin:
  - Manage website content (product, author, supplier, publisher, cover type,...)
  - Manage the order
  - Create an account for the employee


## Screenshots
- Website
![Website screenshot](https://iili.io/HGUUwwQ.png)
- Database Diagram
![Database screenshot](https://iili.io/HGUi6Rn.png)
<!-- If you have screenshots you'd like to share, include them here. -->


## Setup
### Docker
For testing purpose only `docker-compose up`
### Visual Studio and SQL Server
#### Prerequisites

- SQL Server
- Visual Studio

#### Steps to run

- Update the connection string in appsettings.json in Mubasa.Web
- Update the `AddDbContext` to use `UseSqlServer` in Mubasa.Web/Installers/DatabseInstallers.cs
- Build the whole solution.
- In Solution Explorer, make sure that Mubasa.Web is selected as the Startup Project
- In Visual Studio, press "Control + F5".
- The back-office can be accessed via /Admin using the following built-in account: root.user@mubasa.com, Root.user.01@mubasa.com!


## Room for Improvement
Room for improvement:
- Improve speed of query

To do:
- Using indexing database


## Acknowledgements
- This project was inspired by [Fahasa.com](https://www.fahasa.com/)
- This project was based on [this tutorial](https://www.udemy.com/course/complete-aspnet-core-21-course/?referralCode=0533F3B61F426407BE00).


## Contact
Created by [@nhanguyen7901](https://www.linkedin.com/in/nhanguyen7901/) - feel free to contact me!

## License
This project is open source and available under the [MIT License](https://github.com/NuxTijNhaX/Mubasa/blob/master/LICENSE.md).