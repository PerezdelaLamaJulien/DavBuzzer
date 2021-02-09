namespace DavBuzzer
{
    public static class PropertyUtils
    {
        public static async void SavesProperty(string key, string data)
        {
            if (App.Current.Properties.ContainsKey(key))
            {
                App.Current.Properties.Remove(key);
            }
            App.Current.Properties.Add(key, data);
            await App.Current.SavePropertiesAsync();
        }
    }
}
