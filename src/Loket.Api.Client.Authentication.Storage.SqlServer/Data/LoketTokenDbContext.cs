using Loket.Api.Client.Authentication.Storage.SqlServer.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Loket.Api.Client.Authentication.Storage.SqlServer.Data;

public partial class LoketTokenDbContext(DbContextOptions<LoketTokenDbContext> options, IOptions<LoketEntityFrameworkCoreStorageOptions> storageOptions) : DbContext(options)
{
    private readonly LoketEntityFrameworkCoreStorageOptions _storageOptions = storageOptions.Value;

    public DbSet<LoketToken> Tokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoketToken>().ToTable(_storageOptions.TableName);

        modelBuilder.Entity<LoketToken>().Property(p => p.RefreshToken).HasColumnName(_storageOptions.RefreshTokenColumnName);
        modelBuilder.Entity<LoketToken>().Property(p => p.RefreshTokenUpdatedAt).HasColumnName(_storageOptions.RefreshTokenUpdatedAtColumnName);

        modelBuilder.Entity<LoketToken>().Property(p => p.AccessToken).HasColumnName(_storageOptions.AccessTokenColumnName);
        modelBuilder.Entity<LoketToken>().Property(p => p.AccessTokenUpdatedAt).HasColumnName(_storageOptions.AccessTokenUpdatedAtColumnName);
        modelBuilder.Entity<LoketToken>().Property(p => p.AccessTokenExpire).HasColumnName(_storageOptions.AccessTokenExpireColumnName);
    }
}