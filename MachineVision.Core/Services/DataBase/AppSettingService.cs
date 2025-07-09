using MachineVision.Core.Entity;
using MachineVision.Core.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MachineVision.Core.Services.DataBase
{
    public class AppSettingService:BaseService
    {
        /// <summary>
        /// 获取默认的系统配置
        /// </summary>
        /// <returns></returns>
        public async Task<AppSetting> GetSettingAsync()
        {
            return await Sql.Select<AppSetting>().FirstAsync();
        }

        /// <summary>
        /// 更新系统设置
        /// </summary>
        /// <param name="AppSettingParameter">系统参数</param>
        /// <returns></returns>
        public async Task UpdateSettingAsync(AppSettingParameter parameter)
        {
            var appSetting = await GetSettingAsync();
            if (appSetting != null)
            {
                appSetting.Content = JsonConvert.SerializeObject(parameter);
                appSetting.UpdateDate = DateTime.Now;
                await Sql.Update<AppSetting>()
                .SetDto(appSetting)
                .Where(s => s.Id == appSetting.Id)
                .ExecuteAffrowsAsync();
            }
            else
            {
                appSetting = new AppSetting();
                appSetting.Content = JsonConvert.SerializeObject(parameter);
                appSetting.CreateDate = DateTime.Now;   
                await Sql.Insert(appSetting).ExecuteIdentityAsync();
            }
        }
    }
}
