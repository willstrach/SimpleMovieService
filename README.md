# Simple movies app
A simple API, based on [Disham993's 9000+ Movies Dataset](https://www.kaggle.com/datasets/disham993/9000-movies-dataset), allowing users to browse a list of movies.

## Repository structure
This repo currently has one main directory:
- **`service`**: The API service, implemented with .NET 8 and MSSQL

> **Note:** A client app may be implemented in the future. This app will likely reside in a folder called `client`

## Using the app
In order to execute the code in this repo, the following are required:
- Git
- Docker
- NPM

You may then clone this repo and follow the below instructions.

### Running the API service
Using your command prompt of choice, navigate to the service directory of this repo. You can then start the service and it's database using the docker compose file.

``` bash
docker compose up --build
```

This will build the api service, download the MSSQL image, and seed the database using the [movies dataset](https://www.kaggle.com/datasets/disham993/9000-movies-dataset). Once complete, navigate to [https://localhost:8080/swagger/index.html](https://localhost:8080/swagger/index.html) to view the swagger docs for the API.

