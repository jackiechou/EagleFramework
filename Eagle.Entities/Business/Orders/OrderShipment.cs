using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Orders
{
    [Table("Sales.OrderShipment")]
    public class OrderShipment : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderShipmentId { get; set; }
        public Guid OrderNo { get; set; }
        public int? CustomerId { get; set; }
        public int ShippingMethodId { get; set; }
        public DateTime? ShipDate { get; set; }
        public decimal? Weight { get; set; }
        public bool? IsInternational { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhone { get; set; }
        
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
        public int? RegionId { get; set; }
        public string PostalCode { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedDate { get; set; }
    }
}
