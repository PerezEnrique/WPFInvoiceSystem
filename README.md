# WPF Invoice System

![Image of the project](https://i.imgur.com/DIWnDib.png)

---

## Table of Contents

- [Description](#description)
- [Key features](#key-features)
- [How to install](#how-to-install)
- [Author Info](#Author-Info)

## Description

I came up with this idea while working in my family business. 
They had stopped using the previous invoice system of the company because 
it had many bugs and the license was expensive. In addition, I had always 
been interested in building a Windows desktop application, and I knew C#, 
so I decided to make a new invoice system for the company. 
The development was a challenge for my Object Oriented Programming skills, 
and I even had the opportunity to put into practice some design patterns. 
The project in this portfolio is a lightweight version of that
project because the latter has more company-related 
features that I decided not to include here.

### Key features

- API project uses APS.Net Core dependency injection system to provide application layer with tools implementations.
- WPF Client follows MVVM architectural pattern and Prism library for an easier way of handling navigation.
- SQLite is used for data persistece and is managed through Entity Framework code-first approach.
- Excel report generation with ClosedXML.

### Technologies

- .Net
- C#
- ASP.Net Core
- SQLite
- Entity Framework
- WPF
- Prism

---

## How to install the WPF app?
There are two ways you can get it. You can generate the executable or download it directly 
from here using the link in the Releases section.

If you want to generate the executable yourself, you need to have the .Net SDK installed on a 
Windows machine and then follow these steps:

1. Get the source code from this repository.
2. Open a terminal, go to the project directory, and then to the WPFInvoiceSystem.WPFClient directory.
3. Once there, run the following command: dotnet publish -c release --self-contained.
4. Once the command finishes its execution, you can find the executable in /bin/Release/net8.0-windows10.0.22621.0/win-x64/WPFInvoiceSystem.WPFClient.exe

The url to the project's API has been already set in the appsettings.json file of the WPF app.

[Back To The Top](#WPF-Invoice-System)

## Author Info

- [My Portfolio](enrique-perez-portfolio.netlify.app)
- [LinkedIn](https://www.linkedin.com/in/enrique-perez28/)
- [Twitter](https://twitter.com/jesus93enrique)

[Back To The Top](#WPF-Invoice-System)
