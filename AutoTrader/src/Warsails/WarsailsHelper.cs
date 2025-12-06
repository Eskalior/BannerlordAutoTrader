using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;

namespace AutoTrader.Warsails
{
    /// <summary>
    /// Helper class to interact with Warsails DLC features using reflection.
    /// Dynamically loads and calls Warsails DLC methods when the DLC is available.
    /// </summary>
    public static class WarsailsHelper
    {
        /// <summary>
        /// Gets the NavalDLCInventoryCapacityModel instance if available and active.
        /// </summary>
        /// <param name="modelType">Output parameter for the model type</param>
        /// <returns>The current inventory capacity model instance, or null if not available</returns>
        private static object GetNavalDLCModel(out Type modelType)
        {
            modelType = null;

            if (!WarsailsDetector.IsWarsailsDLCAvailable())
            {
                AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: Warsails DLC not available");
                return null;
            }

            // Get the Warsails DLC assembly
            var warsailsAssembly = WarsailsDetector.GetWarsailsDLCAssembly();
            if (warsailsAssembly == null)
            {
                AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: Failed to get Warsails DLC assembly");
                return null;
            }

            // Get the NavalDLCInventoryCapacityModel type
            modelType = warsailsAssembly.GetType("NavalDLC.GameComponents.NavalDLCInventoryCapacityModel");
            if (modelType == null)
            {
                AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: NavalDLCInventoryCapacityModel type not found");
                return null;
            }

            // Get the current inventory capacity model from Campaign
            var currentModel = Campaign.Current?.Models?.InventoryCapacityModel;
            if (currentModel == null)
            {
                AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: Campaign.Current.Models.InventoryCapacityModel is null");
                return null;
            }

            // Check if the current model is a NavalDLCInventoryCapacityModel
            // NOTE: E.g. if you have Warsails installed but not enabled, the model will not be of the correct type
            if (!modelType.IsInstanceOfType(currentModel))
            {
                AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: Current model is not NavalDLCInventoryCapacityModel");
                return null;
            }

            return currentModel;
        }

        /// <summary>
        /// Calculates the fleet cargo capacity for the given mobile party.
        /// Uses reflection to call NavalDLC.GameComponents.NavalDLCInventoryCapacityModel.CalculateInventoryCapacity().
        /// </summary>
        /// <param name="mobileParty">The mobile party to calculate capacity for</param>
        /// <returns>Fleet cargo capacity, or 0 if Warsails DLC is not available</returns>
        public static int GetFleetCargoCapacity(MobileParty mobileParty)
        {
            try
            {
                // NOTE: It follows some very defensive programming to really make sure this doesn't break anything
                Type modelType;
                var currentModel = GetNavalDLCModel(out modelType);
                if (currentModel == null)
                {
                    return 0;
                }

                // Get the CalculateInventoryCapacity method
                var calculateMethod = modelType.GetMethod("CalculateInventoryCapacity", 
                    BindingFlags.Public | BindingFlags.Instance);
                
                if (calculateMethod == null)
                {
                    AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: CalculateInventoryCapacity method not found");
                    return 0;
                }

                // Call the method with default values (second arg is specifying that we want the value for fleets)
                var parameters = new object[] { mobileParty, true, false, 0, 0, 0, false };
                var result = calculateMethod.Invoke(currentModel, parameters);
                
                if (result == null)
                {
                    AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: CalculateInventoryCapacity returned null");
                    return 0;
                }

                // Get the ResultNumber property from the result
                var resultNumberProperty = result.GetType().GetProperty("ResultNumber");
                if (resultNumberProperty == null)
                {
                    AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: ResultNumber property not found");
                    return 0;
                }

                var resultNumber = resultNumberProperty.GetValue(result);
                int capacity = (int)Convert.ToSingle(resultNumber);
                
                AutoTraderHelpers.PrintDebugMessage($"WarsailsHelper: Fleet cargo capacity = {capacity}");
                return capacity;
            }
            catch (Exception ex)
            {
                AutoTraderHelpers.PrintDebugMessage($"WarsailsHelper: Error getting fleet capacity: {ex.Message}");
                AutoTraderHelpers.PrintDebugMessage($"WarsailsHelper: Stack trace: {ex.StackTrace}");
                return 0;
            }
        }

        /// <summary>
        /// Calculates the total weight carried by the given mobile party including fleet cargo.
        /// Uses reflection to call NavalDLC.GameComponents.NavalDLCInventoryCapacityModel.CalculateTotalWeightCarried().
        /// </summary>
        /// <param name="mobileParty">The mobile party to calculate weight for</param>
        /// <returns>Total weight carried including fleet cargo, or 0 if Warsails DLC is not available</returns>
        public static float GetFleetTotalWeightCarried(MobileParty mobileParty)
        {
            try
            {
                Type modelType;
                var currentModel = GetNavalDLCModel(out modelType);
                if (currentModel == null)
                {
                    return 0;
                }

                // Get the CalculateTotalWeightCarried method
                var calculateMethod = modelType.GetMethod("CalculateTotalWeightCarried", 
                    BindingFlags.Public | BindingFlags.Instance);
                
                if (calculateMethod == null)
                {
                    AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: CalculateTotalWeightCarried method not found");
                    return 0;
                }

                // Call the method: CalculateTotalWeightCarried(mobileParty, false, false)
                var parameters = new object[] { mobileParty, false, false };
                var result = calculateMethod.Invoke(currentModel, parameters);
                
                if (result == null)
                {
                    AutoTraderHelpers.PrintDebugMessage("WarsailsHelper: CalculateTotalWeightCarried returned null");
                    return 0;
                }

                float totalWeight = Convert.ToSingle(result);
                
                AutoTraderHelpers.PrintDebugMessage($"WarsailsHelper: Fleet total weight carried = {totalWeight}");
                return totalWeight;
            }
            catch (Exception ex)
            {
                AutoTraderHelpers.PrintDebugMessage($"WarsailsHelper: Error getting fleet total weight: {ex.Message}");
                AutoTraderHelpers.PrintDebugMessage($"WarsailsHelper: Stack trace: {ex.StackTrace}");
                return 0;
            }
        }
    }
}
