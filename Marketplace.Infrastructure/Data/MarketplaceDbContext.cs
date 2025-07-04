using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure.Data;

public class MarketplaceDbContext: DbContext
{
    public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options) : base(options){ }
    public DbSet<Product> Products => Set<Product>();

}
