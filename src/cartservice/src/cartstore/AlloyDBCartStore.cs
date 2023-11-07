using System;
using Grpc.Core;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;

namespace cartservice.cartstore
{
    public class AlloyDBCartStore : ICartStore
    {
        private readonly string tableName;
        private readonly string connectionString;

        // Constructor and other methods remain unchanged...

        public async Task AddItemAsync(string userId, string productId, int quantity)
        {
            Console.WriteLine($"AddItemAsync for {userId} called");
            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                await using var transaction = await connection.BeginTransactionAsync();

                var fetchCmd = $"SELECT quantity FROM {tableName} WHERE userID='{userId}' AND productID='{productId}'";
                var currentQuantity = 0;
                await using (var cmdRead = new NpgsqlCommand(fetchCmd, connection))
                {
                    await using (var reader = await cmdRead.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                            currentQuantity += reader.GetInt32(0);
                    }
                }

                var totalQuantity = quantity + currentQuantity;

                var insertCmd = $"INSERT INTO {tableName} (userId, productId, quantity) VALUES ('{userId}', '{productId}', {totalQuantity}) ON CONFLICT (userId, productId) DO UPDATE SET quantity = {totalQuantity}";
                await using (var cmdInsert = new NpgsqlCommand(insertCmd, connection))
                {
                    await cmdInsert.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Since transaction is declared in the try block, we need to handle a potential null reference here
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                throw new RpcException(
                    new Status(StatusCode.FailedPrecondition, $"Can't access cart storage at {connectionString}. {ex}"));
            }
        }

        // Other methods...

        public async Task EmptyCartAsync(string userId)
        {
            Console.WriteLine($"EmptyCartAsync called for userId={userId}");

            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                await using var transaction = await connection.BeginTransactionAsync();
                
                var deleteCmd = $"DELETE FROM {tableName} WHERE userID = '{userId}'";
                await using (var cmd = new NpgsqlCommand(deleteCmd, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Since transaction is declared in the try block, we need to handle a potential null reference here
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                throw new RpcException(
                    new Status(StatusCode.FailedPrecondition, $"Can't access cart storage at {connectionString}. {ex}"));
            }
        }

        // Ping method remains unchanged...
    }
}

