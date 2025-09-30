using MySqlConnector;

namespace HomeTechBlazor.Service
{
    public static class SafeGetHelper
    {
        public static string SafeGetString(this MySqlDataReader reader, string column, string defaultValue = "Chưa cập nhật")
        {
            int index = reader.GetOrdinal(column);
            return reader.IsDBNull(index) ? defaultValue : reader.GetString(index);
        }

        public static int SafeGetInt(this MySqlDataReader reader, string column, int defaultValue = 0)
        {
            int index = reader.GetOrdinal(column);
            return reader.IsDBNull(index) ? defaultValue : reader.GetInt32(index);
        }

        public static short SafeGetInt16(this MySqlDataReader reader, string column, short defaultValue = 0)
        {
            int index = reader.GetOrdinal(column);
            return reader.IsDBNull(index) ? defaultValue : reader.GetInt16(index);
        }

        public static DateTime? SafeGetDateTime(this MySqlDataReader reader, string column, DateTime? defaultValue = null)
        {
            int index = reader.GetOrdinal(column);
            return reader.IsDBNull(index) ? defaultValue : reader.GetDateTime(index);
        }
    }
}
