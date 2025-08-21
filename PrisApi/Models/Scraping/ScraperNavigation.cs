using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisApi.Models.Scraping
{
    public class ScraperNavigation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string NavMeat { get; set; }
        [MaxLength(100)]
        public string NavChark { get; set; } = null;
        [MaxLength(100)]
        public string NavDairy { get; set; }
        [MaxLength(100)]
        public string NavCheese { get; set; } = null;
        [MaxLength(100)]
        public string NavFruitAndVegetables { get; set; }
        [MaxLength(100)]
        public string NavFishAndSeafood { get; set; }
        [MaxLength(100)]
        public string NavVegetarian { get; set; }
        [MaxLength(100)]
        public string NavPantry { get; set; }
        [MaxLength(100)]
        public string NavFrozen { get; set; }
        [MaxLength(100)]
        public string NavBreadAndCookies { get; set; }
        [MaxLength(100)]
        public string NavIceCreamCandyAndSnacks { get; set; }
        [MaxLength(100)]
        public string NavSnacks { get; set; }
        [MaxLength(100)]
        public string NavBeverage { get; set; }
        [MaxLength(100)]
        public string NavReadyMeals { get; set; }
        [MaxLength(100)]
        public string NavKids { get; set; }
        [MaxLength(100)]
        public string NavHomeAndCleaning { get; set; }
        [MaxLength(100)]
        public string NavHealth { get; set; }
        [MaxLength(100)]
        public string NavHygien { get; set; }
        [MaxLength(100)]
        public string NavPharmacy { get; set; }
        [MaxLength(100)]
        public string NavAnimals { get; set; }
        [MaxLength(100)]
        public string NavTobacco { get; set; }
    }
}