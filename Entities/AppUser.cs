using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tinder_Admin.Entities.Shared;
using Tinder_Admin.Helpers.Types;

namespace Tinder_Admin.Entities
{
    [Table("m_users")]
    public class AppUser : IdentityUser
    {
        [StringLength(100)]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderType Gender { get; set; } = GenderType.Other;

        public string? PhoneNumber { get; set; }

        public string? AvatarUrl { get; set; }

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
