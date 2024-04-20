using System.Runtime.CompilerServices;
using solution.DTOs;
using System.Data.SqlClient;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace solution.EndPoints;

public static class AnimalEndpoints
{
    public static void RegisterAnimalsEndpoints(this WebApplication app)
    {
        app.MapGet("api/animals", GetAnimals);
        app.MapPost("api/animals", CreateAnimal);
    }

    private static IResult GetAnimals(IConfiguration configuration, string? orderBy)
    {
        var validParameters = new[] { "name", "description", "category", "area" };
        if (orderBy is null)
            orderBy = "name";
        if (validParameters.Contains(orderBy.ToLower()))
        {
            var response = new List<GetAnimalsResponse>();
            using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default")))
            {
                var sqlCommand = new SqlCommand("SELECT * FROM Animal ORDER BY " + orderBy, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@1", orderBy);
                sqlCommand.Connection.Open();
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    response.Add(new GetAnimalsResponse(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                        )
                    );
                }
            }

            return Results.Ok(response);
        }
        return Results.BadRequest("Invalid orderBy parameter");
    }

    private static IResult CreateAnimal(IConfiguration configuration, IValidator<CreateAnimalRequest> validator, CreateAnimalRequest request)
    {
        var validation = validator.Validate(request);
        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default")))
        {
            var sqlCommand = new SqlCommand("INSERT INTO Animal (Name, Description, Category, Area) OUTPUT INSERTED.IdAnimal VALUES (@1, @2, @3, @4)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@1", request.Name);
            sqlCommand.Parameters.AddWithValue("@2", request.Description);
            sqlCommand.Parameters.AddWithValue("@3", request.Category);
            sqlCommand.Parameters.AddWithValue("@4", request.Area);
            sqlCommand.Connection.Open();

            var id = sqlCommand.ExecuteScalar();
            Console.WriteLine(id);

            return Results.Created($"animals/{id}", new CreateAnimalResponse((int)id, request));
        }
    }
    
    
}