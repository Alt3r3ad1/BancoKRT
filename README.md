# BancoKRT

The main objective of this project is to create a simple system that can manage PIX transaction limits for customers, carrying out the appropriate validations for their executions and making it possible to visualize the data.

The application was made available on the web using Razor Pages and its business rules were developed in C# together with the API, and stored in the DynamoDB database (AWS) to enable external consumption of the environment.

## Table of Contents
- [Usage](#usage)
- [License](#license)

## Usage

In order to use the project, it is necessary to enter the AWS region, accesskey and secretkey for connecting to the database in the "appsettings.json" file. The data tables needed for the system to work will be generated automatically in DynamoDB if the user informed has access to carry out the process, otherwise it will be necessary to create the tables manually and then use the application.

## License

This project is licensed under the MIT License - see the file [LICENSE.md](LICENSE.md) for details.
