using Microsoft.EntityFrameworkCore;
using NeofiliaDomain;
using NeofiliaDomain.Application.Common.Repositories;

namespace NeofiliaApi.Data.Repository;

public class TableRepository : ITableRepository
{
    private readonly ApplicationDbContext _context;
    public TableRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<PubTable>> Get()
    {
        var tables = await _context.Tables.ToListAsync();
        return tables ?? [];
    } 
    public async Task Delete(int id)
    {
        var row = _context.Tables
            .Where(table => table.Id == id).ExecuteDelete();
        await _context.SaveChangesAsync();
    }
    public async Task<PubTable?> GetById(int id)
    {
        var table = await _context.Tables
                .Include(t => t.CurrentTableReward)
                .FirstOrDefaultAsync(t => t.Id == id);

        return table;
    }

    public async Task Update(PubTable table)
    {
        _context.Tables.Update(table);
        await _context.SaveChangesAsync();
    }
}
