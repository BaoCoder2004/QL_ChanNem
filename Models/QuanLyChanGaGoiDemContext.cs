using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BTL_TKWeb.ModelView;

namespace BTL_TKWeb.Models;

public partial class QuanLyChanGaGoiDemContext : DbContext
{
    public QuanLyChanGaGoiDemContext()
    {
    }

    public QuanLyChanGaGoiDemContext(DbContextOptions<QuanLyChanGaGoiDemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnhHang> AnhHangs { get; set; }

    public virtual DbSet<ChatLieu> ChatLieus { get; set; }

    public virtual DbSet<ChiTietDdh> ChiTietDdhs { get; set; }

    public virtual DbSet<ChiTietHang> ChiTietHangs { get; set; }

    public virtual DbSet<ChiTietHdn> ChiTietHdns { get; set; }

    public virtual DbSet<CongViec> CongViecs { get; set; }

    public virtual DbSet<DanhMucHang> DanhMucHangs { get; set; }

    public virtual DbSet<DonDatHang> DonDatHangs { get; set; }

    public virtual DbSet<HoaDonNhap> HoaDonNhaps { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KichThuoc> KichThuocs { get; set; }

    public virtual DbSet<MauSac> MauSacs { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<TheLoai> TheLoais { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnhHang>(entity =>
        {
            entity.HasKey(e => e.TenAnh).HasName("PK__AnhHang__302FA282B7966A74");

            entity.ToTable("AnhHang");

            entity.Property(e => e.TenAnh).HasMaxLength(256);
            entity.Property(e => e.MaChiTietHang).HasMaxLength(50);

            entity.HasOne(d => d.MaChiTietHangNavigation).WithMany(p => p.AnhHangs)
                .HasForeignKey(d => d.MaChiTietHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietHang_AnhHang");
        });

        modelBuilder.Entity<ChatLieu>(entity =>
        {
            entity.HasKey(e => e.MaChatLieu).HasName("PK__ChatLieu__453995BC684F085C");

            entity.ToTable("ChatLieu");

            entity.Property(e => e.MaChatLieu).HasMaxLength(20);
            entity.Property(e => e.TenChatLieu).HasMaxLength(50);
        });

        modelBuilder.Entity<ChiTietDdh>(entity =>
        {
            entity
                .HasKey(c => new { c.MaDdh, c.MaChiTietHang });
                entity.ToTable("ChiTietDDH", tb => tb.HasTrigger("ThanhTien"));

            entity.Property(e => e.MaChiTietHang).HasMaxLength(50);
            entity.Property(e => e.MaDdh).HasColumnName("MaDDH");
            entity.Property(e => e.ThanhTien).HasColumnType("money");

            entity.HasOne(d => d.MaChiTietHangNavigation).WithMany()
                .HasForeignKey(d => d.MaChiTietHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietHang_ChiTietDonDatHang");

            entity.HasOne(d => d.MaDdhNavigation).WithMany()
                .HasForeignKey(d => d.MaDdh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonDatHang_ChiTietDonDatHang");
        });

        modelBuilder.Entity<ChiTietHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTietHang).HasName("PK__ChiTietH__436B59F989CB25C1");

            entity.ToTable("ChiTietHang");

            entity.Property(e => e.MaChiTietHang).HasMaxLength(50);
            entity.Property(e => e.DonGiaBan).HasColumnType("money");
            entity.Property(e => e.DonGiaNhap).HasColumnType("money");
            entity.Property(e => e.MaChatLieu).HasMaxLength(20);
            entity.Property(e => e.MaHang).HasMaxLength(20);
            entity.Property(e => e.MaKichThuoc).HasMaxLength(20);
            entity.Property(e => e.MaMau).HasMaxLength(20);

            entity.HasOne(d => d.MaChatLieuNavigation).WithMany(p => p.ChiTietHangs)
                .HasForeignKey(d => d.MaChatLieu)
                .HasConstraintName("FK_ChatLieu_ChiTietHang");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.ChiTietHangs)
                .HasForeignKey(d => d.MaHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hang_ChiTietHang");

            entity.HasOne(d => d.MaKichThuocNavigation).WithMany(p => p.ChiTietHangs)
                .HasForeignKey(d => d.MaKichThuoc)
                .HasConstraintName("FK_KichThuoc_ChiTietHang");

            entity.HasOne(d => d.MaMauNavigation).WithMany(p => p.ChiTietHangs)
                .HasForeignKey(d => d.MaMau)
                .HasConstraintName("FK_MauSac_ChiTietHang");
        });

        modelBuilder.Entity<ChiTietHdn>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ChiTietHDN", tb => tb.HasTrigger("ThanhTienNhap"));

            entity.Property(e => e.MaChiTietHang).HasMaxLength(50);
            entity.Property(e => e.MaHdn).HasColumnName("MaHDN");
            entity.Property(e => e.ThanhTien).HasColumnType("money");

            entity.HasOne(d => d.MaChiTietHangNavigation).WithMany()
                .HasForeignKey(d => d.MaChiTietHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietHang_ChiTietHDN");

            entity.HasOne(d => d.MaHdnNavigation).WithMany()
                .HasForeignKey(d => d.MaHdn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDonNhap_ChiTietHDN");
        });

        modelBuilder.Entity<CongViec>(entity =>
        {
            entity.HasKey(e => e.MaCv).HasName("PK__CongViec__27258E7619B0BF3E");

            entity.ToTable("CongViec");

            entity.Property(e => e.MaCv)
                .HasMaxLength(20)
                .HasColumnName("MaCV");
            entity.Property(e => e.LuongThang).HasColumnType("money");
            entity.Property(e => e.TenCv)
                .HasMaxLength(100)
                .HasColumnName("TenCV");
        });

        modelBuilder.Entity<DanhMucHang>(entity =>
        {
            entity.HasKey(e => e.MaHang).HasName("PK__DanhMucH__19C0DB1DBDE6823B");

            entity.ToTable("DanhMucHang");

            entity.Property(e => e.MaHang).HasMaxLength(20);
            entity.Property(e => e.MaLoai).HasMaxLength(20);
            entity.Property(e => e.TenHang).HasMaxLength(300);

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.DanhMucHangs)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK_LoaiHang_Hang");
        });

        modelBuilder.Entity<DonDatHang>(entity =>
        {
            entity.HasKey(e => e.MaDdh).HasName("PK__DonDatHa__3D88C8049C17635B");

            entity.ToTable("DonDatHang");

            entity.Property(e => e.MaDdh).HasColumnName("MaDDH");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(20)
                .HasColumnName("MaNV");
            entity.Property(e => e.TongTien).HasColumnType("money");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DonDatHangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_Khach_DonDatHang");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.DonDatHangs)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK_NhanVien_DonDatHang");
        });

        modelBuilder.Entity<HoaDonNhap>(entity =>
        {
            entity.HasKey(e => e.MaHdn).HasName("PK__HoaDonNh__3C90E8C64139D1AF");

            entity.ToTable("HoaDonNhap");

            entity.Property(e => e.MaHdn).HasColumnName("MaHDN");
            entity.Property(e => e.MaNcc)
                .HasMaxLength(20)
                .HasColumnName("MaNCC");
            entity.Property(e => e.MaNv)
                .HasMaxLength(20)
                .HasColumnName("MaNV");
            entity.Property(e => e.TongTien).HasColumnType("money");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.HoaDonNhaps)
                .HasForeignKey(d => d.MaNcc)
                .HasConstraintName("FK_NhaCungCap_HoaDonNhap");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDonNhaps)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK_NhanVien_HoaDonNhap");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1E3D30FFBC");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(250);
            entity.Property(e => e.DienThoai).HasMaxLength(10);
            entity.Property(e => e.MatKhau).HasMaxLength(32);
            entity.Property(e => e.Salt)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.TenKhach).HasMaxLength(60);
        });

        modelBuilder.Entity<KichThuoc>(entity =>
        {
            entity.HasKey(e => e.MaKichThuoc).HasName("PK__KichThuo__22BFD66456823A47");

            entity.ToTable("KichThuoc");

            entity.Property(e => e.MaKichThuoc).HasMaxLength(20);
            entity.Property(e => e.TenKichThuoc).HasMaxLength(50);
        });

        modelBuilder.Entity<MauSac>(entity =>
        {
            entity.HasKey(e => e.MaMau).HasName("PK__MauSac__3A5BBB7DAEDC3D97");

            entity.ToTable("MauSac");

            entity.Property(e => e.MaMau).HasMaxLength(20);
            entity.Property(e => e.TenMau).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__NhaCungC__3A185DEB305A0978");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNcc)
                .HasMaxLength(20)
                .HasColumnName("MaNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(250);
            entity.Property(e => e.DienThoai).HasMaxLength(10);
            entity.Property(e => e.TenNcc)
                .HasMaxLength(150)
                .HasColumnName("TenNCC");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70AF767F91B");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv)
                .HasMaxLength(20)
                .HasColumnName("MaNV");
            entity.Property(e => e.DiaChi).HasMaxLength(250);
            entity.Property(e => e.DienThoai).HasMaxLength(10);
            entity.Property(e => e.MaCv)
                .HasMaxLength(20)
                .HasColumnName("MaCV");
            entity.Property(e => e.MatKhau).HasMaxLength(32);
            entity.Property(e => e.Salt)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.TenNv)
                .HasMaxLength(60)
                .HasColumnName("TenNV");

            entity.HasOne(d => d.MaCvNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaCv)
                .HasConstraintName("FK_CongViec_NhanVien");
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__TheLoai__730A57597B53BBA5");

            entity.ToTable("TheLoai");

            entity.Property(e => e.MaLoai).HasMaxLength(20);
            entity.Property(e => e.TenLoai).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<BTL_TKWeb.ModelView.AdminRegisterViewModel> AdminRegisterViewModel { get; set; } = default!;

public DbSet<BTL_TKWeb.ModelView.LoginAdminViewModel> LoginAdminViewModel { get; set; } = default!;
}
