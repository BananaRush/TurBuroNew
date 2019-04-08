namespace StorageAPI.Models.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TourAgencyDataBase : DbContext
    {
        public TourAgencyDataBase()
            : base("name=TourAgency")
        {

        }

        public virtual DbSet<NewsModel> NewsModel { get; set; }
        public virtual DbSet<Slider> Slider { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<User_Test> User_Test { get; set; }
        public virtual DbSet<VideoConnect> VideoConnects { get; set; }
        public virtual DbSet<Live> LivesString { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsModel>()
                .Property(e => e.IconUri)
                .IsUnicode(false);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.User_Test)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.User_Test)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

        }
    }
}
