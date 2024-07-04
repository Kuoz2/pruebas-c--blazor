using Microsoft.EntityFrameworkCore;
using puntodeventa.Data;
using puntodeventa.Models;

namespace puntodeventa.Services
{
    public class VentasService
    {
        private readonly ApplicationDbContext _context;

        public VentasService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Ventas>> GetVentasAsync()
        {
            return await _context.polls_ventas.Include(v => v.Productos).ToListAsync();
        }
        public async Task<Ventas> GetVentaByIdAsync(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.polls_ventas.Include(v => v.Productos)
                .FirstOrDefaultAsync(v => v.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
        public async Task<Ventas> CreateVentasAsync(Ventas ventas)
        {
            _context.polls_ventas.Add(ventas);
            await _context.SaveChangesAsync();
            return ventas;
        }
        public async Task UpdateVentasAsync(Ventas ventas)
        {
            _context.Entry(ventas).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DelteVentasAsync(int id)
        {
            var venta = await _context.polls_ventas.FindAsync(id);
            if (venta != null)
            {
                _context.polls_ventas.Remove(venta);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<List<Ventas>> GetVentasDiarias()
        {
            DateTime fechaActual = DateTime.Now;
            string fechaFormateada = fechaActual.ToString("dd/m/yyyy");
            Console.WriteLine(await _context.polls_ventas.Include(v => v.Productos).Where(v => v.Fechav == fechaFormateada).ToListAsync());
            return await _context.polls_ventas.Include(v => v.Productos).Where(v => v.Fechav == fechaFormateada).ToListAsync();

        }
    }
}
