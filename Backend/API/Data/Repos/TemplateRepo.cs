using API.FileManipulation;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repos;

public class TemplateRepo(DataContext context) : ITemplateRepo
{
    private readonly DataContext _context = context;
    public async Task<Template> Save(Template template)
    {
        _context.Add(template);
        await SaveChangesAsync();
        return template;
    }

    public async Task<List<Template>> GetAll()
    {
        return await _context.Templates
        .Include(t => t.Fields)
        .ToListAsync();
    }

    public async Task<Template?> GetById(uint id)
    {
        return await _context.Templates
        .Include(t => t.Fields)
        .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task Delete(Template template)
    {
        _context.Remove(template);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
