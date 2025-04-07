using Bogus;

namespace WebApplication1.Data
{
    

    
        public class DataBaseManager
        {
            public void AddNews(AppAlionaContext context)
            {
                var faker = new Faker<News>("uk")
                    .RuleFor(b => b.title, f => f.Lorem.Sentence(5, 3))
                    .RuleFor(b => b.slug, (f, b) => f.Internet.DomainName())
                    .RuleFor(b => b.summary, f =>  f.Lorem.Sentence(10, 5))
                    .RuleFor(b => b.content, f =>  f.Lorem.Paragraphs(1))
                    .RuleFor(b => b.Image, f => f.Image.PicsumUrl());

                for (int i = 0; i < 20; i++)
                {
                    var b = faker.Generate(1);
                    context.Add(b[0]);
                    context.SaveChanges();
                }
            }
        }
    
}
