using Cottage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cottage.API.Tests
{
	internal class TestDataDbContext
	{
		public CottageContext GetDatabaseContext()
		{
			var options = new DbContextOptionsBuilder<CottageContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			var databaseContext = new CottageContext(options);

			databaseContext.Database.EnsureCreated();

			return databaseContext;
		}
	}
}
