using Bogus;

namespace WebApplication1.Data
{
    

    
        public class DataBaseManager
        {
        public void AddBanans(AppAlionaContext context)
        {
            var faker = new Faker<Banan>("uk")
                .RuleFor(b => b.FirstName, f => f.Name.FirstName())
                .RuleFor(b => b.LastName, f => f.Name.LastName())
                .RuleFor(b => b.Image, f => f.Internet.Avatar())
                .RuleFor(b => b.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(b => b.Sex, f => f.Random.Bool());

            for (int i = 0; i < 20; i++)
            {
                var b = faker.Generate(1);
                context.Add(b[0]);
                context.SaveChanges();
            }
        }
    }
    
}
