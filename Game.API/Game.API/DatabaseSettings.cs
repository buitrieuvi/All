﻿namespace Game.API.Model
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string UserCollection { get; set; } = string.Empty;
        public string PlayerCollection { get; set; } = string.Empty;
    }
}
