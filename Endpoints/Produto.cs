using megev.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

namespace megev.Endpoints
{
    public static class ProdutoEndpoints
    {
        public static void RegistrarEndpointsProdutos(this IEndpointRouteBuilder rotas)
        {
            var rotaProdutos = rotas.MapGroup("/produtos");

            // Endpoint para Listar Produtos com Paginação
            rotaProdutos.MapGet("/", async (MegevDbContext dbContext, HttpContext httpContext, int page = 1, int limit = 12) =>
            {
                if (page < 1 || limit < 1)
                {
                    return Results.BadRequest("O número da página e o limite devem ser maiores que zero.");
                }

                var usuarioId = int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var totalProdutos = await dbContext.Produto.CountAsync(p => p.UsuarioId == usuarioId);
                var totalPages = (int)Math.Ceiling(totalProdutos / (double)limit);

                if (page > totalPages)
                {
                    return Results.BadRequest("Número da página maior que o total de páginas disponível.");
                }

                var produtos = await dbContext.Produto
                    .Where(p => p.UsuarioId == usuarioId) // Filtra produtos pelo ID do usuário autenticado
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .Include(p => p.Usuario) // Inclui dados do usuário associado
                    .Select(p => new ProdutoDto
                    {
                        Id = p.Id,
                        Referencia = p.Referencia,
                        Descricao = p.Descricao,
                        Categoria = p.Categoria,
                        Preco = p.Preco,
                        Status = p.Status,
                        Image = p.Image,
                        Usuario = new UsuarioDto
                        {
                            Id = p.Usuario.Id,
                            Nome = p.Usuario.Nome,
                            Sobrenome = p.Usuario.Sobrenome,
                            Email = p.Usuario.Email
                        }
                    })
                    .ToListAsync();

                var paginatedResult = new
                {
                    Total = totalProdutos,
                    PageSize = limit,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    Produtos = produtos
                };

                return Results.Ok(paginatedResult);
            });

            // Endpoint para Obter Produto por ID
            rotaProdutos.MapGet("/{id}", async (MegevDbContext dbContext, HttpContext httpContext, int id) =>
            {
                var usuarioId = int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var produto = await dbContext.Produto
                    .Include(p => p.Usuario) // Inclui dados do usuário associado
                    .Where(p => p.Id == id && p.UsuarioId == usuarioId) // Verifica se o produto pertence ao usuário autenticado
                    .Select(p => new ProdutoDto
                    {
                        Id = p.Id,
                        Referencia = p.Referencia,
                        Descricao = p.Descricao,
                        Categoria = p.Categoria,
                        Preco = p.Preco,
                        Status = p.Status,
                        Image = p.Image,
                        Usuario = new UsuarioDto
                        {
                            Id = p.Usuario.Id,
                            Nome = p.Usuario.Nome,
                            Sobrenome = p.Usuario.Sobrenome,
                            Email = p.Usuario.Email
                        }
                    })
                    .SingleOrDefaultAsync();

                if (produto == null)
                {
                    return Results.NotFound("Produto não encontrado ou não pertence ao usuário.");
                }

                return Results.Ok(produto);
            }).Produces<ProdutoDto>();


            // Endpoint para Adicionar um Novo Produto
            rotaProdutos.MapPost("/", async (MegevDbContext dbContext, Produto produto, HttpContext httpContext) =>
            {
                var usuarioId = int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                produto.UsuarioId = usuarioId;

                dbContext.Produto.Add(produto);
                await dbContext.SaveChangesAsync();

                return Results.Created($"/produtos/{produto.Id}", produto);
            }).RequireAuthorization();


            // Endpoint para Atualizar um Produto
            rotaProdutos.MapPut("/{id}", async (int id, MegevDbContext dbContext, Produto produto, HttpContext httpContext) =>
            {
                var usuarioId = int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var produtoExistente = await dbContext.Produto.SingleOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);
                if (produtoExistente == null)
                {
                    return Results.NotFound("Produto não encontrado ou não pertence ao usuário.");
                }

                // Atualiza apenas os campos que podem ser modificados
                produtoExistente.Referencia = produto.Referencia;
                produtoExistente.Descricao = produto.Descricao;
                produtoExistente.Categoria = produto.Categoria;
                produtoExistente.Preco = produto.Preco;
                produtoExistente.Status = produto.Status;
                produtoExistente.Image = produto.Image;

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }).RequireAuthorization();


            // Endpoint para Deletar um Produto
            rotaProdutos.MapDelete("/{id}", async (int id, MegevDbContext dbContext) =>
            {
                var produtoExistente = await dbContext.Produto.SingleOrDefaultAsync(p => p.Id == id);
                if (produtoExistente == null)
                {
                    return Results.NotFound("Produto não encontrado.");
                }

                dbContext.Produto.Remove(produtoExistente);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }).RequireAuthorization();
        }
    }
}
