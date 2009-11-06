using System.Data.Linq.Mapping;

namespace LinqFu.Tests.Data
{
    public interface IProduct
    {
        [Column(Storage = "_ProductID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        int ProductID { get; set; }

        [Column(Storage = "_Name", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        string Name { get; set; }

        [Column(Storage = "_ProductNumber", DbType = "NVarChar(25) NOT NULL", CanBeNull = false)]
        string ProductNumber { get; set; }

        [Column(Storage = "_MakeFlag", DbType = "Bit NOT NULL")]
        bool MakeFlag { get; set; }

        [Column(Storage = "_FinishedGoodsFlag", DbType = "Bit NOT NULL")]
        bool FinishedGoodsFlag { get; set; }

        [Column(Storage = "_Color", DbType = "NVarChar(15)")]
        string Color { get; set; }
    }
}