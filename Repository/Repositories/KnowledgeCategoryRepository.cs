using Repository.Entites;
using Repository.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class KnowledgeCategoryRepository : Irepository<KnowledgeCategory>
    {
        private readonly Icontext _context;

        public KnowledgeCategoryRepository(Icontext context)
        {
            _context = context;
        }

        public async Task<KnowledgeCategory> AddItem(KnowledgeCategory item)
        {
            await _context.KnowledgeCategories.AddAsync(item);
            return item;
        }

        public async Task<KnowledgeCategory> DeleteItem(int id)
        {
            var entity = await Getbyid(id);
            if (entity == null) return null;

            _context.KnowledgeCategories.Remove(entity);
            return entity;
        }

        public async Task<List<KnowledgeCategory>> GetAll()
        {
            return await _context.KnowledgeCategories.ToListAsync();
        }

        public async Task<KnowledgeCategory> Getbyid(int id)
        {
            return await _context.KnowledgeCategories.FirstOrDefaultAsync(k => k.ID_knowledge == id);
        }

        public async Task<KnowledgeCategory> UpDateItem(int id, KnowledgeCategory item)
        {
            var existing = await Getbyid(id);
            if (existing == null) return null;

            existing.describtion = item.describtion;
            return existing;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

    }
}
