using Il2Cpp;
using MelonLoader;
using PantheonAddonFramework;
using PantheonAddonFramework.Configuration;
using PantheonAddonFramework.Models;
using PantheonAddonFramework.UI;
using System;
using System.Collections.Generic;

namespace MacroHotKeys;

[AddonMetadata("Macro HotKeys", "Xanera", "Allows the use of in-game macros via hotkeys")]
public class MacroHotKeys : Addon
{
    private bool _isActive = true;

    // Create a mapping of keypad numbers to macro names
    // Use int or the appropriate enum type instead of KeyCodes for the dictionary key
    private readonly Dictionary<int, string> _keypadMacroMap = new Dictionary<int, string>();

    public MacroHotKeys()
    {
        var macroBar = UIMacroBar.Instance;

        
    
    }

    public override void OnCreate()
    {
        LifecycleEvents.OnUpdate.Subscribe(OnUpdate);
    }
    private bool _hasFetchedMacros = false;
private void OnUpdate()
    {
        if (!_isActive)
        {
            return;
        }
        if (_hasFetchedMacros)
        {
            ProcessKeypadMacros();
            return; // Skip if we've already fetched macros
        }
        try
        {
            int i = 257;
            var mList = Macros.GetAllMacros();
            if (mList != null)
            {
                foreach (var macro in mList)
                {
                    MelonLogger.Msg("Macro found - " + macro.Name + ", assigned to key " + i.ToString());
                    _keypadMacroMap.Add(i, macro.Name);
                    i++;
                }
                _hasFetchedMacros = true; // Mark as fetched so we don't run again
            }
            ProcessKeypadMacros();
        }
        catch(NullReferenceException)
        {
            //MelonLogger.Msg("Character Not Loaded - List is empty");
        }
        

    }

    // Method to check for key presses and activate macros
    private void ProcessKeypadMacros()
    {
        foreach (var mapping in _keypadMacroMap)
        {
            if (Keyboard.IsKeyDown(mapping.Key))
            {
                var macro = Macros.GetByName(mapping.Value);
                macro?.Activate(false);
            }
        }
    }

    public override void Enable()
    {
        _isActive = true;
    }

    public override void Disable()
    {
        _isActive = false;
    }

    public override IEnumerable<IConfigurationValue> GetConfiguration()
    {
        return Array.Empty<IConfigurationValue>();
    }

    public override void Dispose()
    {
        LifecycleEvents.OnUpdate.Unsubscribe(OnUpdate);
    }
}