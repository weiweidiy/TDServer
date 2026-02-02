


using JFramework;
using Microsoft.EntityFrameworkCore;

namespace TiktokGame2Server.Entities
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)/*, IGameDataStore*/
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<LevelNode> LevelNodes { get; set; }
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<FormationDeploy> FormationDeploy { get; set; }
        public DbSet<HpPool> HpPools { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<BagSlot> BagSlots { get; set; }
        public DbSet<BagItem> BagItems { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingUpgradSchedule> BuildingUpgradSchedules { get; set; }
        public DbSet<GuideProgress> GuideProgress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LevelNode>().HasIndex(c => new { c.BusinessId, c.PlayerId }).IsUnique();
            //modelBuilder.Entity<Samurai>().HasIndex(c => new { c.Uid, c.PlayerId }).IsUnique();
            modelBuilder.Entity<FormationDeploy>().HasIndex(c => new { c.FormationType, c.SamuraiId , c.PlayerId }).IsUnique();
            modelBuilder.Entity<FormationDeploy>().HasIndex(c => new { c.FormationType, c.FormationPoint, c.PlayerId }).IsUnique();
            modelBuilder.Entity<HpPool>().HasIndex(c => new { c.PlayerId }).IsUnique();
            modelBuilder.Entity<Currency>().HasIndex(c => new { c.PlayerId , c.CurrencyType}).IsUnique();
            modelBuilder.Entity<BagSlot>().HasIndex(c => new { c.PlayerId, c.ItemId }).IsUnique();
            modelBuilder.Entity<Formation>().HasIndex(c => new {  c.PlayerId , c.FormationType }).IsUnique();
            modelBuilder.Entity<Building>().HasIndex(c => new { c.PlayerId, c.BusinessId }).IsUnique();
            modelBuilder.Entity<BuildingUpgradSchedule>().HasIndex(c => new { c.PlayerId, c.BuildingId }).IsUnique();
            modelBuilder.Entity<GuideProgress>().HasIndex(c => new { c.PlayerId }).IsUnique();

            modelBuilder.Entity<BagSlot>()
                .HasOne(b => b.BagItem)
                .WithOne(i => i.BagSlot)
                .HasForeignKey<BagItem>(i => i.BagSlotId);


            // 确保DeviceId唯一
            //modelBuilder.Entity<Account>()
            //    .HasIndex(u => u.PlayerId)
            //    .IsUnique();
        }

        //public Task ClearAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> ExistsAsync(string key)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<string>> GetAllKeysAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<T> GetAsync<T>(string key)
        //{
        //    //switch(nameof(T))
        //    //{
        //    //    case nameof(Player):
        //    //        return Players.FindAsync(key).Result;
        //    //}
        //    throw new NotImplementedException();
        //}

        //public Task<bool> RemoveAsync(string key)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task SaveAsync<T>(string key, T value)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
