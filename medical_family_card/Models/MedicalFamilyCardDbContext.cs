using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace medical_family_card.Models;

public partial class MedicalFamilyCardDbContext : DbContext
{
    public MedicalFamilyCardDbContext()
    {
    }

    public MedicalFamilyCardDbContext(DbContextOptions<MedicalFamilyCardDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Allergy> Allergies { get; set; }

    public virtual DbSet<Blood> Bloods { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<FriendType> FriendTypes { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Ht> Hts { get; set; }

    public virtual DbSet<ImgVisit> ImgVisits { get; set; }

    public virtual DbSet<RhesusFactor> RhesusFactors { get; set; }

    public virtual DbSet<Usr> Usrs { get; set; }

    public virtual DbSet<UsrInfo> UsrInfos { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<Wt> Wts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-I27OFMM\\SQLEXPRESS;Database=medical_family_card_db;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.HasKey(e => e.AllergyId).HasName("PK__allergy__ACDD0692E0F92BA8");

            entity.ToTable("allergy");

            entity.Property(e => e.AllergyId).HasColumnName("allergy_id");
            entity.Property(e => e.AllergyComment).HasColumnName("allergy_comment");
            entity.Property(e => e.AllergyName).HasColumnName("allergy_name");
            entity.Property(e => e.UsrId).HasColumnName("usr_id");

            entity.HasOne(d => d.Usr).WithMany(p => p.Allergies)
                .HasForeignKey(d => d.UsrId)
                .HasConstraintName("FK__allergy__usr_id__3D5E1FD2");
        });

        modelBuilder.Entity<Blood>(entity =>
        {
            entity.HasKey(e => e.BloodId).HasName("PK__blood__24E36D6F4B05A394");

            entity.ToTable("blood");

            entity.Property(e => e.BloodId).HasColumnName("blood_id");
            entity.Property(e => e.BloodName).HasColumnName("blood_name");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.FriendId);

            entity.ToTable("friend");

            entity.Property(e => e.FriendId).HasColumnName("friend_id");
            entity.Property(e => e.FriendTypeId).HasColumnName("friend_type_id");
            entity.Property(e => e.FromUsrId).HasColumnName("from_usr_id");
            entity.Property(e => e.ToUsrId).HasColumnName("to_usr_id");

            entity.HasOne(d => d.FriendType).WithMany()
                .HasForeignKey(d => d.FriendTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__friend__friend_t__3B75D760");

            entity.HasOne(d => d.FromUsr).WithMany()
                .HasForeignKey(d => d.FromUsrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__friend__from_usr__398D8EEE");

            entity.HasOne(d => d.ToUsr).WithMany()
                .HasForeignKey(d => d.ToUsrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__friend__to_usr_i__3A81B327");
        });

        modelBuilder.Entity<FriendType>(entity =>
        {
            entity.HasKey(e => e.FriendTypeId).HasName("PK__friend_t__D9A5073BC2307F73");

            entity.ToTable("friend_type");

            entity.Property(e => e.FriendTypeId).HasColumnName("friend_type_id");
            entity.Property(e => e.FriendTypeName).HasColumnName("friend_type_name");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__gender__9DF18F872EAB860C");

            entity.ToTable("gender");

            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.GenderName).HasColumnName("gender_name");
        });

        modelBuilder.Entity<Ht>(entity =>
        {
            entity.HasKey(e => e.HtId).HasName("PK__ht__6E7D72B7A002DED5");

            entity.ToTable("ht");

            entity.Property(e => e.HtId).HasColumnName("ht_id");
            entity.Property(e => e.HtDate)
                .HasColumnType("date")
                .HasColumnName("ht_date");
            entity.Property(e => e.HtValue).HasColumnName("ht_value");
            entity.Property(e => e.UsrId).HasColumnName("usr_id");

            entity.HasOne(d => d.Usr).WithMany(p => p.Hts)
                .HasForeignKey(d => d.UsrId)
                .HasConstraintName("FK__ht__usr_id__412EB0B6");
        });

        modelBuilder.Entity<ImgVisit>(entity =>
        {
            entity.HasKey(e => e.ImgId).HasName("PK__img_visi__6F16A71CA99146C9");

            entity.ToTable("img_visit");

            entity.Property(e => e.ImgId).HasColumnName("img_id");
            entity.Property(e => e.ImgData).HasColumnName("img_data");
            entity.Property(e => e.ImgName).HasColumnName("img_name");
            entity.Property(e => e.VisitId).HasColumnName("visit_id");

            entity.HasOne(d => d.Visit).WithMany(p => p.ImgVisits)
                .HasForeignKey(d => d.VisitId)
                .HasConstraintName("FK__img_visit__visit__160F4887");
        });

        modelBuilder.Entity<RhesusFactor>(entity =>
        {
            entity.HasKey(e => e.RhesusFactorId).HasName("PK__rhesus_f__9FC815184AA4268A");

            entity.ToTable("rhesus_factor");

            entity.Property(e => e.RhesusFactorId).HasColumnName("rhesus_factor_id");
            entity.Property(e => e.RhesusFactorName).HasColumnName("rhesus_factor_name");
        });

        modelBuilder.Entity<Usr>(entity =>
        {
            entity.HasKey(e => e.UsrId).HasName("PK__usr__60621ABC92B7924D");

            entity.ToTable("usr");

            entity.Property(e => e.UsrId).HasColumnName("usr_id");
            entity.Property(e => e.UsrEmail).HasColumnName("usr_email");
            entity.Property(e => e.UsrName).HasColumnName("usr_name");
            entity.Property(e => e.UsrPassword).HasColumnName("usr_password");
        });

        modelBuilder.Entity<UsrInfo>(entity =>
        {
            entity.HasKey(e => e.UsrId).HasName("PK__usr_info__60621ABCE691502C");

            entity.ToTable("usr_info");

            entity.HasIndex(e => e.UsrId, "UQ__usr_info__60621ABD071DA7AE").IsUnique();

            entity.Property(e => e.UsrId)
                .ValueGeneratedNever()
                .HasColumnName("usr_id");
            entity.Property(e => e.BloodId).HasColumnName("blood_id");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.ImgData).HasColumnName("img_data");
            entity.Property(e => e.ImgName).HasColumnName("img_name");
            entity.Property(e => e.RhesusFactorId).HasColumnName("rhesus_factor_id");
            entity.Property(e => e.UsrInfoBirthday)
                .HasColumnType("date")
                .HasColumnName("usr_info_birthday");
            entity.Property(e => e.UsrInfoFirstName).HasColumnName("usr_info_first_name");
            entity.Property(e => e.UsrInfoLastName).HasColumnName("usr_info_last_name");
            entity.Property(e => e.UsrInfoMiddleName).HasColumnName("usr_info_middle_name");

            entity.HasOne(d => d.Blood).WithMany(p => p.UsrInfos)
                .HasForeignKey(d => d.BloodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usr_info__blood___36B12243");

            entity.HasOne(d => d.Gender).WithMany(p => p.UsrInfos)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usr_info__gender__35BCFE0A");

            entity.HasOne(d => d.RhesusFactor).WithMany(p => p.UsrInfos)
                .HasForeignKey(d => d.RhesusFactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usr_info__rhesus__37A5467C");

            entity.HasOne(d => d.Usr).WithOne(p => p.UsrInfo)
                .HasForeignKey<UsrInfo>(d => d.UsrId)
                .HasConstraintName("FK__usr_info__usr_id__34C8D9D1");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.VisitId).HasName("PK__visit__375A75E1B002203E");

            entity.ToTable("visit");

            entity.Property(e => e.VisitId).HasColumnName("visit_id");
            entity.Property(e => e.UsrId).HasColumnName("usr_id");
            entity.Property(e => e.VisitComment).HasColumnName("visit_comment");
            entity.Property(e => e.VisitCost).HasColumnName("visit_cost");
            entity.Property(e => e.VisitEndDate)
                .HasColumnType("date")
                .HasColumnName("visit_end_date");
            entity.Property(e => e.VisitName).HasColumnName("visit_name");
            entity.Property(e => e.VisitStartDate)
                .HasColumnType("date")
                .HasColumnName("visit_start_date");

            entity.HasOne(d => d.Usr).WithMany(p => p.Visits)
                .HasForeignKey(d => d.UsrId)
                .HasConstraintName("FK__visit__usr_id__440B1D61");
        });

        modelBuilder.Entity<Wt>(entity =>
        {
            entity.HasKey(e => e.WtId).HasName("PK__wt__05D3885E5D120BD1");

            entity.ToTable("wt");

            entity.Property(e => e.WtId).HasColumnName("wt_id");
            entity.Property(e => e.UsrId).HasColumnName("usr_id");
            entity.Property(e => e.WtDate)
                .HasColumnType("date")
                .HasColumnName("wt_date");
            entity.Property(e => e.WtValue).HasColumnName("wt_value");

            entity.HasOne(d => d.Usr).WithMany(p => p.Wts)
                .HasForeignKey(d => d.UsrId)
                .HasConstraintName("FK__wt__usr_id__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
