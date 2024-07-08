using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using BancoKRT.API.Middlewares;
using System.Net;
using System.Reflection;

namespace BancoKRT.API.Infrastructure.Services;

public class DynamoDBTableService
{
    private readonly IAmazonDynamoDB _dynamoDBClient;

    public DynamoDBTableService(IAmazonDynamoDB dynamoDBClient)
    {
        _dynamoDBClient = dynamoDBClient;
    }

    public async Task<bool> CreateTableAsync()
    {
        try
        {
            string targetNamespace = "BancoKRT.API.Domain.Models";

            var classes = Assembly.GetExecutingAssembly()
                                  .GetTypes()
                                  .Where(t => t.IsClass && t.Namespace == targetNamespace)
                                  .ToList();

            foreach (var classType in classes)
            {
                if (classType.Name == "Client" || classType.Name == "PIX")
                {
                    do
                    {
                        if (!await TableExistsAsync(classType.Name))
                        {
                            var tableName = classType.Name;
                            var propertyId = classType.GetProperties(BindingFlags.Public | BindingFlags.Instance).First();

                            var attributeDefinitions = new List<AttributeDefinition>();
                            var keySchema = new List<KeySchemaElement>();

                            var attributeType = GetAttributeType(propertyId.PropertyType);
                            if (attributeType != null)
                            {
                                attributeDefinitions.Add(new AttributeDefinition
                                {
                                    AttributeName = propertyId.Name,
                                    AttributeType = attributeType
                                });

                                keySchema.Add(new KeySchemaElement
                                {
                                    AttributeName = propertyId.Name,
                                    KeyType = "HASH" // Partition key
                                });
                            }

                            var request = new CreateTableRequest
                            {
                                TableName = tableName,
                                AttributeDefinitions = attributeDefinitions,
                                KeySchema = keySchema,
                                ProvisionedThroughput = new ProvisionedThroughput
                                {
                                    ReadCapacityUnits = 5,
                                    WriteCapacityUnits = 5
                                }
                            };

                            var response = await _dynamoDBClient.CreateTableAsync(request);

                            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                            {
                                continue;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    while (!await GetTableStatusAsync(classType.Name));
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new HttpException(HttpStatusCode.InternalServerError, $"{ex.Message}");
        }
    }

    private string? GetAttributeType(Type type)
    {
        if (type == typeof(string))
            return "S";
        if (type == typeof(int) || type == typeof(long))
            return "N";
        if (type == typeof(byte[]))
            return "B";

        return null;
    }

    public async Task<bool> TableExistsAsync(string tableName)
    {
        var response = await _dynamoDBClient.ListTablesAsync();

        return response.TableNames.Contains(tableName);
    }

    public async Task<bool> GetTableStatusAsync(string tableName)
    {
        try
        {
            var response = await _dynamoDBClient.DescribeTableAsync(new DescribeTableRequest
            {
                TableName = tableName
            });

            if (response.Table.TableStatus.Equals(TableStatus.ACTIVE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (ResourceNotFoundException)
        {
            return false;
        }
        catch (Exception ex)
        {
            throw new HttpException(HttpStatusCode.InternalServerError, $"{ex.Message}");
        }
    }
}
