using DinnerSpinner.Api.Data;

namespace DinnerSpinner.Api.Features.Categories.EditCategory
{
    public static class EditCategory
    {
        public record Command(int Id, string Name);

        public class Handler
        {
            private readonly AppDbContext context;

            public Handler(AppDbContext context)
            {
                this.context = context;
            }

            public async Task Handle(Command command, CancellationToken cancellationToken)
            {
                var category = await context.Categories.FindAsync(
                    command.Id, cancellationToken)
                    ??
                    throw new KeyNotFoundException($"Category with ID {command.Id} not found.");

                category.Name = command.Name;

                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
