using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamViewerNet.ConsoleApp
{
    static class StateManager
    {
        const string fileName = "settings.ini";

        public static AppState Load()
        {
            try
            {
                string content = File.ReadAllText(fileName);

                if (string.IsNullOrEmpty(content))
                    return null;

                return AppState.FromJSON(content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool Save(AppState state)
        {
            try
            {
                File.WriteAllText(fileName, state.Serialize());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
