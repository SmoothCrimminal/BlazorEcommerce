

namespace BlazorEcommerce.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContext) : base(dbContext)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    Title = "Naruto Shippuden: Ultimate Ninja Storm 3",
                    Description = "Naruto Shippuden: Ultimate Ninja Storm 3, known in Japan as Naruto Shippūden: Narutimate Storm 3 (Japanese: NARUTOナルト 疾風伝 ナルティメットストーム3, Hepburn: Naruto Shippūden: Narutimetto Sutōmu 3), the fourth installment of the UltimateStorm series, is a fighting game developed by CyberConnect2 as part of the Naruto: Ultimate Ninja video-game series based on Masashi Kishimoto's Naruto manga. It was first released for PlayStation 3 and Xbox 360 by Namco Bandai Games on March 2013 in North America and in Europe, and on April 2013 in Japan.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/5/57/Naruto_Shippuden_UNS_3_box_art.png",
                    Price = 9.99m
                },
                new Product()
                {
                    Id = 2,
                    Title = "Grand Theft Auto V",
                    Description = "Grand Theft Auto V (GTA V) – komputerowa gra akcji, należąca do serii Grand Theft Auto. Została wydana na konsole PlayStation 3 i Xbox 360 17 września 2013 przez studio Rockstar Games. Akcja gry została umiejscowiona w fikcyjnym mieście Los Santos oraz w terenach pozamiejskich nazwanych Blaine County w stanie San Andreas[16], stworzonych na podstawie miasta Los Angeles i Kalifornii. 10 czerwca 2014 podczas targów E3 ujawniono zapowiedź gry w wersjach na komputery osobiste oraz konsole ósmej generacji – PlayStation 4 i Xbox One[17]. Wersje konsolowe ukazały się 18 listopada 2014[11], a na komputery osobiste 14 kwietnia 2015[12]. W czerwcu 2020 zapowiedziano wydanie gry na konsole dziewiątej generacji – PlayStation 5 oraz Xbox Series X.",
                    ImageUrl = "https://m.media-amazon.com/images/I/91T0XQv8gEL._SL1500_.jpg",
                    Price = 7.99m
                },
                new Product()
                {
                    Id = 3,
                    Title = "Rok 1984",
                    Description = "Rok 1984 (tytuł oryginalny: Nineteen Eighty-Four) – futurystyczna dystopia o licznych podtekstach politycznych, napisana przez George’a Orwella i opublikowana w roku 1949. Autor napisał ją pod wpływem kontaktu z praktyczną stroną systemu stalinowskiego, do jakiego po raz pierwszy doszło w Hiszpanii w 1936 roku (w czasie wojny domowej), gdzie pojechał jako dziennikarz i sympatyk strony republikańskiej. Wspomnienia niedalekiej przeszłości pozwoliły wykreować mu antyutopijny porządek świata w fikcyjnej przyszłości. Książka była ostatnim dziełem George’a Orwella. Pracę nad nią rozpoczął w 1948 roku, kiedy jego zdrowie uległo znacznemu pogorszeniu (cierpiał na zaawansowaną formę gruźlicy)[1].",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c3/1984first.jpg/800px-1984first.jpg",
                    Price = 6.99m
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=ecommerce;Username=postgres;Password=qwerty");
            }
        }

        public DbSet<Product> Products { get; set; }
    }
}
