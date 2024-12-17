using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.InventoryMgt
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;



	[Table("tblIMReceiptsTransactionHeader")]
	public class TblIMReceiptsTransactionHeader : AutoActiveGenerateIdAuditableKey<int>
	{
		[StringLength(20)]
		[Key]
		public string TranNumber { get; set; }

		[Column(TypeName = "date")]
		public DateTime TranDate { get; set; }

		[StringLength(50)]
		public string TranUser { get; set; }

		[StringLength(10)]
		public string TranLocation { get; set; }

		[StringLength(10)]
		public string TranToLocation { get; set; }


		[StringLength(50)]
		public string TranDocNumber { get; set; }

		[StringLength(50)]
		public string TranReference { get; set; }

		[StringLength(10)]
		public string TranType { get; set; }

		[Column(TypeName = "decimal(10,3)")]
		public decimal TranTotalCost { get; set; }


		public int TranTotItems { get; set; }

		[Column(TypeName = "date")]
		public DateTime TranCreateDate { get; set; }

		[Column(TypeName = "nvarchar(25)")]
		public string TranCreateUser { get; set; }

		[Column(TypeName = "date")]
		public DateTime TranLastEditDate { get; set; }

		[Column(TypeName = "nvarchar(25)")]
		public string TranLastEditUser { get; set; }

		public sbyte TranLockStat { get; set; }

		[Column(TypeName = "nvarchar(25)")]
		public string TranLockUser { get; set; }

		public sbyte TranPostStatus { get; set; }

		[Column(TypeName = "date")]
		public DateTime TranPostDate { get; set; }

		[Column(TypeName = "nvarchar(25)")]
		public string TranpostUser { get; set; }

		public sbyte TranVoidStatus { get; set; }

		[Column(TypeName = "nvarchar(25)")]
		public string TranVoidUser { get; set; }


		[Column(TypeName = "date")]
		public DateTime TranvoidDate { get; set; }

		[StringLength(250)]
		public string TranRemarks { get; set; }

		[StringLength(50)]
		public string TranInvAccount { get; set; }

		[StringLength(50)]
		public string TranInvAdjAccount { get; set; }

		[StringLength(15)]
		public string JVNum { get; set; }

		[StringLength(10)]
		public string TranBranch { get; set; }

		public bool IsPaid { get; set; }
		public Boolean IsApproved { get; set; }
	}
}
