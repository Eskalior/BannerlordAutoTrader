using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AutoTrader.Warsails
{
    /// <summary>
    /// Detects at runtime whether the Warsails DLC (NavalDLC) is available.
    /// This allows the mod to automatically enable fleet capacity features when the DLC is present.
    /// </summary>
    public static class WarsailsDetector
    {
        private static bool? _isWarsailsDLCAvailable = null;
        private static Assembly _warsailsDLCAssembly = null;

        /// <summary>
        /// Checks if the NavalDLC.dll file exists and can be loaded.
        /// </summary>
        public static bool IsWarsailsDLCAvailable()
        {
            // We cache the result to avoid repeated checks
            if (_isWarsailsDLCAvailable.HasValue)
            {
                return _isWarsailsDLCAvailable.Value;
            }

            try
            {
                // First check if NavalDLC is already loaded
                var navalDLCAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => assembly.GetName().Name == "NavalDLC")
                    .FirstOrDefault();

                if (navalDLCAssembly != null)
                {
                    _warsailsDLCAssembly = navalDLCAssembly;
                    _isWarsailsDLCAvailable = true;
                    AutoTraderHelpers.PrintDebugMessage("WarsailsDetector: Warsails DLC (NavalDLC) assembly already loaded");
                    return true;
                }

                // Try to find NavalDLC.dll file
                // Get the Bannerlord modules path from the game's base directory
                string gameDir = AppDomain.CurrentDomain.BaseDirectory;
                string navalDLCPath = Path.Combine(gameDir, "..", "..", "Modules", "NavalDLC", "bin", "Win64_Shipping_Client", "NavalDLC.dll");
                navalDLCPath = Path.GetFullPath(navalDLCPath);

                if (File.Exists(navalDLCPath))
                {
                    AutoTraderHelpers.PrintDebugMessage($"WarsailsDetector: Warsails DLC (NavalDLC.dll) found at {navalDLCPath}");
                    
                    // Try to load the assembly
                    try
                    {
                        _warsailsDLCAssembly = Assembly.LoadFrom(navalDLCPath);
                        _isWarsailsDLCAvailable = true;
                        AutoTraderHelpers.PrintDebugMessage("WarsailsDetector: Warsails DLC assembly loaded successfully");
                        return true;
                    }
                    catch (Exception loadEx)
                    {
                        AutoTraderHelpers.PrintDebugMessage($"WarsailsDetector: Failed to load Warsails DLC assembly: {loadEx.Message}");
                        _isWarsailsDLCAvailable = false;
                        return false;
                    }
                }
                else
                {
                    AutoTraderHelpers.PrintDebugMessage($"WarsailsDetector: Warsails DLC (NavalDLC.dll) not found at {navalDLCPath}");
                    _isWarsailsDLCAvailable = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                AutoTraderHelpers.PrintDebugMessage($"WarsailsDetector: Error checking Warsails DLC: {ex.Message}");
                _isWarsailsDLCAvailable = false;
                return false;
            }
        }

        /// <summary>
        /// Gets the loaded Warsails DLC assembly if available.
        /// </summary>
        public static Assembly GetWarsailsDLCAssembly()
        {
            if (_warsailsDLCAssembly == null)
            {
                // This will try to load it
                IsWarsailsDLCAvailable(); 
            }
            return _warsailsDLCAssembly;
        }
    }
}
