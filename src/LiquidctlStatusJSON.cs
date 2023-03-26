using System.Globalization;

namespace FanControl.Liquidctl
{
    public class LiquidctlStatusJSON
    {
        public class StatusRecord
        {
            public string? Key { get; set; }
            public string? Value { get; set; }
            public string? Unit { get; set; }

            public float? GetValueAsFloat()
            {
                if (float.TryParse(Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out float valueAsFloat))
                    return valueAsFloat;
                return null;
            }
        }
        public string Bus { get; set; } = "";
        public string? Port { get; set; }
        public string Address { get; set; } = "";

        public string GetAddress()
        {
            if (Bus.StartsWith("usb"))
                return $"usb#{Port}";
            return Address;
        }

        public static KeyValuePair<string, string> GetBusAndAddress(string address)
        {
            if (address.StartsWith("usb#"))
                return new KeyValuePair<string, string>("usb", address.Split('#')[1]);
            return new KeyValuePair<string, string>("hid", address);
        }

        public string? Description { get; set; }

        public List<StatusRecord> Status { get; set; } = [];
    }
}
