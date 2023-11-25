using System;
using System.Transactions;
using IMDB.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Import the namespace for ILogger

namespace IMDB.Entity
{
    public class Transaction
    {
        private readonly DbContext _dbContext;
        private readonly ILogger<Transaction> _logger; // Use ILogger<Transaction>

        public Transaction(DbContext dbContext, ILogger<Transaction> logger) // Use ILogger<Transaction>
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public void PerformDatabaseOperations()
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var newEntity = new myEntity
                    {
                        Property1 = "Value1",
                        Property2 = "Value2",                       
                    };

                    _dbContext.Add(newEntity);
                    _dbContext.SaveChanges();
                 
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while performing database operations.");
                }
            }
        }
    }
}
