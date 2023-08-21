# WPF Invoice System

![Image of the project](https://i.imgur.com/Pz7V8OO.png)

---

## Table of Contents

- [Description](#description)
- [Key features](#key-features)
- [How to install](#how-to-install)
- [Author Info](#Author-Info)

## Description

I came up with this idea while working in my family business. 
They had stopped using the previous invoice system of the company because 
it had many bugs and the license was expensive. In addition, I have always 
been interested in building a Windows desktop application, and I knew C#, 
so I decided to make a new invoice system for the company. 
The project was a challenge for my Object Oriented Programming skills, 
and I even had the opportunity to put into practice some design patterns. 
The project in this portfolio is a lightweight version of that
project because the original project has more company-related 
features that I decided not to include here.

### Key features

- It uses the Prism library for an easier way of handling navigation.
- It uses SQLite for data persistece so that the people who want to try this
  project can have their data in the desktop directory of their computers.
- SQLite DB is managed through Entity Framework code-first approach.

### Technologies

- .Net
- C#
- WPF
- Prism
- SQLite
- Entity Framework

---

## How to install?
There are two ways you can try this project. You can generate the executable or download it directly 
from here using the link in the Release section.

If you want to generate the executable yourself, you need to have the .Net SDK installed on a 
Windows machine and then follow these steps:

1. Get the source code from this repository.
2. Open a terminal, go to the project directory, and then to the WPFInvoiceSystem/ directory you can find inside.
3. Once there, run the following command: dotnet publish -c release --self-contained.
4. Once the command finishes its execution, you can find the executable in /WPFInvoiceSystem/WPFInvoiceSystem/bin/release/net7.0-windows/win-x64/publish

The program automatically detects if a Sqlite db file exists in the Desktop directory; if it doesn't, it creates it.

[Back To The Top](#WPF-Invoice-System)

## Author Info

- [My Portfolio](enrique-perez-portfolio.netlify.app)
- [LinkedIn](https://www.linkedin.com/in/enrique-perez28/)
- [Twitter](https://twitter.com/jesus93enrique)

[Back To The Top](#WPF-Invoice-System)
