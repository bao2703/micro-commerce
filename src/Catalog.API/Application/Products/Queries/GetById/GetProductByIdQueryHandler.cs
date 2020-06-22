﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Application.Products.Models;
using Catalog.API.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.MediatR.Exceptions;
using UnitOfWork;

namespace Catalog.API.Application.Products.Queries.GetById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IRepository<Product> _repository;

        public GetProductByIdQueryHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.Query()
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUri = p.ImageUri,
                    Description = p.Description,
                    Categories = p.Categories.Select(s => new CategoryDto
                    {
                        Id = s.CategoryId,
                        Name = s.Category.Name
                    }).ToList()
                })
                .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            return product;
        }
    }
}
