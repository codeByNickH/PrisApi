using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; }
        [Required, MaxLength(150)]
        public string Website { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string NavMeat { get; set; }
        public string NavChark { get; set; } = null;
        public string NavDairy { get; set; }
        public string NavCheese { get; set; } = null;
        public string NavFruitAndVegetables { get; set; }
        public string NavFishAndSeafood { get; set; }
        public string NavVegetarian { get; set; }
        public string NavPantry { get; set; }
        public string NavFrozen { get; set; }
        public string NavBreadAndCookies { get; set; }
        public string NavIceCreamCandyAndSnacks { get; set; }
        public string NavSnacks { get; set; }
        public string NavBeverage { get; set; }
        public string NavReadyMeals { get; set; }
        public string NavKids { get; set; }
        public string NavHomeAndCleaning { get; set; }
        public string NavHealth { get; set; }
        public string NavHygien { get; set; }
        public string NavPharmacy { get; set; }
        public string NavAnimals { get; set; }
        public string NavTobacco { get; set; }
    }
}