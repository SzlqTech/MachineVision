using MachineVision.Core.Entity;
using MachineVision.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MachineVision.Core.Services.DataBase
{
    public class ProductService:BaseService
    {
        public async Task<bool> CreateAsync(ProductModel input)
        {
            var model = mapper.Map<Product>(input);
            return await Sql.Insert(model).ExecuteAffrowsAsync() > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await Sql.Delete<Product>(new { id = id }).ExecuteAffrowsAsync() > 0;
        }

        public async Task<List<ProductModel>> GetListAsync(string filterText)
        {
            var models = await Sql.Select<Product>().Where(q => q.Name.Contains(filterText)).ToListAsync();
            return mapper.Map<List<ProductModel>>(models);
        }

        public async Task<ProductModel> GetProductByIdAsync(long id)
        {
            var model = await Sql.Select<Product>().Where(q => q.Id == id).FirstAsync();
            if (model == null) return default;
            var productModel = mapper.Map<ProductModel>(model);
            return productModel;
        }

        public async Task<bool> UpdateAsync(ProductModel input)
        {
            var model = mapper.Map<Product>(input);
            return await Sql.Update<Product>()
                        .SetDto(model)
                        .Where(q => q.Id == input.Id)
                        .ExecuteAffrowsAsync() > 0;
        }

        public async Task<bool> InsertOrUpdateBatchAsync(List<ProductModel> inputs)
        {
            var models = mapper.Map<List<ProductModel>>(inputs);
            return await Sql.InsertOrUpdate<ProductModel>()
                            .IfExistsDoNothing()//如果数据存在，啥事也不干（相当于只有不存在数据时才插入）
                            .SetSource(models)
                            .ExecuteAffrowsAsync()>0;
        }
    }
}
