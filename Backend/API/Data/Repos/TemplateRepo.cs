using API.FileManipulation;
using API.Models;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repos;

public class TemplateRepo(DataContext context) : ITemplateRepo
{
    private readonly DataContext _context = context;
    public async Task<Template> Save(Template template, string? userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return null;
        }
        template.CreationTime = DateTime.UtcNow;
        _context.Add(template);
        user.Templates.Add(template);
        await SaveChangesAsync();
        return template;
    }

    public async Task<List<Template>> GetAll(string? userId)
    {
        var user = await _context.Users
            .Include(u => u.Templates)
            .ThenInclude(t => t.Fields)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.Templates.ToList() ?? [];
    }

    public async Task<Template?> GetById(uint id, string? userId)
    {
        var user = await _context.Users
            .Include(u => u.Templates)
            .ThenInclude(t => t.Fields)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.Templates.FirstOrDefault(t => t.Id == id) ?? null;
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

    public async Task<List<Template>> Search(string searchQuery, string? userId)
    {
        var user = await _context.Users
            .Include(u => u.Templates)
            .ThenInclude(t => t.Fields)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var templates = user?.Templates ?? [];
        List<Template> filteredTemplates = [];
        var keywords = searchQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var template in templates)
        {
            if (template?.FileData == null)
            {
                continue;
            }

            using var stream = new MemoryStream(template.FileData);
            using var docxDocument = WordprocessingDocument.Open(stream, false);
            string docxText = docxDocument.MainDocumentPart?.Document?.Body?.InnerText ?? string.Empty;

            if (keywords.Any(keyword => docxText.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            || keywords.Any(keyword => template.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            {
                filteredTemplates.Add(template);
            }
        }
        return filteredTemplates;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
