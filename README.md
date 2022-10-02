
# File Operations

This project allows developers and individuals to upload/downlaod their contents into popular storage services such as Azure Blob Storage and Amazon S3 Service.

## To-Do
- &#9744; Add Login endpoint for JWT support
- [ ] Add multi-upload/download support

## Set up
To use this service, you need to have Azure Blob Storage account and Amazon S3 account. All these accounts should be accessible via public. Since you have these accounts, you need to register them via appsettings.json file. Here's the example appsettings.json file:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AmazonS3Credentials": {
    "BucketName": "{MY BUCKET}",
    "Region": "{MY_REGION}",
    "AwsAccessKey": "{MY_ACCESS_KEY}",
    "AwsSecretAccessKey": "{MY_SECRET_ACCESS_KEY}"
  },
  "AzureCredentials": {
    "ConnectionString": "{MY_CONNECTION_STRING}",
    "ContainerName": "{MY_CONTAINER}"
  },
  "ElasticConfiguration": {
    "Uri": "http://localhost:9200/"
  }
}
```

#### Clone the project

```bash
  $ git clone https://github.com/TekyaygilFethi/FileOperationsApplication.git
```

Go to project root directory ```./FileOperationsApplication``` and execute the command below to setup ElasticSearch and Kibana:
```bash
  $ docker-compose up -d
```
or you can use your own Elastic Search and Kibana instances.

## Run Locally
Go to project root directory ```./FileOperationsApplication``` and execute command below:
```bash
  $ dotnet run
```

## Run with Docker
Go to project root directory ```./FileOperationsApplication``` and build projects image via the help of Dockerfile with the command below:
```bash
  $ docker build --rm -t  fethitekyaygil-dev/fileoperationsapplication:latest .
```

After build, you can run this image with the command below:

```bash
  $ docker run --rm --name fileoperationsapplication -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 fethitekyaygil-dev/fileoperationsapplication
```

Then you should be able to access site through:

```
http://localhost:5000/swagger
```
## Endpoints

#### ResponseObject
This object is a default type for all API Responses. It contains following fields:
```csharp
public class ResponseObject<T>
{
    public bool IsSuccess { get; set; } = true;
    public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
    public string Message { get; set; } = String.Empty;
    public T Data { get; set; }
}
```
#### Upload

Upload method uploads relevant file to chosen remote service [S3, Azure]. Takes request as 'form'

```http
  POST /api/File/Upload
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `FileDestination`      | `Enum` | The remote service which will file be uploaded to [S3, Azure] |
| `File`      | `file` | File that will be uploaded |
| `RemoteFilePath`      | `string` | The path where file will be uploaded on remote service |

Returns ```ResponseObject<bool>``` typed response. In 'Data' field, the status of upload operation wherell be returned. "true" for success and "false" for error.


#### Download

Download method returns relevant object URL according to the requested filepath and service. Takes request as 'body'

```http
  POST /api/File/Download
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `FileDestination`      | `Enum` | The remote service which will file be downloaded from [S3, Azure] |
| `RemoteFilePath`      | `string` | The path where file will be downloaded from remote service |

Returns ```ResponseObject<string>``` typed response. In 'Data' field, the remote url will be returned.


## Authors

- [@TekyaygilFethi](https://www.github.com/TekyaygilFethi)