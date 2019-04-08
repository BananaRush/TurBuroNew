using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ModelData.Model.Database;
using ModelData.Models.Database;
using System.Collections.Generic;

namespace StorageAPI.Models.Database
{
    class UserContextInit : DropCreateDatabaseAlways<TourAgencyContext>
    {
        protected override void Seed(TourAgencyContext context)
        {
            context.LanguageModels.AddRange(new List<LanguageModel>() {
                new LanguageModel()
                {
                     Name = "Русский",
                     CodeLang = "RU",
                     IsActive = true
                },
                new LanguageModel()
                {
                     Name = "Aнглиский",
                     CodeLang = "EN",
                     IsActive = false
                },
                new LanguageModel()
                {
                     Name = "Китайский",
                     CodeLang = "CN",
                     IsActive = false
                }
            });

            context.TerminalsModels.Add(new TerminalsModel()
            {
                TerminalId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
                Name = "LoacalHost",
                IsAutorizate = true
            });

            // сохраняем 
            context.SaveChanges();
        }
    }

    public partial class TourAgencyContext : DbContext
    {
        //
        public TourAgencyContext() : base("DataContext")
        {
            //System.Data.Entity.Database.SetInitializer(new UserContextInit());
        }

        public virtual DbSet<NewsModel> NewsModel { get; set; }
        public virtual DbSet<Slider> Slider { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<User_Test> User_Test { get; set; }
        public virtual DbSet<VideoConnect> VideoConnects { get; set; }
        public virtual DbSet<Live> LivesString { get; set; }
        public virtual DbSet<Information> Information { get; set; }
        public virtual DbSet<UrlInfo> UrlInfos { get; set; }
        public virtual DbSet<UrlListInfo> UrlListInfos { get; set; }
        public virtual DbSet<PassageImage> PassageImages { get; set; }
        public virtual DbSet<ImageList> ImageLists { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<VideoModel> VideoModels { get; set; }
        public virtual DbSet<SurveyModel> Survey { get; set; }
        public virtual DbSet<SurveyValueModel> SurveyValue { get; set; }
        public virtual DbSet<UIElementModel> UIElements { get; set; }
        public virtual DbSet<LanguageModel> LanguageModels { get; set; }
        public virtual DbSet<TerminalsModel> TerminalsModels { get; set; }
        public virtual DbSet<TerminalDataModel> TerminalDataModels { get; set; }
        public virtual DbSet<VideoGuideModel> VideoGuideModels { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsModel>()
                .Property(e => e.IconUri)
                .IsUnicode(false);

            modelBuilder.Entity<Section>()
                .HasMany(p => p.Children)
                .WithOptional(p => p.Parent)
                .HasForeignKey(p=>p.ParnetId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Section>()
                .HasMany(p => p.Disciples)
                .WithOptional(p => p.Section)
                .HasForeignKey(p=>p.SectionsId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.User_Test)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.User_Test)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UrlInfo>()
                .HasMany(e => e.UrlListInfos)
                .WithOptional(e => e.urlInfo)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<PassageImage>()
                .HasMany(e => e.imageLists)
                .WithOptional(e => e.passageImage)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<NewsModel>()
                .HasMany(r => r.ListInformation)
                .WithOptional(r => r.Button)
                .HasForeignKey(r=>r.ButtonId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<NewsModel>()
                 .HasMany(r => r.ListPassageImages)
                 .WithOptional(r => r.Button)
                 .HasForeignKey(r => r.ButtonId)
                 .WillCascadeOnDelete(true);

           modelBuilder.Entity<NewsModel>()
                 .HasMany(r => r.ListUrlInfo)
                 .WithOptional(r => r.Button)
                 .HasForeignKey(r => r.ButtonId)
                 .WillCascadeOnDelete(true);

            // Секция должна удаляться вместе с конопкой
           modelBuilder.Entity<NewsModel>()
                 .HasMany(r => r.ListSections)
                 .WithOptional(r => r.Button)
                 .HasForeignKey(r => r.ButtonId)
                 .WillCascadeOnDelete(false);

           modelBuilder.Entity<NewsModel>()
                 .HasMany(r => r.ListVideo)
                 .WithOptional(r => r.Button)
                 .HasForeignKey(r => r.ButtonId)
                 .WillCascadeOnDelete(true);

           modelBuilder.Entity<NewsModel>()
                 .HasMany(r => r.ListTerminal)
                 .WithRequired(r => r.newsModel)
                 .HasForeignKey(r => r.NewsModelId)
                 .WillCascadeOnDelete(true);

            modelBuilder.Entity<SurveyModel>()
                 .HasMany(r => r.ListOption)
                 .WithOptional(r => r.Survey)
                 .HasForeignKey(r => r.SurveyId)
                 .WillCascadeOnDelete(true);

           //modelBuilder.Entity<UIElementModel>()
           //      .HasMany(r => r.ListTerminal)
           //      .WithRequired(r => r.uIElementModel)
           //      .HasForeignKey(r => r.UIElementModelId)
           //      .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
