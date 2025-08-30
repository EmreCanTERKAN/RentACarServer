using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Categories;
[Permission("category:delete")]
public sealed record CategoryDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class CategoryDeleteCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CategoryDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);

        if (category is null)
        {
            return Result<string>.Failure("Bu id'ye ait kategori bulunamadı");
        }

        category.Delete();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kategori başarıyla silindi";

    }
}
