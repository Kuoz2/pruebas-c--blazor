using Microsoft.EntityFrameworkCore;
using puntodeventa.Data;
using puntodeventa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace puntodeventa.Services
{
    public class ProductosService
    {
        private readonly ApplicationDbContext _context;
        public ProductosService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }
        public async Task<List<Productos>> GetProductosAsync()
        {
            return await _context.polls_productos.ToListAsync();
        }
        public async Task<Productos> GetProductosByIdAync(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.polls_productos.FindAsync(id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
        public async Task AddProductos(Productos productos)
        {
            if (productos == null)
            {
                throw new ArgumentNullException(nameof(productos));
            }
            _context.polls_productos.Add(productos);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProductosAsync(Productos productos)
        {
            if (productos == null)
            {
                throw new ArgumentNullException(nameof(productos));
            }
            _context.Entry(productos).State = EntityState.Modified;
            await _context.SaveChangesAsync(); 
        }
        public async Task DeleteProductosAsync(int id)
        {
            var producto = await _context.polls_productos.FindAsync(id);
            if (producto != null)
            {
                _context.polls_productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
