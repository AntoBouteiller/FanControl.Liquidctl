using FanControl.Plugins;

namespace FanControl.Liquidctl
{
    public class LiquidctlPlugin(IPluginLogger pluginLogger) : IPlugin2
    {
        internal List<LiquidctlDevice> devices = [];
        internal IPluginLogger logger = pluginLogger;

        public string Name => "LiquidctlPlugin";

        public void Initialize()
        {

            LiquidctlCLIWrapper.Initialize();
        }

        public void Load(IPluginSensorsContainer _container)
        {
            List<LiquidctlStatusJSON> input = LiquidctlCLIWrapper.ReadStatus() ?? [];
            foreach (LiquidctlStatusJSON liquidctl in input)
            {
                LiquidctlDevice device = new(liquidctl, logger);
                logger.Log(device.GetDeviceInfo());
                if (device.hasPumpSpeed)
                    _container.FanSensors.Add(device.pumpSpeed);
                if (device.hasPumpDuty)
                    _container.ControlSensors.Add(device.pumpDuty);
                if (device.hasLiquidTemperature)
                    _container.TempSensors.Add(device.liquidTemperature);
                if (device.hasFanSpeed)
                {
                    _container.FanSensors.Add(device.fanSpeed);
                    _container.ControlSensors.Add(device.fanControl);
                }
                for (int i = 0; i < 3; i++)
                {
                    if (device.hasMultipleFanSpeed[i])
                    {
                        _container.FanSensors.Add(device.fanSpeedMultiple[i]);
                        _container.ControlSensors.Add(device.fanControlMultiple[i]);
                    }
                }
                devices.Add(device);
            }
        }

        public void Close()
        {
            devices.Clear();
        }
        public void Update()
        {
            foreach (LiquidctlDevice device in devices)
            {
                device.LoadJSON();
            }
        }
    }
}
