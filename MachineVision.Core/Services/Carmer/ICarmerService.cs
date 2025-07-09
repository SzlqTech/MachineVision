

namespace MachineVision.Core.Services.Carmer
{
    public interface ICarmerService
    {
        /// <summary>
        /// 设置参数
        /// </summary>
        void SetParam(string exposure, string gain, string frameRate);

        /// <summary>
        /// 获取参数
        /// </summary>
        void GetParam();

        /// <summary>
        /// 软件触发执行
        /// </summary>
        void TriggerExec();

        /// <summary>
        /// 停止采集
        /// </summary>
        void StopGrab();

        /// <summary>
        /// 开始采集
        /// </summary>
        void StartGrab();

        /// <summary>
        /// 关闭设备
        /// </summary>
        void CloseDevice();

        /// <summary>
        /// 打开设备
        /// </summary>
        void OpenDevice();

        /// <summary>
        /// 查询设备
        /// </summary>
        void FindDevice();
        
    }
}
