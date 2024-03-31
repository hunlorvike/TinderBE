using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tinder_Admin.Entities.Shared
{
    public abstract class _EntityBase
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("register_date")]
        public DateTime RegisteredAt { get; set; }

        [Column("registrant_id")]
        public int RegistrantId { get; set; }

        [Column("update_date")]
        public DateTime UpdatedAt { get; set; }

        [Column("updater_id")]
        public int UpdaterId { get; set; }

        [Column("delete_date")]
        public DateTime DeletedAt { get; set; }

        [Column("deleter_id")]
        public int DeleterId { get; set; }
    }
}
